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
 -0.00000;-0.02290;0.01040;,
 1.72742;-0.00082;-0.25176;,
 1.72742;0.02222;-0.10771;,
 -0.00000;-0.00328;0.18255;,
 -0.00000;0.01601;0.01040;,
 -0.00000;-0.00328;0.18255;,
 1.72742;0.02222;-0.10771;,
 1.72742;0.03801;-0.25176;,
 -0.00000;0.00001;-0.00000;,
 1.72742;0.02299;-0.26423;,
 -0.00000;0.00001;-0.00000;,
 1.72742;0.02299;-0.26423;,
 1.72742;-0.00082;-0.25176;,
 -0.00000;-0.02290;0.01040;;
 
 8;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;8,4,7,9;,
 4;10,11,1,0;,
 3;9,7,12;,
 3;7,6,12;,
 3;13,4,8;,
 3;4,13,5;;
 
 MeshMaterialList {
  8;
  8;
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1;;
  Material {
   0.000000;0.000000;0.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   5.000000;
   1.000000;1.000000;1.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "extra300s3.png";
   }
  }
  Material {
   0.400000;0.447000;1.000000;0.444000;;
   5.000000;
   1.000000;1.000000;1.000000;;
   0.100000;0.111750;0.250000;;
  }
  Material {
   1.000000;1.000000;1.000000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.250000;0.250000;0.250000;;
  }
  Material {
   0.318000;0.318000;0.318000;0.365000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.079500;0.079500;0.079500;;
  }
  Material {
   0.318000;0.318000;0.318000;0.500000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.079500;0.079500;0.079500;;
  }
  Material {
   0.318000;0.318000;0.318000;0.437000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.079500;0.079500;0.079500;;
  }
  Material {
   0.441000;0.441000;0.441000;1.000000;;
   5.000000;
   0.478000;0.478000;0.478000;;
   0.000000;0.000000;0.000000;;
  }
 }
 MeshNormals {
  7;
  0.035131;-0.990147;0.135551;,
  0.003958;0.993906;0.110162;,
  -0.129314;0.587893;-0.798536;,
  -0.130059;-0.435065;-0.890956;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;;
  8;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  4;3,3,3,3;,
  3;4,4,4;,
  3;4,4,4;,
  3;5,5,5;,
  3;5,5,6;;
 }
 MeshTextureCoords {
  14;
  -0.888860;0.494410;
  -0.501550;0.435630;
  -0.501550;0.467930;
  -0.888860;0.533010;
  -0.391460;0.530270;
  -0.391460;0.491670;
  -0.004150;0.556750;
  -0.004150;0.589050;
  -0.391460;0.532600;
  -0.004150;0.591840;
  -0.888860;0.492080;
  -0.501550;0.432830;
  -0.004150;0.589050;
  -0.391460;0.530270;;
 }
}
