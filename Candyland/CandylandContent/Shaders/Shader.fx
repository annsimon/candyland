//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

#define MaxBones 60 
 
float4x3 Bones[MaxBones]; 
float4x4 world;
float4x4 view;
float4x4 projection;
float4x4 worldInverseTranspose;
float4x4 lightViewProjection;
float4x4 textureScaleBias;

float3 lightDir;
float4 lightColor;
float4 materialAmbient;
float4 materialDiffuse;

float depthBias;
float texelSize;

float4 LineColor = float4(0, 0, 0, 1);
float LineThickness = .03;

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

texture colorMap;
sampler colorMapSampler = sampler_state
{
	Texture = <colorMap>;
    MinFilter = Anisotropic;
	MagFilter = Linear;
    MipFilter = Linear;
    MaxAnisotropy = 16;
};

texture shadowMap;
sampler shadowMapSampler = sampler_state
{
    Texture = <shadowMap>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = None;
    AddressU = Clamp;
    AddressV = Clamp;
};
 
//-----------------------------------------------------------------------------
// Data Structures (for toon shader)
//-----------------------------------------------------------------------------
struct AppToVertex
{
    float4 Position : POSITION0;            // The position of the vertex
    float3 Normal : NORMAL0;                // The vertex's normal
    float2 TextureCoordinate : TEXCOORD0;    // The texture coordinate of the vertex
	int4   Indices  : BLENDINDICES0; 
    float4 Weights  : BLENDWEIGHT0; 
};
 
// The structure used to store information between the vertex shader and the
// pixel shader
struct VertexToPixel
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};

//-----------------------------------------------------------------------------
//Skinning.
//-----------------------------------------------------------------------------
 
 void Skin(inout AppToVertex vin, uniform int boneCount) 
{ 
    float4x3 skinning = 0; 
 
    [unroll] 
    for (int i = 0; i < boneCount; i++) 
    { 
        skinning += Bones[vin.Indices[i]] * vin.Weights[i]; 
    } 
 
    vin.Position.xyz = mul(vin.Position, skinning); 
    vin.Normal = mul(vin.Normal, (float3x3)skinning); 
}  

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

void VS_Shaded(in  float4 inPosition  : POSITION,
	            in  float2 inTexCoord  : TEXCOORD,
	            in  float3 inNormal    : NORMAL,
			    out float4 outPosition : POSITION,
			    out float2 outTexCoord : TEXCOORD0,
				out float3 outNormal   : TEXCOORD1,
				out float3 outLightDir : TEXCOORD2)
{
	float4x4 worldviewprojection = mul(mul(world, view), projection);
	
	outPosition = mul(inPosition, worldviewprojection);
	outTexCoord = inTexCoord;
	outNormal = mul(inNormal, (float3x3)world);
	outLightDir = -lightDir;
}

void VS_ShadedAndAnimated(AppToVertex input,
				out float4 outPosition : POSITION,
			    out float2 outTexCoord : TEXCOORD0,
				out float3 outNormal   : TEXCOORD1,
				out float3 outLightDir : TEXCOORD2)
{
    Skin(input, 4); 
	VS_Shaded(input.Position, input.TextureCoordinate, input.Normal, outPosition, outTexCoord, outNormal, outLightDir);
}

void VS_ShadedWithShadows(in  float4 inPosition        : POSITION,
	                    in  float2 inTexCoord        : TEXCOORD,
	                    in  float3 inNormal          : NORMAL,
			            out float4 outPosition       : POSITION,
			            out float4 outLightSpacePos  : TEXCOORD0,
			            out float2 outShadowTexCoord : TEXCOORD1,
			            out float2 outTexCoord       : TEXCOORD2,
				        out float3 outNormal         : TEXCOORD3,
				        out float3 outLightDir       : TEXCOORD4)
{
	float4x4 worldviewprojection = mul(mul(world, view), projection);
	float4 lightSpacePos = mul(mul(inPosition, world), lightViewProjection);
	float4 shadowCoord = mul(lightSpacePos, textureScaleBias);
					
	outPosition = mul(inPosition, worldviewprojection);
	outLightSpacePos = lightSpacePos;
	outShadowTexCoord = shadowCoord.xy / shadowCoord.w;
	outTexCoord = inTexCoord;
	outNormal = mul(inNormal, (float3x3)world);
	outLightDir = -lightDir;
}

void VS_ShadedWithShadowsAndAnimated(AppToVertex input,
			               out float4 outPosition       : POSITION,
			               out float4 outLightSpacePos  : TEXCOORD0,
			               out float2 outShadowTexCoord : TEXCOORD1,
			               out float2 outTexCoord       : TEXCOORD2,
				           out float3 outNormal         : TEXCOORD3,
				           out float3 outLightDir       : TEXCOORD4)
{
    Skin(input, 4);
	VS_ShadedWithShadows(input.Position, input.TextureCoordinate, input.Normal, outPosition, 
							outLightSpacePos, outShadowTexCoord, outTexCoord, outNormal, outLightDir);
}

VertexToPixel OutlineVertexShader(AppToVertex input)
{
    VertexToPixel output = (VertexToPixel)0;
 
    float4 originalLocation = mul(mul(mul(input.Position, world), view), projection);
    float4 normal = mul(mul(mul(input.Normal, world), view), projection);
    output.Position = originalLocation + (mul(LineThickness, normal));
 
    return output;
}

// The vertex shader that does the outlines
VertexToPixel OutlineVertexShaderAnimated(AppToVertex input)
{
	Skin(input, 4); 
	return OutlineVertexShader(input);
}

//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

float PS_ShadowMapLookup(sampler shadowMap, float2 texCoord, float depth)
{
	return (tex2D(shadowMap, texCoord).r + depthBias < depth) ? 0.0f : 1.0f;
}

float PS_ShadowMapLookup(sampler shadowMap, float2 texCoord, float2 offset, float depth)
{
	return (tex2D(shadowMap, texCoord + offset * texelSize).r + depthBias < depth) ? 0.0f : 1.0f;
}

void PS_Shaded(in  float2 inTexCoord : TEXCOORD0,
                in  float3 inNormal   : TEXCOORD1,
                in  float3 inLightDir : TEXCOORD2,
				out float4 outColor   : COLOR)
{
	float3 l = normalize(inLightDir);
	float3 n = normalize(inNormal);
	float intensity = saturate(dot(n, l));
	
	outColor = (materialAmbient * lightColor) +
	           (materialDiffuse * lightColor * intensity);
			   
	outColor *= tex2D(colorMapSampler, inTexCoord);

    if(intensity < 0)
        intensity = 0;

	if (intensity > 0.6)
        outColor = float4(0.95,0.95,0.95,1.0) * outColor;
    else if (intensity > 0.3)
        outColor = float4(0.9,0.9,0.9,1.0) * outColor;
    else if (intensity > 0.01)
        outColor = float4(0.85,0.85,0.85,1.0) * outColor;
    else
        outColor = float4(0.75,0.75,0.75,1.0) * outColor;
}

void PS_ShadedWithShadows(in  float4 inLightSpacePos  : TEXCOORD0,
                           in  float2 inShadowTexCoord : TEXCOORD1,
                           in  float2 inTexCoord       : TEXCOORD2,
                           in  float3 inNormal         : TEXCOORD3,
                           in  float3 inLightDir       : TEXCOORD4,
				           out float4 outColor         : COLOR)
{
	float3 l = normalize(inLightDir);
	float3 n = normalize(inNormal);
	float nDotL = saturate(dot(n, l));
	
	float depth = inLightSpacePos.z / inLightSpacePos.w;
    float shadowOcclusion = PS_ShadowMapLookup(shadowMapSampler, inShadowTexCoord, depth);
    
    outColor = (materialAmbient * lightColor) +
	           (materialDiffuse * lightColor * nDotL) * shadowOcclusion;
			   
	outColor *= tex2D(colorMapSampler, inTexCoord);
}

void PS_ShadedWithShadowsPCF2x2(in  float4 inLightSpacePos  : TEXCOORD0,
                                 in  float2 inShadowTexCoord : TEXCOORD1,
                                 in  float2 inTexCoord       : TEXCOORD2,
                                 in  float3 inNormal         : TEXCOORD3,
				                 in  float3 inLightDir       : TEXCOORD4,
				                 out float4 outColor         : COLOR)
{
	float3 l = normalize(inLightDir);
	float3 n = normalize(inNormal);
	float nDotL = saturate(dot(n, l));

    float depth = inLightSpacePos.z / inLightSpacePos.w;
    float shadowOcclusion = 0.0f;
    
    shadowOcclusion += PS_ShadowMapLookup(shadowMapSampler, inShadowTexCoord, float2(0.0f, 0.0f), depth);
    shadowOcclusion += PS_ShadowMapLookup(shadowMapSampler, inShadowTexCoord, float2(1.0f, 0.0f), depth);
    
    shadowOcclusion += PS_ShadowMapLookup(shadowMapSampler, inShadowTexCoord, float2(0.0f, 1.0f), depth);
    shadowOcclusion += PS_ShadowMapLookup(shadowMapSampler, inShadowTexCoord, float2(1.0f, 1.0f), depth);   
    
    shadowOcclusion /= 4.0f;
            
    outColor = (materialAmbient * lightColor) +
	           (materialDiffuse * lightColor * nDotL) * shadowOcclusion;
			   
	outColor *= tex2D(colorMapSampler, inTexCoord);
}

// The pixel shader for the outline.
float4 OutlinePixelShader(VertexToPixel input) : COLOR0
{
    return LineColor;
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique Shaded
{
	pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShader();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

	pass Pass2
	{
		VertexShader = compile vs_2_0 VS_Shaded();
		PixelShader = compile ps_2_0 PS_Shaded();
		CullMode = CCW;
	}
}

technique ShadedWithShadows
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShader();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

	pass Pass2
    {
        VertexShader = compile vs_2_0 VS_ShadedWithShadows();
        PixelShader = compile ps_2_0 PS_ShadedWithShadows();
		CullMode = CCW;
    }
}

technique ShadedWithShadows2x2PCF
{
	pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShader();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

    pass Pass2
    {
        VertexShader = compile vs_2_0 VS_ShadedWithShadows();
        PixelShader = compile ps_2_0 PS_ShadedWithShadowsPCF2x2();
		CullMode = CCW;
    }
}


//-------------- animated -------------------

technique ShadedAndAnimated
{
	pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShaderAnimated();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

	pass Pass2
	{
		VertexShader = compile vs_2_0 VS_ShadedAndAnimated();
		PixelShader = compile ps_2_0 PS_Shaded();
		CullMode = CCW;
	}
}

technique ShadedWithShadowsAndAnimated
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShaderAnimated();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

	pass Pass2
    {
        VertexShader = compile vs_2_0 VS_ShadedWithShadowsAndAnimated();
        PixelShader = compile ps_2_0 PS_ShadedWithShadows();
		CullMode = CCW;
    }
}

technique ShadedWithShadowsAndAnimated2x2PCF
{
	pass Pass1
    {
        VertexShader = compile vs_2_0 OutlineVertexShaderAnimated();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }

    pass Pass2
    {
        VertexShader = compile vs_2_0 VS_ShadedWithShadowsAndAnimated();
        PixelShader = compile ps_2_0 PS_ShadedWithShadowsPCF2x2();
		CullMode = CCW;
    }
}


// Modified version of: http://www.dhpoware.com/demos/xnaShadowMapping.html
//-----------------------------------------------------------------------------
// Copyright (c) 2008 dhpoware. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------