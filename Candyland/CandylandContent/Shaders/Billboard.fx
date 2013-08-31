//-----------------------------------------------------------------------------
// Copyright (c) 2011 dhpoware. All Rights Reserved.
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
//
// The alpha testing code in the pixel shader is taken from:
// http://create.msdn.com/en-US/education/catalog/sample/billboard
//
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float4x4 world;
float4x4 view;
float4x4 projection;

float2 billboardSize;

//  1 means we should only accept non-transparent pixels.
// -1 means only accept transparent pixels.
float alphaTestDirection = 1.0f;
float alphaTestThreshold = 0.95f;

bool withFog = false;
bool fogMapMode = false;

float4 fogColor;
float fogDensity;
float fogStart;

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

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

void VS_BillboardingCameraAligned(in  float4 inPosition  : POSITION,
                                  in  float4 inTexCoord  : TEXCOORD0,
			                      out float4 outPosition : POSITION,
			                      out float2 outTexCoord : TEXCOORD0,
								  out float outDepth	 : TEXCOORD1)
{
	float4x4 worldViewProjection = mul(mul(world, view), projection);
	
	float2 offset = inTexCoord.zw;
	float3 xAxis = float3(view._11, view._21, view._31);
	float3 yAxis = float3(view._12, view._22, view._32);

	float3 pos = inPosition.xyz + ((offset.x * billboardSize.x) * xAxis) + (offset.y * billboardSize.y * yAxis);

	outPosition = mul(float4(pos, 1.0f), worldViewProjection);
	outTexCoord = inTexCoord.xy;

	if( withFog )
		outDepth = mul(mul(float4(pos, 1.0f), world), view).z;
}

void VS_BillboardingWorldYAxisAligned(in  float4 inPosition  : POSITION,
                                      in  float4 inTexCoord  : TEXCOORD0,
			                          out float4 outPosition : POSITION,
			                          out float2 outTexCoord : TEXCOORD0,
									  out float outDepth	 : TEXCOORD1)
{
	float4x4 worldViewProjection = mul(mul(world, view), projection);
	
	float2 offset = inTexCoord.zw;
	float3 xAxis = float3(view._11, view._21, view._31);
	float3 yAxis = float3(0.0f, 1.0f, 0.0f);

	float3 pos = inPosition.xyz + (offset.x * billboardSize.x * xAxis) + (offset.y * billboardSize.y * yAxis);

	outPosition = mul(float4(pos, 1.0f), worldViewProjection);
	outTexCoord = inTexCoord.xy;

	if( withFog )
		outDepth = mul(mul(pos, world), view).z;
}

//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

float calculateFogFactor(float distance)
{
	float mix = exp( (distance+fogStart) * fogDensity );
	if( mix > 1 ) mix = 1;
	if( fogMapMode )
		if( abs(distance) > 11 ) mix = mix - 0.2f;
	if( mix < 0 ) mix = 0;
	return mix;
}


void PS_Billboarding(in  float2 inTexCoord : TEXCOORD0,
				     in  float inDepth      : TEXCOORD1,
                     out float4 outColor   : COLOR)
{
	outColor = tex2D(colorMapSampler, inTexCoord);

	if( withFog )
	{
		bool clipColor = false;
		if(outColor.a < 0.1f)
			clipColor = true;
		float mix = calculateFogFactor(inDepth);
		outColor = mix * outColor + (1.0f - mix) * fogColor;
		if(clipColor && mix < 0.5f)
			outColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
	}

	// Apply the alpha test.
	clip((outColor.a - alphaTestThreshold) * alphaTestDirection);
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique BillboardingCameraAligned
{
	pass
	{
		VertexShader = compile vs_2_0 VS_BillboardingCameraAligned();
		PixelShader = compile ps_2_0 PS_Billboarding();
	}
}

technique BillboardingWorldYAxisAligned
{
	pass
	{
		VertexShader = compile vs_2_0 VS_BillboardingWorldYAxisAligned();
		PixelShader = compile ps_2_0 PS_Billboarding();
	}
}