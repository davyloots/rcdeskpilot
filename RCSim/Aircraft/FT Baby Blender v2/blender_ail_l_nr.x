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
 0.25905;0.00384;0.01789;,
 0.26738;0.00384;0.01789;,
 0.26738;-0.01212;-0.00446;,
 0.25905;-0.01212;-0.00446;,
 0.26738;0.00374;0.18443;,
 0.26738;0.00384;0.01789;,
 0.25905;0.00384;0.01789;,
 0.25905;0.00374;0.18443;,
 0.26738;0.00374;0.18443;,
 0.26738;-0.01212;-0.00446;,
 0.26738;0.00384;0.01789;,
 0.25905;-0.00437;0.18443;,
 0.25905;0.00374;0.18443;,
 0.25905;0.00384;0.01789;,
 0.25905;-0.01212;-0.00446;,
 0.25905;-0.00437;0.18443;,
 0.26738;-0.00437;0.18443;,
 0.26738;-0.06001;-0.00446;,
 0.25905;-0.06001;-0.00446;,
 0.25905;-0.01212;-0.00446;,
 0.26738;-0.01212;-0.00446;,
 0.25905;-0.05956;0.00823;,
 0.26738;-0.05956;0.00823;,
 0.26738;-0.05956;0.00823;,
 0.26738;-0.06001;-0.00446;,
 0.26738;-0.00437;0.18443;,
 0.25905;-0.06001;-0.00446;,
 0.25905;-0.05956;0.00823;,
 0.26738;-0.06001;-0.00446;,
 0.25905;-0.06001;-0.00446;;
 
 11;
 4;0,1,2,3;,
 4;4,5,6,7;,
 3;8,9,10;,
 4;11,12,13,14;,
 4;4,7,15,16;,
 4;17,18,19,20;,
 4;21,22,16,15;,
 4;23,24,9,8;,
 3;23,8,25;,
 4;26,27,11,14;,
 4;28,22,21,29;;
 
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
  10;
  0.000000;0.952393;-0.304873;,
  0.000000;0.457580;-0.889169;,
  0.000000;1.000000;0.000642;,
  0.000000;0.000000;1.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;-0.985713;0.168431;,
  1.000000;0.000000;0.000000;,
  0.000000;-0.954271;0.298942;,
  0.000000;-0.999391;0.034885;;
  11;
  4;0,0,1,1;,
  4;2,0,0,2;,
  3;7,7,7;,
  4;4,4,4,4;,
  4;3,3,3,3;,
  4;5,5,1,1;,
  4;6,6,8,8;,
  4;7,7,7,7;,
  3;7,7,7;,
  4;4,4,4,4;,
  4;9,6,6,9;;
 }
 MeshTextureCoords {
  30;
  0.000000;0.000000;
  1.000000;0.000000;
  1.000000;1.000000;
  0.000000;1.000000;
  1.000000;0.136920;
  1.000000;1.000000;
  0.000000;1.000000;
  0.000000;0.136920;
  -2.258880;0.609740;
  -2.286190;0.370640;
  -2.258690;0.409130;
  -2.272830;0.609740;
  -2.258880;0.609740;
  -2.258690;0.409130;
  -2.286190;0.370640;
  0.000000;0.882530;
  1.000000;0.882530;
  1.000000;0.903550;
  0.000000;0.903550;
  0.000000;0.000000;
  1.000000;0.000000;
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
