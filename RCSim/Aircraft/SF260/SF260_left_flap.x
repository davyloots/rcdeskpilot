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
 11;
 0.00011;0.00630;0.09486;,
 0.00011;0.02936;0.00154;,
 0.00011;-0.00030;0.00030;,
 0.51812;0.05209;-0.01440;,
 0.51812;0.07068;-0.08595;,
 0.51812;0.05209;-0.01440;,
 0.00011;0.00630;0.09486;,
 0.00011;-0.00030;0.00030;,
 0.51812;0.04476;-0.08671;,
 0.51812;0.04476;-0.08671;,
 0.51812;0.05209;-0.01440;;
 
 5;
 3;0,1,2;,
 4;3,4,1,0;,
 4;5,6,7,8;,
 4;9,2,1,4;,
 3;4,10,9;;
 
 MeshMaterialList {
  5;
  5;
  4,
  4,
  4,
  4,
  4;;
  Material {
   0.596000;0.608000;0.686000;0.741000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.149000;0.152000;0.171500;;
  }
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   25.000000;
   0.580000;0.580000;0.580000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.149000;0.149000;0.149000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.922000;0.922000;0.922000;0.361000;;
   5.000000;
   0.185000;0.185000;0.185000;;
   0.034114;0.034114;0.034114;;
  }
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "SF260.jpg";
   }
  }
 }
 MeshNormals {
  5;
  -1.000000;0.000000;0.000000;,
  0.102632;-0.991101;0.084768;,
  -0.034964;0.968768;0.245492;,
  -0.168854;0.035106;-0.985016;,
  1.000000;0.000000;-0.000000;;
  5;
  3;0,0,0;,
  4;2,2,2,2;,
  4;1,1,1,1;,
  4;3,3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  11;
  -0.283430;0.407950;
  -0.283430;0.369070;
  -0.283430;0.368550;
  -0.499260;0.362430;
  -0.499260;0.332620;
  0.234560;0.295860;
  0.262720;0.429370;
  0.238350;0.429370;
  0.215920;0.295860;
  -0.499260;0.332300;
  -0.499260;0.362430;;
 }
}
