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
 25;
 0.00000;0.42293;0.15184;,
 0.00000;0.40930;0.28014;,
 0.00565;0.36311;0.11308;,
 0.00524;0.36255;0.16441;,
 0.00565;0.36311;0.11308;,
 0.00000;-0.00578;0.14067;,
 0.00524;0.36255;0.16441;,
 0.00000;-0.00578;0.14067;,
 0.01074;-0.00033;0.00012;,
 0.00524;0.36255;0.16441;,
 -0.00523;0.36255;0.16441;,
 0.00000;0.40930;0.28014;,
 -0.00564;0.36311;0.11308;,
 0.00000;0.42293;0.15184;,
 -0.00564;0.36311;0.11308;,
 -0.00523;0.36255;0.16441;,
 0.00000;-0.00578;0.14067;,
 -0.00523;0.36255;0.16441;,
 -0.01074;-0.00033;0.00012;,
 0.00000;-0.00578;0.14067;,
 0.00000;0.42293;0.15184;,
 -0.00564;0.36311;0.11308;,
 -0.00523;0.36255;0.16441;,
 -0.01074;-0.00033;0.00012;,
 0.00000;-0.00578;0.14067;;
 
 12;
 3;0,1,2;,
 3;1,3,4;,
 3;5,6,1;,
 3;7,8,9;,
 3;10,11,12;,
 3;11,13,14;,
 3;15,16,11;,
 3;17,18,19;,
 3;20,2,21;,
 4;21,2,3,22;,
 4;22,3,8,23;,
 3;23,8,24;;
 
 MeshMaterialList {
  5;
  12;
  4,
  4,
  4,
  4,
  4,
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
  14;
  -0.000096;0.543793;-0.839219;,
  0.998299;0.053336;0.023553;,
  0.000000;-0.999247;-0.038789;,
  0.996080;0.087959;0.009345;,
  0.995958;0.089375;0.008949;,
  0.998479;-0.017560;0.052262;,
  0.996970;-0.019031;0.075422;,
  -0.995956;0.089395;0.008932;,
  -0.996083;0.087930;0.009342;,
  -0.998480;-0.017557;0.052252;,
  -0.996970;-0.019029;0.075429;,
  -0.998299;0.053334;0.023543;,
  -0.000048;-0.999940;-0.010920;,
  0.000000;0.412450;-0.910980;;
  12;
  3;3,1,3;,
  3;1,4,4;,
  3;5,5,1;,
  3;6,6,6;,
  3;7,11,7;,
  3;11,8,8;,
  3;9,9,11;,
  3;10,10,10;,
  3;0,0,0;,
  4;12,12,12,12;,
  4;13,13,13,13;,
  3;2,2,2;;
 }
 MeshTextureCoords {
  25;
  -0.072920;0.695040;
  -0.015650;0.701120;
  -0.090230;0.721740;
  -0.067310;0.721990;
  -0.090230;0.721740;
  -0.077910;0.886430;
  -0.067310;0.721990;
  -0.077910;0.886430;
  -0.140660;0.883990;
  -0.067310;0.721990;
  -0.067200;0.276070;
  -0.015540;0.296940;
  -0.090120;0.276330;
  -0.072820;0.303030;
  -0.090120;0.276330;
  -0.067200;0.276070;
  -0.077800;0.111640;
  -0.067200;0.276070;
  -0.140550;0.114080;
  -0.077800;0.111640;
  -0.072920;0.695040;
  -0.090230;0.721740;
  -0.067310;0.721990;
  -0.140660;0.883990;
  -0.077910;0.886430;;
 }
}
