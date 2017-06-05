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
 0.00021;0.01642;-0.00023;,
 0.00021;-0.01411;0.15465;,
 2.39702;0.03692;0.09249;,
 2.39652;0.06342;-0.04292;,
 2.39702;0.03692;0.09249;,
 0.00021;-0.01411;0.15465;,
 0.00021;-0.01508;-0.00052;,
 2.39697;0.03701;-0.04322;,
 2.39697;0.03701;-0.04322;,
 0.00021;-0.01508;-0.00052;;
 
 5;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;0,3,8,9;,
 3;9,1,0;,
 3;8,3,2;;
 
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
  -0.015852;0.981127;0.192712;,
  0.021580;-0.999763;0.002780;,
  -0.018017;0.010036;-0.999787;,
  -1.000000;0.000000;-0.000007;,
  0.999854;0.017081;-0.000294;;
  5;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  3;3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  10;
  -0.646510;0.141670;
  -0.646510;0.162600;
  -0.970400;0.154200;
  -0.970330;0.135900;
  -0.028000;0.319270;
  -0.351890;0.327670;
  -0.351890;0.306700;
  -0.028000;0.300930;
  -0.970390;0.135860;
  -0.646510;0.141630;;
 }
}
