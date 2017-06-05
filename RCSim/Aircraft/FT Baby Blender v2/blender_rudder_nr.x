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
 -0.00193;0.05623;0.01714;,
 0.01403;0.05623;-0.00521;,
 0.01403;0.04790;-0.00521;,
 -0.00193;0.04790;0.01714;,
 -0.00182;0.04790;0.18368;,
 -0.00182;0.05623;0.18368;,
 -0.00193;0.05623;0.01714;,
 -0.00193;0.04790;0.01714;,
 -0.00182;0.04790;0.18368;,
 -0.00193;0.04790;0.01714;,
 0.01403;0.04790;-0.00521;,
 0.00628;0.05623;0.18368;,
 0.01403;0.05623;-0.00521;,
 -0.00193;0.05623;0.01714;,
 -0.00182;0.05623;0.18368;,
 0.00628;0.04790;0.18368;,
 0.00628;0.05623;0.18368;,
 0.06192;0.04790;-0.00521;,
 0.01403;0.04790;-0.00521;,
 0.01403;0.05623;-0.00521;,
 0.06192;0.05623;-0.00521;,
 0.06148;0.05623;0.00748;,
 0.06148;0.04790;0.00748;,
 0.06148;0.04790;0.00748;,
 0.06192;0.04790;-0.00521;,
 0.00628;0.04790;0.18368;,
 0.06192;0.05623;-0.00521;,
 0.06148;0.05623;0.00748;,
 0.06192;0.04790;-0.00521;,
 0.06192;0.05623;-0.00521;;
 
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
  11;
  -0.952393;0.000000;-0.304874;,
  -0.457579;0.000000;-0.889169;,
  -1.000000;0.000000;0.000642;,
  0.000000;0.000000;1.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.985714;0.000000;0.168430;,
  0.000000;-1.000000;-0.000000;,
  0.954272;0.000000;0.298939;,
  0.000000;-1.000000;-0.000000;,
  0.999391;0.000000;0.034885;;
  11;
  4;0,1,1,0;,
  4;2,2,0,0;,
  3;7,7,7;,
  4;4,4,4,4;,
  4;3,3,3,3;,
  4;5,1,1,5;,
  4;6,8,8,6;,
  4;7,7,7,7;,
  3;7,9,7;,
  4;4,4,4,4;,
  4;10,10,6,6;;
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
