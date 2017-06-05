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
 12;
 -0.67780;0.02624;-0.01719;,
 0.00004;-0.00794;0.04046;,
 -0.00017;-0.00006;-0.00002;,
 -0.67780;0.03450;-0.05855;,
 -0.67780;0.02259;-0.05392;,
 -0.00016;-0.01102;0.00431;,
 0.00004;-0.00794;0.04046;,
 -0.67780;0.02624;-0.01719;,
 -0.67780;0.03450;-0.05855;,
 -0.00017;-0.00006;-0.00002;,
 -0.67780;0.02259;-0.05392;,
 -0.00016;-0.01102;0.00431;;
 
 5;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;8,9,5,4;,
 3;0,3,10;,
 3;2,1,11;;
 
 MeshMaterialList {
  8;
  5;
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
    "trainer.png";
   }
  }
  Material {
   0.000000;0.000000;0.000000;1.000000;;
   11.000000;
   0.660000;0.660000;0.660000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.714000;0.714000;0.714000;1.000000;;
   13.000000;
   0.690000;0.690000;0.690000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.000000;0.000000;0.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.922000;0.922000;0.922000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.214200;0.214200;0.214200;0.300000;;
   5.000000;
   0.280000;0.280000;0.280000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.214200;0.214200;0.214200;0.250000;;
   5.000000;
   0.280000;0.280000;0.280000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.214200;0.214200;0.214200;0.200000;;
   5.000000;
   0.280000;0.280000;0.280000;;
   0.000000;0.000000;0.000000;;
  }
 }
 MeshNormals {
  5;
  0.033174;0.980612;0.193133;,
  -0.057580;-0.994098;0.091946;,
  0.061742;-0.364165;-0.929286;,
  -1.000000;0.000000;0.000000;,
  0.999985;-0.001112;-0.005355;;
  5;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  3;3,3,3;,
  3;4,4,4;;
 }
 MeshTextureCoords {
  12;
  -0.025780;0.197240;
  -0.454800;0.233720;
  -0.454670;0.208100;
  -0.025780;0.171060;
  -0.973730;2.412330;
  -0.544840;2.449180;
  -0.544720;2.472060;
  -0.973730;2.435580;
  -0.973730;2.409400;
  -0.544850;2.446440;
  -0.025780;0.173990;
  -0.454670;0.210840;;
 }
}
