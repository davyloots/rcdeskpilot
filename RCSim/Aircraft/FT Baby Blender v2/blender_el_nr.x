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
 30;
 -0.13921;-0.00181;0.00972;,
 -0.13618;0.01492;-0.01185;,
 -0.12793;0.01492;-0.01069;,
 -0.13096;-0.00181;0.01088;,
 -0.15412;-0.00752;0.17570;,
 -0.16237;-0.00752;0.17454;,
 -0.13921;-0.00181;0.00972;,
 -0.13096;-0.00181;0.01088;,
 -0.15412;-0.00752;0.17570;,
 -0.13096;-0.00181;0.01088;,
 -0.12793;0.01492;-0.01069;,
 -0.16241;0.00058;0.17482;,
 -0.13618;0.01492;-0.01185;,
 -0.13921;-0.00181;0.00972;,
 -0.16237;-0.00752;0.17454;,
 -0.15416;0.00058;0.17598;,
 -0.16241;0.00058;0.17482;,
 -0.12816;0.06278;-0.00903;,
 -0.12793;0.01492;-0.01069;,
 -0.13618;0.01492;-0.01185;,
 -0.13641;0.06278;-0.01019;,
 -0.13817;0.06189;0.00235;,
 -0.12992;0.06189;0.00351;,
 -0.12992;0.06189;0.00351;,
 -0.12816;0.06278;-0.00903;,
 -0.15416;0.00058;0.17598;,
 -0.13641;0.06278;-0.01019;,
 -0.13817;0.06189;0.00235;,
 -0.12816;0.06278;-0.00903;,
 -0.13641;0.06278;-0.01019;;
 
 11;
 4;0,1,2,3;,
 4;4,5,6,7;,
 3;8,9,10;,
 4;11,12,13,14;,
 4;4,15,16,5;,
 4;17,18,19,20;,
 4;21,16,15,22;,
 4;23,8,10,24;,
 3;23,25,8;,
 4;26,12,11,27;,
 4;28,29,21,22;;
 
 MeshMaterialList {
  1;
  11;
  0,
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
    "berk.jpg";
   }
  }
 }
 MeshNormals {
  17;
  0.046999;-0.941172;-0.334643;,
  0.125937;-0.426282;-0.895781;,
  0.004706;-0.999413;-0.033934;,
  -0.139032;-0.034841;0.989675;,
  -0.990268;-0.000012;-0.139172;,
  0.139163;0.034869;-0.989655;,
  -0.028145;0.979236;0.200758;,
  0.990268;0.000024;0.139170;,
  0.990269;-0.000030;0.139168;,
  0.990271;-0.000012;0.139149;,
  -0.990268;-0.000034;-0.139175;,
  -0.046138;0.943258;0.328840;,
  0.990268;0.000050;0.139171;,
  0.990274;0.000005;0.139130;,
  0.990263;0.000096;0.139212;,
  -0.990269;0.000010;-0.139170;,
  -0.009646;0.997566;0.069059;;
  11;
  4;0,1,1,0;,
  4;2,2,0,0;,
  3;7,8,9;,
  4;4,4,10,10;,
  4;3,3,3,3;,
  4;5,1,1,5;,
  4;6,11,11,6;,
  4;12,7,9,13;,
  3;12,14,7;,
  4;15,4,4,15;,
  4;16,16,6,6;;
 }
 MeshTextureCoords {
  30;
  0.000000;0.000000;
  0.000000;1.000000;
  1.000000;1.000000;
  1.000000;0.000000;
  1.000000;0.136920;
  0.000000;0.136920;
  0.000000;1.000000;
  1.000000;1.000000;
  -2.258880;0.609740;
  -2.258690;0.409130;
  -2.286190;0.370640;
  -2.272830;0.609740;
  -2.286190;0.370640;
  -2.258690;0.409130;
  -2.258880;0.609740;
  1.000000;0.882530;
  0.000000;0.882530;
  1.000000;0.903550;
  1.000000;0.000000;
  0.000000;0.000000;
  0.000000;0.903550;
  0.000000;0.080680;
  1.000000;0.080680;
  -2.367910;0.392500;
  -2.368670;0.370640;
  -2.272830;0.609740;
  -2.368670;0.370640;
  -2.367910;0.392500;
  1.000000;0.096450;
  0.000000;0.096450;;
 }
}
