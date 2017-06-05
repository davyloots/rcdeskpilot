//
// Based on shader by Habib from http://habib.wikidot.com/
//
//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float4x4 xReflectionView;
float4x4 xWindDirection;
float3 xCamPos;
float3 xWaterPos;
float3 SunPosition;
float xAmbient;
bool xEnableLighting;

// variables for the BUMP MAP
float WaveLength;
float WaveHeight;


 float xTime;
 float xWindForce;
 float xDrawMode;
 
 //fresnel  intput variables
 int fresnelMode;
 
 //specular input variables
 float specPerturb;
 float specPower;
 
 //dullblend factor
 float xdullBlendFactor;
 
  //dullblend factor
 int xEnableTextureBlending;


//------- Texture Samplers --------
Texture xReflectionMap;

sampler ReflectionSampler = sampler_state { texture = <xReflectionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xRefractionMap;

sampler RefractionSampler = sampler_state { texture = <xRefractionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

/////// BUMP MAP
Texture xWaterBumpMap;

sampler WaterBumpMapSampler = sampler_state { texture = <xWaterBumpMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Water --------

struct WaterVertexToPixel
{
    float4 Position                 : POSITION;    
    float4 ReflectionMapSamplingPos    : TEXCOORD1;
    float2 BumpMapSamplingPos        : TEXCOORD2;
    float4 RefractionMapSamplingPos : TEXCOORD3;
    float4 Position3D                : TEXCOORD4;
};

struct WaterPixelToFrame
{
    float4 Color : COLOR0;
};

WaterVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex: TEXCOORD)
{
    WaterVertexToPixel Output = (WaterVertexToPixel)0;
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    float4x4 preReflectionViewProjection = mul (xReflectionView, xProjection);
    float4x4 preWorldReflectionViewProjection = mul (xWorld, preReflectionViewProjection);
    
    Output.Position = mul(inPos, preWorldViewProjection);
    Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);
    Output.RefractionMapSamplingPos = mul(inPos, preWorldViewProjection);
    Output.Position3D = inPos;


     float4 absoluteTexCoords = float4(inTex, 0, 1);
     float4 rotatedTexCoords = mul(absoluteTexCoords, xWindDirection);
     float2 moveVector = float2(0, 1);
     
     // moving the water
     Output.BumpMapSamplingPos = rotatedTexCoords.xy/WaveLength + xTime*xWindForce*moveVector.xy;
         
    return Output;    
}

WaterPixelToFrame WaterPS(WaterVertexToPixel PSIn)
{
    WaterPixelToFrame Output = (WaterPixelToFrame)0;
    
    float2 ProjectedTexCoords;
    ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;

    // sampling the bump map
    float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
    
    // perturbating the color
    float2 perturbation = WaveHeight*(bumpColor.rg - 0.5f);
    
    // the final texture coordinates
    float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);

    float2 ProjectedRefrTexCoords;
    ProjectedRefrTexCoords.x = PSIn.RefractionMapSamplingPos.x/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedRefrTexCoords.y = -PSIn.RefractionMapSamplingPos.y/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
    float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;
    float4 refractiveColor = tex2D(RefractionSampler, perturbatedRefrTexCoords);

    float3 eyeVector = normalize(xCamPos - xWaterPos - PSIn.Position3D);
    float3 normalVector = float3(0,0,1);

/////////////////////////////////////////////////
// FRESNEL TERM APPROXIMATION
/////////////////////////////////////////////////
	float fresnelTerm = (float)0;

	if ( fresnelMode == 1 )
	{
		fresnelTerm = 0.02+0.97f*pow((1-dot(eyeVector, normalVector)),5);
	} else
	if ( fresnelMode == 0 )
	{
		fresnelTerm = 1-dot(eyeVector, normalVector)*1.3f;
	} else
	if ( fresnelMode == 2 )
	{
		float fangle = 1.0f+dot(eyeVector, normalVector);
		fangle = pow(fangle,5);  
	   // fresnelTerm = fangle*50;
		fresnelTerm = 1/fangle;
	}

	//    fresnelTerm = (1/pow((fresnelTerm+1.0f),5))+0.2f; // 
	    
	//Hardness factor - user input
	fresnelTerm = fresnelTerm * xDrawMode;
    
	//just to be sure that the value is between 0 and 1;
	fresnelTerm = fresnelTerm < 0? 0 : fresnelTerm;
	fresnelTerm = fresnelTerm > 1? 1 : fresnelTerm;
    
	// creating the combined color
	float4 combinedColor = refractiveColor*(1-fresnelTerm) + reflectiveColor*(fresnelTerm);

	/////////////////////////////////////////////////
	// WATER COLORING
	/////////////////////////////////////////////////

		float4 dullColor = float4(0.1f, 0.1f, 0.2f, 1.0f);
		//float4 dullColor = float4(0.0f, 1.0f, 1.4f, 1.0f);
	    
	   // float dullBlendFactor = 0.3f;
	   // float dullBlendFactor = 0.1f;
		float dullBlendFactor = xdullBlendFactor;

		// eredeti víz
		//Output.Color = dullBlendFactor*dullColor + (1-dullBlendFactor)*combinedColor;
	    
		// kicsit sötétebb a tükörkép, mint az eredeti...
		Output.Color = (dullBlendFactor*dullColor + (1-dullBlendFactor)*combinedColor);
	//Output.Color = (dullBlendFactor*dullColor + (1-dullBlendFactor)*combinedColor)*1.25f;



/////////////////////////////////////////////////
// Specular Highlights
/////////////////////////////////////////////////

  float4 speccolor;

	// float3 lightSourceDir = normalize(float3(0.1f,0.6f,0.5f));
	float3 sunDir = normalize(SunPosition);

	float3 halfvec = normalize(eyeVector+sunDir+float3(perturbation.x*specPerturb,perturbation.y*specPerturb,0));
	
	float3 temp = 0;

	temp.x = pow(dot(halfvec,normalVector),specPower);
	
	speccolor = float4(0.98,0.97,0.7,0.6);
	
	speccolor = speccolor*temp.x;

	speccolor = float4(speccolor.x*speccolor.w,speccolor.y*speccolor.w,speccolor.z*speccolor.w,0);

	Output.Color = Output.Color + speccolor;

    return Output;
}

technique Water
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 WaterVS();
        PixelShader = compile ps_2_0 WaterPS();
    }
}