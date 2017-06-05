xof 0302txt 0064
template Header {
 <3D82AB43-62DA-11cf-AB39-0020AF71E433>
 WORD major;
 WORD minor;
 DWORD flags;
}

template Vector {
 <3D82AB5E-62DA-11cf-AB39-0020AF71E433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template Coords2d {
 <F6F23F44-7686-11cf-8F52-0040333594A3>
 FLOAT u;
 FLOAT v;
}

template Matrix4x4 {
 <F6F23F45-7686-11cf-8F52-0040333594A3>
 array FLOAT matrix[16];
}

template ColorRGBA {
 <35FF44E0-6C7C-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <D3E16E81-7835-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template IndexedColor {
 <1630B820-7842-11cf-8F52-0040333594A3>
 DWORD index;
 ColorRGBA indexColor;
}

template Boolean {
 <4885AE61-78E8-11cf-8F52-0040333594A3>
 WORD truefalse;
}

template Boolean2d {
 <4885AE63-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template MaterialWrap {
 <4885AE60-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template TextureFilename {
 <A42790E1-7810-11cf-8F52-0040333594A3>
 STRING filename;
}

template Material {
 <3D82AB4D-62DA-11cf-AB39-0020AF71E433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshFace {
 <3D82AB5F-62DA-11cf-AB39-0020AF71E433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template MeshFaceWraps {
 <4885AE62-78E8-11cf-8F52-0040333594A3>
 DWORD nFaceWrapValues;
 Boolean2d faceWrapValues;
}

template MeshTextureCoords {
 <F6F23F40-7686-11cf-8F52-0040333594A3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshMaterialList {
 <F6F23F42-7686-11cf-8F52-0040333594A3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material]
}

template MeshNormals {
 <F6F23F43-7686-11cf-8F52-0040333594A3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template MeshVertexColors {
 <1630B821-7842-11cf-8F52-0040333594A3>
 DWORD nVertexColors;
 array IndexedColor vertexColors[nVertexColors];
}

template Mesh {
 <3D82AB44-62DA-11cf-AB39-0020AF71E433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

Header{
1;
0;
1;
}

Mesh {
 10;
 -0.00024;0.01641;-0.00027;,
 -2.39655;0.06341;-0.04296;,
 -2.39704;0.03691;0.09245;,
 -0.00023;-0.01412;0.15462;,
 -2.39704;0.03691;0.09245;,
 -2.39700;0.03700;-0.04326;,
 -0.00024;-0.01509;-0.00056;,
 -0.00023;-0.01412;0.15462;,
 -0.00024;-0.01509;-0.00056;,
 -2.39700;0.03700;-0.04326;;
 
 5;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;0,8,9,1;,
 3;8,0,3;,
 3;9,2,1;;
 
 MeshMaterialList {
  1;
  5;
  0,
  0,
  0,
  0,
  0;;
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   5.000000;
   0.420000;0.420000;0.420000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "slopesoarer_text.png";
   }
  }
 }
 MeshNormals {
  5;
  0.015847;0.981128;0.192710;,
  -0.021580;-0.999763;0.002780;,
  0.018017;0.010037;-0.999787;,
  1.000000;0.000000;-0.000007;,
  -0.999854;0.017082;-0.000297;;
  5;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  3;3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  10;
  -0.352210;0.141670;
  -0.028380;0.135900;
  -0.028320;0.154200;
  -0.352210;0.162600;
  -0.970080;0.319270;
  -0.970080;0.300930;
  -0.646190;0.306700;
  -0.646190;0.327670;
  -0.352210;0.141630;
  -0.028320;0.135860;;
 }
}
