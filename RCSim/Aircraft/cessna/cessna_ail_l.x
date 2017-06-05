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
 24;
 0.00232;0.01814;0.23941;,
 2.36309;0.07389;0.10117;,
 2.36309;0.12180;-0.12386;,
 0.00232;0.06834;0.01255;,
 0.00232;0.06834;0.01255;,
 2.36309;0.12180;-0.12386;,
 2.36309;0.06113;-0.13153;,
 0.00232;0.00045;0.00129;,
 2.36309;0.06172;0.10024;,
 0.00232;0.00281;0.23957;,
 2.36309;0.07389;0.10117;,
 0.00232;0.01814;0.23941;,
 0.56026;0.01572;-0.01130;,
 0.56937;0.01572;-0.01130;,
 0.56937;-0.00460;-0.01130;,
 0.56026;-0.00460;-0.01130;,
 0.56306;-0.00460;-0.02986;,
 0.55395;-0.00460;-0.02986;,
 0.56691;-0.10733;-0.00328;,
 0.56937;0.01496;0.04711;,
 0.56026;0.01496;0.04711;,
 0.55780;-0.10733;-0.00328;,
 0.55395;-0.10677;-0.02986;,
 0.56306;-0.10677;-0.02986;;
 
 19;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;7,6,8,9;,
 4;9,8,10,11;,
 4;10,8,6,5;,
 4;9,11,4,7;,
 4;12,13,14,15;,
 4;15,14,16,17;,
 4;18,19,20,21;,
 4;22,17,16,23;,
 4;22,23,18,21;,
 3;19,14,13;,
 3;19,18,23;,
 3;23,14,19;,
 3;23,16,14;,
 3;20,12,15;,
 3;15,21,20;,
 3;15,22,21;,
 3;22,15,17;;
 
 MeshMaterialList {
  1;
  19;
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
   0.397000;0.397000;0.397000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "cessna.png";
   }
  }
 }
 MeshNormals {
  22;
  -0.010263;0.977187;0.212132;,
  -0.059841;0.144325;-0.987719;,
  0.025685;-0.999651;0.006214;,
  0.000000;-0.380967;0.924589;,
  0.000000;-0.000049;-1.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.059424;-0.032918;0.997690;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;-0.999779;-0.021022;,
  0.999148;-0.008946;-0.040299;,
  0.994620;-0.022139;-0.101202;,
  1.000000;-0.000000;-0.000017;,
  0.989052;0.038800;-0.142376;,
  0.988801;-0.009014;-0.148970;,
  0.946904;0.000000;-0.321518;,
  -0.999925;0.011642;-0.003899;,
  -0.993199;0.014674;0.115502;,
  -0.997241;0.029162;0.068257;,
  -0.972139;0.017507;0.233750;,
  -0.946888;-0.000009;0.321563;;
  19;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  4;7,7,7,7;,
  4;8,8,8,8;,
  4;9,9,9,9;,
  4;4,4,4,4;,
  4;5,5,5,5;,
  4;3,3,3,3;,
  4;6,6,6,6;,
  4;10,10,10,10;,
  3;11,12,13;,
  3;11,14,15;,
  3;15,12,11;,
  3;15,16,12;,
  3;17,9,18;,
  3;18,19,17;,
  3;18,20,19;,
  3;20,18,21;;
 }
 MeshTextureCoords {
  24;
  -0.663780;0.203600;
  -0.932050;0.219310;
  -0.932050;0.244880;
  -0.663780;0.229380;
  -0.333760;0.528310;
  -0.064270;0.512730;
  -0.064270;0.511860;
  -0.333760;0.527020;
  -0.064270;0.538310;
  -0.333760;0.554220;
  -0.064270;0.538420;
  -0.333760;0.554200;
  -0.679390;0.688390;
  -0.679390;0.688390;
  -0.679390;0.705320;
  -0.679390;0.705320;
  -0.694860;0.705320;
  -0.694860;0.705320;
  -0.672710;0.790930;
  -0.630720;0.689020;
  -0.630720;0.689020;
  -0.672710;0.790930;
  -0.694860;0.790460;
  -0.694860;0.790460;;
 }
}
