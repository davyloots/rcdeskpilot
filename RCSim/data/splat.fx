float4x4 matViewProj;         // View * Projection matrix
float4x4 matWorld;            // World matrix
float4x4 matInvertTransposeWorld;   // transpose invert world
float3 cameraPos;             // camera position
float blendSqDistance;        // blending squared distance
float blendSqWidth;           // 1/blending width
float nearRepeat;             // near splats repetition
float farRepeat;              // far splats repetition
float nearFactor2;             // near splats repetition
float farFactor2;              // far splats repetition
float nearFactor3;             // near splats repetition
float farFactor3;              // far splats repetition
float nearFactor4;             // near splats repetition
float farFactor4;              // far splats repetition
float3 sunPosition;           // sun position
float underwater;             // used if water present
float4 dullColor;             // used only underwater
float dullBlendFactor;        // used only underwater
float ambientFactor;
float sunFactor;

texture NormalMapTexture;
sampler NormalMapSampler  = sampler_state
{   
    Texture = <NormalMapTexture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};


texture AlphaTexture;
sampler AlphaTextureSampler  = sampler_state
{   
    Texture = <AlphaTexture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

texture DetailTexture1;
sampler DetailTexture1Sampler = sampler_state
{   
    texture = <DetailTexture1>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

texture DetailTexture2;
sampler DetailTexture2Sampler = sampler_state
{   
    texture = <DetailTexture2>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear; 
};

texture DetailTexture3;
sampler DetailTexture3Sampler = sampler_state
{   
    texture = <DetailTexture3>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear; 
};


texture DetailTexture4;
sampler DetailTexture4Sampler = sampler_state
{   
    texture = <DetailTexture4>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear; 
};


struct VS_OUTPUT 
{
    float4 Position : POSITION;
    float2 TextureCoord : TEXCOORD0;
    float Distance : TEXCOORD1;
    float2 nearTextureCoord : TEXCOORD2;
    float2 farTextureCoord : TEXCOORD3;
    float3 sunLight : TEXCOORD4;
    float3 View : TEXCOORD5;
};

 
float ComputeDistance(float3 position1, float3 position2)
{
   float dist = mul(position1.x-position2.x,position1.x-position2.x);
   dist += mul(position1.y-position2.y,position1.y-position2.y);
   dist += mul(position1.z-position2.z,position1.z-position2.z);
   return dist;
}

VS_OUTPUT TextureSplattingVS( float4 Pos : POSITION, float2 Tex : TEXCOORD, float3 Norm : NORMAL,  float3 Tangent : TANGENT)
{
    VS_OUTPUT Output;
    
    
    float4x4 matWorldViewProj = mul(matWorld, matViewProj);
    Output.Position = mul(Pos, matWorldViewProj);
    Output.TextureCoord = Tex;
    Output.nearTextureCoord = Tex*nearRepeat;
    Output.farTextureCoord = Tex*farRepeat;
    
    float3 worldPos = mul(Pos, matWorld);
    Output.Distance = ComputeDistance(cameraPos, worldPos);
    
    float3x3 worldToTangentSpace;
    worldToTangentSpace[0] = mul(Tangent, matWorld);
    worldToTangentSpace[1] = mul(cross(Norm, Tangent), matWorld);
    worldToTangentSpace[2] = mul(Norm, matWorld);
    
    Output.sunLight = mul(worldToTangentSpace, sunPosition);
    Output.View = mul(worldToTangentSpace, float4(cameraPos,1) - worldPos);
     
    return Output;    
}


float4 TextureSplattingPS(VS_OUTPUT psIn) : COLOR
{
	//float3 sunPos = float3(-1.0, 1.0, -1.0);
	float3 sunPos = sunPosition;
    float3 nLight = normalize(sunPos);
    float3 nView = normalize(psIn.View);


    // blending terrain (splatting)
    float blendFactor = clamp((psIn.Distance-blendSqDistance)*blendSqWidth, 0, 1);    
    
    float4 splat = tex2D(AlphaTextureSampler, psIn.TextureCoord);

    float4 farColor;
    farColor = tex2D(DetailTexture1Sampler, psIn.farTextureCoord) * splat[0];
    farColor += tex2D(DetailTexture2Sampler, psIn.farTextureCoord*farFactor2) * splat[1];
    farColor += tex2D(DetailTexture3Sampler, psIn.farTextureCoord*farFactor3) * splat[2];
    farColor += tex2D(DetailTexture4Sampler, psIn.farTextureCoord*farFactor4) * splat[3];

    float4 nearColor;
    nearColor = tex2D(DetailTexture1Sampler, psIn.nearTextureCoord) * splat[0];
    nearColor += tex2D(DetailTexture2Sampler, psIn.nearTextureCoord*nearFactor2) * splat[1];
    nearColor += tex2D(DetailTexture3Sampler, psIn.nearTextureCoord*nearFactor3) * splat[2];
    nearColor += tex2D(DetailTexture4Sampler, psIn.nearTextureCoord*nearFactor4) * splat[3];
    
    // merging colors
    float4 terrainColor = farColor*blendFactor + nearColor*(1-blendFactor);
   
    // uncompressing terrain normal map  
    float3 normal = tex2D(NormalMapSampler, psIn.TextureCoord) * 2 - 1;
    
    // diffuse for whole terrain
    float diffuse = saturate(dot(normal, nLight));


    float4 finalColor = ambientFactor * terrainColor + sunFactor * terrainColor * diffuse;
    finalColor.w = 1.0f; // alpha preserve
    return finalColor;
    
}

technique TextureSplatting
{
    pass P0
    {          
        VertexShader = compile vs_2_0 TextureSplattingVS();
        PixelShader  = compile ps_2_0 TextureSplattingPS();
    }

}


