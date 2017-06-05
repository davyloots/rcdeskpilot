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
 -0.01787;-0.09635;-0.08441;,
 0.00000;0.00008;-0.00001;,
 0.01787;-0.09635;-0.08441;,
 0.00000;0.00008;-0.00001;,
 0.03446;-0.00162;0.00251;,
 0.03641;-0.09740;-0.08318;,
 0.00000;0.00008;-0.00001;,
 -0.03641;-0.09740;-0.08318;,
 -0.03446;-0.00162;0.00251;,
 0.00000;0.00008;-0.00001;,
 0.00000;0.00008;-0.00001;;
 
 10;
 3;0,1,2;,
 3;3,4,5;,
 3;6,7,8;,
 3;5,2,9;,
 3;10,0,7;,
 3;2,1,0;,
 3;5,4,3;,
 3;8,7,6;,
 3;9,2,5;,
 3;7,0,10;;
 
 MeshMaterialList {
  5;
  10;
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
  18;
  0.000000;0.658601;-0.752493;,
  -0.043556;0.662534;-0.747764;,
  0.087035;0.665187;-0.741587;,
  -0.087018;0.665184;-0.741592;,
  0.043556;0.662534;-0.747764;,
  0.087044;0.665188;-0.741585;,
  -0.087008;0.665183;-0.741594;,
  0.087027;0.665185;-0.741589;,
  -0.087027;0.665185;-0.741589;,
  -0.043556;-0.662534;0.747764;,
  0.000000;-0.658601;0.752493;,
  0.043556;-0.662534;0.747764;,
  -0.087035;-0.665187;0.741587;,
  -0.087044;-0.665188;0.741585;,
  0.087008;-0.665183;0.741594;,
  0.087018;-0.665184;0.741592;,
  -0.087027;-0.665185;0.741589;,
  0.087027;-0.665185;0.741589;;
  10;
  3;1,0,4;,
  3;5,5,2;,
  3;6,3,6;,
  3;2,4,7;,
  3;8,1,3;,
  3;9,10,11;,
  3;12,13,13;,
  3;14,15,14;,
  3;16,9,12;,
  3;15,11,17;;
 }
 MeshTextureCoords {
  11;
  0.091470;0.503860;
  0.058700;0.499250;
  0.091470;0.494650;
  0.058700;0.499250;
  0.058700;0.490370;
  0.091520;0.489870;
  0.058700;0.499250;
  0.091520;0.508640;
  0.058700;0.508140;
  0.058700;0.499250;
  0.058700;0.499250;;
 }
}
