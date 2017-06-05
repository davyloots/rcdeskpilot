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
 -0.79461;0.59869;-0.00060;,
 -0.80288;0.58773;0.12562;,
 -0.02633;0.00200;0.12562;,
 -0.00005;-0.00006;-0.00060;,
 -0.80288;0.58773;0.12562;,
 -0.81114;0.57677;0.01387;,
 -0.01657;-0.02198;0.01387;,
 -0.02633;0.00200;0.12562;,
 -0.79461;0.59869;-0.00060;,
 -0.00005;-0.00006;-0.00060;;
 
 5;
 4;0,1,2,3;,
 4;4,5,6,7;,
 3;8,5,4;,
 4;5,8,9,6;,
 3;7,6,9;;
 
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
  0.598356;0.793663;0.109860;,
  -0.597655;-0.792732;0.119931;,
  -0.798623;0.601832;-0.000006;,
  -0.280611;-0.372383;-0.884640;,
  0.839421;-0.511703;0.183118;;
  5;
  4;0,0,0,0;,
  4;1,1,1,1;,
  3;2,2,2;,
  4;3,3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  10;
  -0.013670;0.466740;
  -0.011100;0.505940;
  -0.252260;0.505940;
  -0.260430;0.466740;
  -0.516820;0.675450;
  -0.519390;0.640740;
  -0.272630;0.640740;
  -0.275660;0.675450;
  -0.514260;0.636250;
  -0.267500;0.636250;;
 }
}
