float4x4 matWorld;                  // World
float4x4 matWorldViewProj;          // World * View * Projection matrix
float4x4 matWorldReflectedViewProj; // World * Reflected View * Projection matrix
float3 cameraPos;                   // Camera position
float waveLength;
float waveHeight;
float totalTime;
float windForce;
float4x4 windDirection; 
float4 dullColor;
float dullBlendFactor;
float4 specularColor;
float3 lightDir;
float specularPower;


texture RefractionMap;
sampler RefractionMapSampler = sampler_state
{
  Texture = <RefractionMap>;
  MinFilter = Linear;
  MagFilter = Linear;
  MipFilter = Linear;
  AddressU = mirror; 
  AddressV = mirror;
  AddressW = mirror;
};

texture ReflectionMap;
sampler ReflectionMapSampler = sampler_state
{
  Texture = <ReflectionMap>;
  MinFilter = Linear;
  MagFilter = Linear;
  MipFilter = Linear;
  AddressU = mirror; 
  AddressV = mirror;
  AddressW = mirror;
};

texture WaterNormalMap;
sampler WaterNormalMapSampler = sampler_state
{
  Texture = <WaterNormalMap>;
  MinFilter = Linear;
  MagFilter = Linear;
  MipFilter = Linear;
  AddressU = mirror; 
  AddressV = mirror;
  AddressW = mirror;
};


struct VS_OUTPUT 
{
    float4 Position                    : POSITION;   
    float4 ReflectionMapSamplingPos    : TEXCOORD1;
    float4 RefractionMapSamplingPos    : TEXCOORD3;
    float2 NormalMapSamplingPos        : TEXCOORD2;
    float4 PosWorld                  : TEXCOORD4;
};

 
VS_OUTPUT WaterVS(float4 Pos : POSITION, float2 Tex: TEXCOORD)
{
     VS_OUTPUT Output;     
     
     Output.Position = mul(Pos, matWorldViewProj);
     Output.ReflectionMapSamplingPos = mul(Pos, matWorldReflectedViewProj);
     Output.RefractionMapSamplingPos = mul(Pos, matWorldViewProj);
     Output.PosWorld = mul(Pos, matWorld);

     // wind
     float4 absoluteTexCoords = float4(Tex, 0, 1);
     float4 rotatedTexCoords = mul(absoluteTexCoords, windDirection);
     float2 moveVector = float2(0, 0.0001);
     Output.NormalMapSamplingPos = rotatedTexCoords.xy/waveLength + totalTime*windForce*moveVector.xy;

     return Output;    
}
 


float4 WaterPS20(VS_OUTPUT psIn) : COLOR
{
   float4 bumpColor = tex2D(WaterNormalMapSampler, psIn.NormalMapSamplingPos);
   float2 perturbation = waveHeight*(bumpColor.rg - 0.5f);
  
   // computing reflection
   float2 projectedTexCoords;
   projectedTexCoords.x = psIn.ReflectionMapSamplingPos.x/psIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
   projectedTexCoords.y = -psIn.ReflectionMapSamplingPos.y/psIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
   float2 perturbatedTexCoords = projectedTexCoords + perturbation;
   float4 reflectiveColor = tex2D(ReflectionMapSampler, perturbatedTexCoords);
   
   // computing refraction
   float2 projectedRefrTexCoords;
   projectedRefrTexCoords.x = psIn.RefractionMapSamplingPos.x/psIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
   projectedRefrTexCoords.y = -psIn.RefractionMapSamplingPos.y/psIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
   float2 perturbatedRefrTexCoords = projectedRefrTexCoords + perturbation;
   float4 refractiveColor = tex2D(RefractionMapSampler, perturbatedRefrTexCoords);

   // fresnel
   float3 eyeVector = normalize(cameraPos-psIn.PosWorld);
   float3 normalVector = float3(0,1,0);
   float fresnelTerm = dot(eyeVector, normalVector);
   float4 fresnelColor = refractiveColor*fresnelTerm + reflectiveColor*(1-fresnelTerm); 
   
   // adding some dull
   return dullBlendFactor*dullColor + (1-dullBlendFactor)*fresnelColor; 
}
 

float4 WaterPS(VS_OUTPUT psIn) : COLOR
{

   float4 bumpColor = tex2D(WaterNormalMapSampler, psIn.NormalMapSamplingPos);
   float2 perturbation = waveHeight*(bumpColor.rg - 0.5f);

  
   // computing reflection
   float2 projectedTexCoords;
   projectedTexCoords.x = psIn.ReflectionMapSamplingPos.x/psIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
   projectedTexCoords.y = -psIn.ReflectionMapSamplingPos.y/psIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
   float2 perturbatedTexCoords = projectedTexCoords + perturbation;
   float4 reflectiveColor = tex2D(ReflectionMapSampler, perturbatedTexCoords);
   
   // computing refraction
   float2 projectedRefrTexCoords;
   projectedRefrTexCoords.x = psIn.RefractionMapSamplingPos.x/psIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
   projectedRefrTexCoords.y = -psIn.RefractionMapSamplingPos.y/psIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
   float2 perturbatedRefrTexCoords = projectedRefrTexCoords + perturbation;
   float4 refractiveColor = tex2D(RefractionMapSampler, perturbatedRefrTexCoords);

   // fresnel
   float3 eyeVector = normalize(cameraPos-psIn.PosWorld);
   float3 normalVector = float3(0,1,0);
   float fresnelTerm = dot(eyeVector, normalVector);
   float4 fresnelColor = refractiveColor*fresnelTerm + reflectiveColor*(1-fresnelTerm); 
   
   // adding some dull
   float4 dullColor = dullBlendFactor*dullColor + (1-dullBlendFactor)*fresnelColor; 
   
   
   // adding specular light (phong)
   float3 nLight = normalize(lightDir);
   float diff = saturate(dot(normalVector, nLight));
   float3 reflect = normalize(2 * diff * normalVector - nLight);
   float4 specular = specularColor * pow(saturate(dot(reflect,eyeVector)),specularPower);

   
   return specular + dullColor;
}

technique Water20
{
   pass Pass0
   {
       VertexShader = compile vs_2_0 WaterVS();
       PixelShader = compile ps_2_0 WaterPS20();
   }
}

technique Water
{
   pass Pass0
   {
       VertexShader = compile vs_3_0 WaterVS();
       PixelShader = compile ps_3_0 WaterPS();
   }
}




