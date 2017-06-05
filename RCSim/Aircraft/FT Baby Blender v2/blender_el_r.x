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
 16;
 0.08045;-0.02650;0.06963;,
 0.17176;-0.03393;0.28256;,
 0.17176;-0.00989;0.28339;,
 0.08045;-0.00246;0.07047;,
 0.83975;-0.03393;0.28257;,
 0.83975;-0.00989;0.28339;,
 0.84135;-0.02432;0.00738;,
 0.84135;0.00000;0.00003;,
 -0.08907;-0.02650;0.06963;,
 -0.08907;-0.00246;0.07047;,
 -0.18037;-0.00989;0.28339;,
 -0.18037;-0.03393;0.28256;,
 -0.84837;-0.00989;0.28339;,
 -0.84837;-0.03393;0.28257;,
 -0.84997;0.00000;0.00003;,
 -0.84997;-0.02432;0.00738;;
 
 18;
 4;0,1,2,3;,
 4;1,4,5,2;,
 4;4,6,7,5;,
 4;8,9,10,11;,
 4;11,10,12,13;,
 4;13,12,14,15;,
 4;0,3,9,8;,
 4;15,14,7,6;,
 3;5,7,2;,
 3;7,3,2;,
 3;10,14,12;,
 3;14,10,9;,
 4;9,3,7,14;,
 4;15,6,0,8;,
 3;15,8,11;,
 3;11,13,15;,
 3;1,0,6;,
 3;6,4,1;;
 
 MeshMaterialList {
  1;
  18;
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
    "blender2.dds";
   }
  }
 }
 MeshNormals {
  24;
  -0.919141;-0.013641;0.393692;,
  -0.000012;-0.034467;0.999406;,
  0.999983;0.000763;0.005764;,
  0.919142;-0.013659;0.393689;,
  0.000013;-0.034486;0.999405;,
  -0.999983;0.000763;0.005764;,
  -0.000003;-0.034863;0.999392;,
  0.000001;-0.289328;-0.957230;,
  -0.000000;0.999391;0.034899;,
  -0.000000;0.999391;0.034900;,
  -0.000000;0.999391;0.034899;,
  -0.000000;0.999391;0.034901;,
  0.000000;0.999391;0.034899;,
  0.000000;0.999391;0.034900;,
  0.000000;0.999391;0.034899;,
  0.000000;0.999391;0.034901;,
  -0.000000;-0.999391;-0.034899;,
  0.000000;-0.999391;-0.034899;,
  0.000000;-0.999391;-0.034900;,
  -0.000000;-0.999391;-0.034900;,
  -0.000000;-0.999391;-0.034898;,
  -0.000000;-0.999391;-0.034898;,
  0.000000;-0.999391;-0.034898;,
  0.000000;-0.999391;-0.034898;;
  18;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  4;3,3,3,3;,
  4;4,4,4,4;,
  4;5,5,5,5;,
  4;6,6,6,6;,
  4;7,7,7,7;,
  3;8,9,10;,
  3;9,11,10;,
  3;12,13,14;,
  3;13,12,15;,
  4;15,11,9,13;,
  4;16,17,18,19;,
  3;16,19,20;,
  3;20,21,16;,
  3;22,18,17;,
  3;17,23,22;;
 }
 MeshTextureCoords {
  16;
  -0.099970;0.373860;
  -0.091010;0.352980;
  -0.091010;0.352900;
  -0.099970;0.373780;
  -0.025520;0.352980;
  -0.025520;0.352900;
  -0.025360;0.379960;
  -0.025360;0.380680;
  -0.116590;0.373860;
  -0.116590;0.373780;
  -0.125540;0.352900;
  -0.125540;0.352980;
  -0.191030;0.352900;
  -0.191030;0.352980;
  -0.191190;0.380680;
  -0.191190;0.379960;;
 }
}
