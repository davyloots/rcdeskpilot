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
 14;
 -0.38956;1.05495;-0.20766;,
 -0.38956;3.15334;-0.20766;,
 0.38963;3.15334;-0.20766;,
 0.38963;1.05495;-0.20766;,
 -0.89822;1.24590;-0.20766;,
 0.00004;-0.00071;-0.20767;,
 0.89831;1.24590;-0.20766;,
 0.38988;3.15368;0.22068;,
 0.38988;1.05529;0.22068;,
 -0.38931;3.15368;0.22068;,
 -0.38931;1.05529;0.22068;,
 0.00028;-0.00038;0.22068;,
 -0.89799;1.24627;0.22068;,
 0.89853;1.24627;0.22068;;
 
 15;
 4;0,1,2,3;,
 3;4,0,5;,
 3;5,3,6;,
 3;5,0,3;,
 4;3,2,7,8;,
 4;2,1,9,7;,
 4;1,0,10,9;,
 4;4,5,11,12;,
 4;0,4,12,10;,
 4;5,6,13,11;,
 4;6,3,8,13;,
 3;8,11,13;,
 3;12,11,10;,
 3;11,8,10;,
 4;7,9,10,8;;
 
 MeshMaterialList {
  1;
  15;
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
   1.000000;0.000000;0.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.250000;0.000000;0.000000;;
  }
 }
 MeshNormals {
  19;
  -0.000000;0.000000;-1.000000;,
  -0.000000;0.000000;-1.000000;,
  -0.000001;0.000001;-1.000000;,
  -0.000000;0.000000;-1.000000;,
  -0.000000;0.000000;-1.000000;,
  0.000000;-0.000000;-1.000000;,
  -0.811321;-0.584600;0.000929;,
  0.811321;-0.584600;0.000032;,
  -1.000000;0.000000;0.000577;,
  1.000000;-0.000000;-0.000578;,
  0.000000;1.000000;-0.000793;,
  0.351462;0.936202;-0.000967;,
  -0.351463;0.936202;-0.000579;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;;
  15;
  4;3,5,5,4;,
  3;0,3,1;,
  3;1,4,2;,
  3;1,3,4;,
  4;9,9,9,9;,
  4;10,10,10,10;,
  4;8,8,8,8;,
  4;6,6,6,6;,
  4;11,11,11,11;,
  4;7,7,7,7;,
  4;12,12,12,12;,
  3;13,14,15;,
  3;16,14,17;,
  3;14,13,17;,
  4;18,18,17,13;;
 }
 MeshTextureCoords {
  14;
  0.000000;0.000000;
  0.000000;1.000000;
  1.000000;1.000000;
  1.000000;0.000000;
  0.000000;0.000000;
  1.000000;0.000000;
  1.000000;1.000000;
  1.000000;1.000000;
  1.000000;0.000000;
  0.000000;1.000000;
  0.000000;0.000000;
  1.000000;0.000000;
  0.000000;0.000000;
  1.000000;1.000000;;
 }
}
