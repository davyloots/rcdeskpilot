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
 16;
 0.00446;0.27092;0.03624;,
 0.00446;0.01286;0.05893;,
 0.00446;0.00079;0.00430;,
 0.00446;0.27669;0.00430;,
 -0.00446;0.01286;0.05893;,
 -0.00446;0.27092;0.03624;,
 -0.00446;0.27669;0.00430;,
 -0.00446;0.00079;0.00430;,
 0.00008;0.27017;0.04049;,
 0.00008;0.27745;0.00005;,
 0.00008;0.27017;0.04049;,
 0.00008;0.27745;0.00005;,
 0.00008;-0.00006;0.00005;,
 0.00008;-0.00006;0.00005;,
 0.00008;0.01367;0.06310;,
 0.00008;0.01367;0.06310;;
 
 10;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;8,0,3,9;,
 4;10,11,6,5;,
 4;9,3,2,12;,
 4;11,13,7,6;,
 4;12,2,1,14;,
 4;13,15,4,7;,
 4;14,1,0,8;,
 4;15,10,5,4;;
 
 MeshMaterialList {
  1;
  10;
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0;;
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "trainer.png";
   }
  }
 }
 MeshNormals {
  14;
  -0.915976;0.035184;0.399687;,
  0.919655;0.034438;0.391215;,
  -0.917223;0.000000;-0.398373;,
  0.920882;0.000000;-0.389841;,
  0.000005;0.984133;0.177432;,
  0.696047;0.000000;-0.717996;,
  0.000071;-0.976790;0.214201;,
  0.000262;0.984133;0.177432;,
  -0.000252;0.984133;0.177432;,
  -0.682597;0.000000;-0.730795;,
  0.003908;-0.976782;0.214200;,
  -0.003765;-0.976783;0.214200;,
  0.691530;0.063342;0.719565;,
  -0.678025;0.064455;0.732207;;
  10;
  4;1,1,3,3;,
  4;0,0,2,2;,
  4;4,7,7,4;,
  4;4,4,8,8;,
  4;5,3,3,5;,
  4;9,9,2,2;,
  4;6,10,10,6;,
  4;6,6,11,11;,
  4;12,1,1,12;,
  4;13,13,0,0;;
 }
 MeshTextureCoords {
  16;
  -0.034890;0.738720;
  -0.017960;0.931310;
  -0.058730;0.940310;
  -0.058730;0.734420;
  -0.981490;0.651350;
  -0.964550;0.458770;
  -0.940720;0.454460;
  -0.940720;0.660360;
  -0.031720;0.739290;
  -0.061900;0.733860;
  -0.967720;0.459330;
  -0.937550;0.453900;
  -0.061900;0.940950;
  -0.937550;0.660990;
  -0.014850;0.930700;
  -0.984600;0.650750;;
 }
}
