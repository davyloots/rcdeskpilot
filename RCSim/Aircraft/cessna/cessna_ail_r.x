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
 -0.00006;0.01777;0.23816;,
 -0.00006;0.06797;0.01130;,
 -2.36083;0.12144;-0.12510;,
 -2.36083;0.07353;0.09992;,
 -0.00006;0.06797;0.01130;,
 -0.00006;0.00009;0.00004;,
 -2.36083;0.06077;-0.13277;,
 -2.36083;0.12144;-0.12510;,
 -0.00006;0.00244;0.23832;,
 -2.36083;0.06136;0.09899;,
 -0.00006;0.01777;0.23816;,
 -2.36083;0.07353;0.09992;,
 -0.55800;0.01535;-0.01255;,
 -0.55800;-0.00497;-0.01255;,
 -0.56711;-0.00497;-0.01255;,
 -0.56711;0.01535;-0.01255;,
 -0.55169;-0.00497;-0.03111;,
 -0.56080;-0.00497;-0.03111;,
 -0.56465;-0.10769;-0.00453;,
 -0.55554;-0.10769;-0.00453;,
 -0.55800;0.01459;0.04586;,
 -0.56711;0.01459;0.04586;,
 -0.55169;-0.10713;-0.03111;,
 -0.56080;-0.10713;-0.03111;;
 
 19;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;5,8,9,6;,
 4;8,10,11,9;,
 4;11,7,6,9;,
 4;8,5,4,10;,
 4;12,13,14,15;,
 4;13,16,17,14;,
 4;18,19,20,21;,
 4;22,23,17,16;,
 4;22,19,18,23;,
 3;21,15,14;,
 3;21,23,18;,
 3;23,21,14;,
 3;23,14,17;,
 3;20,13,12;,
 3;13,20,19;,
 3;13,19,22;,
 3;22,16,13;;
 
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
   0.405000;0.405000;0.405000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "cessna.png";
   }
  }
 }
 MeshNormals {
  22;
  0.010263;0.977187;0.212132;,
  0.059841;0.144325;-0.987719;,
  -0.025685;-0.999651;0.006214;,
  0.000000;-0.380967;0.924589;,
  0.000000;-0.000049;-1.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;0.000000;-1.000000;,
  -0.059424;-0.032917;0.997690;,
  -1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;-0.999779;-0.021021;,
  -0.999148;-0.008946;-0.040299;,
  -1.000000;-0.000000;-0.000017;,
  -0.994620;-0.022139;-0.101202;,
  -0.988801;-0.009014;-0.148970;,
  -0.989052;0.038800;-0.142376;,
  -0.946904;0.000000;-0.321518;,
  0.999925;0.011643;-0.003899;,
  0.993199;0.014674;0.115502;,
  0.997242;0.029162;0.068257;,
  0.972139;0.017507;0.233749;,
  0.946888;-0.000009;0.321563;;
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
  3;14,11,13;,
  3;14,13,16;,
  3;17,18,9;,
  3;18,17,19;,
  3;18,19,20;,
  3;20,21,18;;
 }
 MeshTextureCoords {
  24;
  -0.334120;0.203600;
  -0.334120;0.229380;
  -0.065850;0.244880;
  -0.065850;0.219310;
  -0.664920;0.528310;
  -0.664920;0.527020;
  -0.934420;0.511860;
  -0.934420;0.512730;
  -0.664920;0.554220;
  -0.934420;0.538310;
  -0.664920;0.554200;
  -0.934420;0.538420;
  -0.679390;0.688390;
  -0.679390;0.705320;
  -0.679390;0.705320;
  -0.679390;0.688390;
  -0.694860;0.705320;
  -0.694860;0.705320;
  -0.672700;0.790930;
  -0.672700;0.790930;
  -0.630720;0.689020;
  -0.630720;0.689020;
  -0.694860;0.790460;
  -0.694860;0.790460;;
 }
}
