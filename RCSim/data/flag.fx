float4x4 matWorldViewProj: WORLDVIEWPROJECTION;
float4x4 matWorld  : WORLD;
float time;
float windSpeed;
float4 vecLightDir;
float4 sunColor;
float light = 0.8f;

texture Texture;
sampler Sampler  = sampler_state
{   
    Texture = <Texture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

struct VS_OUTPUT
{
    float4 Pos : POSITION;
    float2 tex : TEXCOORD0;    
    float3 Norm : TEXCOORD1;    
};

VS_OUTPUT VS(float4 Pos : POSITION, float2 tex : TEXCOORD0, float3 Normal : NORMAL)
{
    VS_OUTPUT Out = (VS_OUTPUT)0; 

    float timeangle=(time%360)*3.0f;
    float angleinv = windSpeed + 2.0f - Pos.y;
    float angle = 1.57f/angleinv;

	float displacement = sin(Pos.x*(10.0f+windSpeed)+timeangle+3.0*Pos.y);
	displacement += sin(Pos.y/0.2f+timeangle);	
    Pos.z = displacement;
    Pos.z *= Pos.x * 0.09f;
    Pos.y -= Pos.x*sin(angle);
    Pos.x -= Pos.x*sin(angle);

    Out.Pos = Out.Pos + mul(Pos , matWorldViewProj); 	
	Out.Norm = Normal; //normalize(mul(Normal, matWorld)); // transform       Normal and normalize it 
	Out.Norm.x -= 0.5f*displacement;	
	Out.Norm = normalize(Out.Norm);
    Out.tex = tex; 

    return Out;
}

float4 PS(float2 tex: TEXCOORD0, float3 Norm : TEXCOORD1) : COLOR
{
	float4 color = tex2D(Sampler, tex);
     
    return light*color*sunColor*(1.0f + 0.5f*saturate(dot(vecLightDir, Norm)));
}

technique TFlag
{
    pass P0
    {
        // shaders
        VertexShader = compile vs_1_1 VS();
        PixelShader = compile ps_1_1 PS();
        // texture
        //Texture[0] = (Tex0);       
    }  
}

