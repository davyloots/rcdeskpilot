float Distance;
int Background;
texture Texture;
sampler TextureSampler  = sampler_state
{   
    Texture = <Texture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

texture DepthMap;
sampler DepthMapSampler  = sampler_state
{   
    Texture = <DepthMap>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

float4 PS(float2 tex: TEXCOORD0) : COLOR
{
	float4 color = tex2D(TextureSampler, tex);
    float4 depth = tex2D(DepthMapSampler, tex);
    if (Background == 1)
	{
	    if (depth[0] < Distance)
		{
			return color;
		}
		else
		{
			return float4(0.0,0.0,0.0,0.0);
		}
	}
	else
	{
		if (depth[0] >= Distance)
		{
			return color;
		}
		else
		{
			return float4(0.0,0.0,0.0,0.0);
		}
	}
}

technique TPhoto
{
    pass P0
    {
        // shaders
        PixelShader = compile ps_2_0 PS();
        // texture
        //Texture[0] = (Tex0);       
    }  
}