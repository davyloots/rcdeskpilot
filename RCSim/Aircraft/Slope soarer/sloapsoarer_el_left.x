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
 0.79459;0.59868;-0.00064;,
 0.00002;-0.00007;-0.00064;,
 0.02630;0.00198;0.12558;,
 0.80285;0.58772;0.12558;,
 0.80285;0.58772;0.12558;,
 0.02630;0.00198;0.12558;,
 0.01654;-0.02200;0.01383;,
 0.81111;0.57675;0.01383;,
 0.79459;0.59868;-0.00064;,
 0.00002;-0.00007;-0.00064;;
 
 5;
 4;0,1,2,3;,
 4;4,5,6,7;,
 3;8,4,7;,
 4;7,6,9,8;,
 3;5,9,6;;
 
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
  -0.598356;0.793663;0.109860;,
  0.597655;-0.792733;0.119930;,
  0.798623;0.601832;-0.000006;,
  0.280610;-0.372382;-0.884641;,
  -0.839422;-0.511703;0.183117;;
  5;
  4;0,0,0,0;,
  4;1,1,1,1;,
  3;2,2,2;,
  4;3,3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  10;
  -0.514430;0.466740;
  -0.267670;0.466740;
  -0.275840;0.505940;
  -0.517000;0.505940;
  -0.010920;0.675450;
  -0.252090;0.675450;
  -0.255120;0.640740;
  -0.008360;0.640740;
  -0.013490;0.636250;
  -0.260250;0.636250;;
 }
}
