float4x4 matWorldViewProj;
float4x4 matWorld;
float4 vecLightDir;
float4 sunColor;
float light = 0.8f;
float4 cameraPosition;
float specular = 0.5f;

texture SkyboxTexture; 
samplerCUBE SkyboxSampler : register (s1)  = sampler_state 
{ 
   texture = <SkyboxTexture>; 
   magfilter = LINEAR; 
   minfilter = LINEAR; 
   mipfilter = LINEAR; 
   AddressU = Mirror; 
   AddressV = Mirror; 
};

texture Texture;
sampler Sampler  = sampler_state
{   
    Texture = <Texture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

struct VS_INPUT
{
	float4 Pos : POSITION0;
    float3 Normal   : NORMAL0;
    float2 Tex : TEXCOORD0;
    float4 Col : COLOR0;
};

struct VS_OUTPUT
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;    	
    float4 Norm : TEXCOORD1;   
    float3 Refl : TEXCOORD2; 
    float4 Color : COLOR0;
};

VS_OUTPUT VS(in VS_INPUT In)
{
    VS_OUTPUT Out = (VS_OUTPUT)0; 

	Out.Tex = In.Tex;
    Out.Norm = normalize(mul(In.Normal, matWorld));
    float3 worldPos = mul(In.Pos, matWorld).xyz;
    float3 vecToEye = cameraPosition.xyz - worldPos;
    Out.Pos = mul(In.Pos, matWorldViewProj);
    Out.Color = In.Col;    
    Out.Refl = reflect(-normalize(vecToEye), Out.Norm);
    
	return Out;
}

float4 PS(float4 pos: POSITION, float2 tex: TEXCOORD0, float3 Norm : TEXCOORD1, float3 refl: TEXCOORD2, float4 col : COLOR0) : COLOR
{
	float4 color = tex2D(Sampler, tex);
	float spec = color.w;
	float4 result = (1.0f - spec)*light*color*sunColor*(0.9f + 0.8f*saturate(dot(vecLightDir, Norm))) + spec*texCUBE(SkyboxSampler, refl);	
	result.w = 0.5f;
	return result;
	//return texCUBE(SkyboxSampler, normalize(refl));	
}

technique TReflection
{
    pass P0
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }  
}

