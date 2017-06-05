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
 6;
 -0.08201;-0.08897;0.12737;,
 -0.00133;0.00098;0.08172;,
 -0.00044;0.00029;0.00006;,
 -0.08596;-0.09390;0.10411;,
 -0.09668;-0.10472;0.05454;,
 -0.12907;-0.14164;0.00476;;
 
 7;
 4;0,1,2,3;,
 4;3,2,1,0;,
 3;3,2,4;,
 3;4,2,5;,
 4;2,3,0,1;,
 3;2,3,4;,
 3;2,4,5;;
 
 MeshMaterialList {
  5;
  7;
  4,
  4,
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
  10;
  -0.741150;0.671159;-0.015547;,
  -0.742850;0.669410;-0.008047;,
  -0.747776;0.663811;-0.013665;,
  -0.737859;0.674951;-0.002427;,
  -0.743406;0.668817;-0.005546;,
  0.741191;-0.671293;0.001486;,
  0.741194;-0.671263;0.006173;,
  0.747776;-0.663811;0.013665;,
  0.737859;-0.674951;0.002427;,
  0.741150;-0.671159;0.015547;;
  7;
  4;2,2,1,4;,
  4;5,6,7,7;,
  3;4,1,3;,
  3;3,1,0;,
  4;1,4,2,2;,
  3;6,5,8;,
  3;6,8,9;;
 }
 MeshTextureCoords {
  6;
  0.207230;0.568880;
  0.195460;0.599950;
  0.174410;0.599950;
  0.201230;0.567250;
  0.188460;0.563350;
  0.175630;0.550710;;
 }
}
