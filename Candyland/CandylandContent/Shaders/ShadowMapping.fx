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

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

#define MaxBones 60 
 
float4x3 Bones[MaxBones]; 

float4x4 world;
float4x4 lightViewProjection;

struct AppToVertex
{
    float4 Position : POSITION0;            // The position of the vertex
    float3 Normal : NORMAL0;                // The vertex's normal
    float2 TextureCoordinate : TEXCOORD0;    // The texture coordinate of the vertex
	int4   Indices  : BLENDINDICES0; 
    float4 Weights  : BLENDWEIGHT0; 
};

//-----------------------------------------------------------------------------
// Vertex shaders.
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

void VS_CreateShadowMap(AppToVertex vin,
                        out float4 outPosition : POSITION,
                        out float2 outDepth    : TEXCOORD)
{   
    outPosition = mul(vin.Position, mul(world, lightViewProjection));
    outDepth = outPosition.zw;
}

void VS_CreateShadowMapAnimated(AppToVertex vin,
                        out float4 outPosition : POSITION,
                        out float2 outDepth    : TEXCOORD)
{   
	Skin(vin, 4); 
    VS_CreateShadowMap(vin, outPosition, outDepth);
}


//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

void PS_CreateShadowMap(in  float2 inDepth  : TEXCOORD,
                        out float4 outColor : COLOR)
{
    outColor = float4(inDepth.x / inDepth.y, 0.0f, 0.0f, 1.0f);
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique CreateShadowMap
{
    pass
    {
        VertexShader = compile vs_2_0 VS_CreateShadowMap();
        PixelShader = compile ps_2_0 PS_CreateShadowMap();
		CullMode = CW;
		AlphaBlendEnable = false;
    }
}

technique CreateShadowMapAnimated
{
    pass
    {
        VertexShader = compile vs_2_0 VS_CreateShadowMapAnimated();
        PixelShader = compile ps_2_0 PS_CreateShadowMap();
		CullMode = CW;
		AlphaBlendEnable = false;
    }
}