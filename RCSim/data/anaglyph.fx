float3 TexelSize;

float4 Resample(float4 position)
{
	position.xy += float2(-TexelSize.x, TexelSize.y) * position.ww;
	return position;
}

float3x3 leftFilter = { 0.0, 0.7, 0.3, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
float3x3 rightFilter = { 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 };

float RedGamma = 1.5;

texture LeftTexture;
sampler LeftSampler = sampler_state
{   
    Texture = <LeftTexture>;
    MinFilter = Point;
    MagFilter = Point;
};

texture RightTexture;
sampler RightSampler = sampler_state
{   
    Texture = <RightTexture>;
    MinFilter = Point;
    MagFilter = Point;
};

struct VS_INPUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD;
};

struct VS_OUTPUT 
{
    float2 TexCoord : TEXCOORD0;
};

#define PS_INPUT VS_OUTPUT

VS_OUTPUT VS(VS_INPUT IN, out float4 outPosition : POSITION)
{
	outPosition = Resample(IN.Position);
	
	VS_OUTPUT OUT;
	OUT.TexCoord = IN.TexCoord;
	
	return OUT;
} 

float4 PS(PS_INPUT IN) : COLOR
{
	float3 left = tex2D(LeftSampler, IN.TexCoord).rgb;
	float3 right = tex2D(RightSampler, IN.TexCoord).rgb;
	float3 color = mul(leftFilter, left) + mul(rightFilter, right);
	color.r = pow(color.r, 1 / RedGamma);
	
	return float4(color, 1);
}

technique ShaderModel2
{
    pass Main
    {          
        AlphaBlendEnable = False;
        AlphaBlendEnable = False;
                
        VertexShader = compile vs_2_0 VS();
        PixelShader  = compile ps_2_0 PS();
    }

}


