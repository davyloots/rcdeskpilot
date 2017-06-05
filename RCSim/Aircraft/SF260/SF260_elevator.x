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
 50;
 0.46175;0.00621;-0.03712;,
 0.45533;-0.00206;0.07512;,
 0.46175;-0.00523;-0.03764;,
 0.46175;-0.00523;-0.03764;,
 0.45533;-0.00206;0.07512;,
 0.43613;-0.00918;-0.00077;,
 0.43613;-0.00918;-0.00077;,
 0.01499;-0.00524;0.11250;,
 0.02702;-0.00928;-0.00013;,
 0.43613;-0.00918;-0.00077;,
 0.43613;0.00712;0.00026;,
 0.46175;0.00621;-0.03712;,
 0.43613;0.00712;0.00026;,
 0.01499;-0.00524;0.11250;,
 0.43613;0.00712;0.00026;,
 0.02702;0.00698;0.00015;,
 0.02702;0.00698;0.00015;,
 0.02702;-0.00928;-0.00013;,
 0.01499;-0.00524;0.11250;,
 0.43613;0.00712;0.00026;,
 0.43613;-0.00918;-0.00077;,
 0.02702;-0.00928;-0.00013;,
 0.02702;0.00698;0.00015;,
 0.46175;0.00621;-0.03712;,
 0.46175;-0.00523;-0.03764;,
 -0.46175;0.00621;-0.03712;,
 -0.46175;-0.00523;-0.03764;,
 -0.45533;-0.00206;0.07512;,
 -0.46175;-0.00523;-0.03764;,
 -0.43613;-0.00918;-0.00077;,
 -0.45533;-0.00206;0.07512;,
 -0.43613;-0.00918;-0.00077;,
 -0.02702;-0.00928;-0.00013;,
 -0.01499;-0.00524;0.11250;,
 -0.43613;-0.00918;-0.00077;,
 -0.43613;0.00712;0.00026;,
 -0.46175;0.00621;-0.03712;,
 -0.43613;0.00712;0.00026;,
 -0.01499;-0.00524;0.11250;,
 -0.43613;0.00712;0.00026;,
 -0.02702;0.00698;0.00015;,
 -0.02702;0.00698;0.00015;,
 -0.01499;-0.00524;0.11250;,
 -0.02702;-0.00928;-0.00013;,
 -0.43613;0.00712;0.00026;,
 -0.02702;0.00698;0.00015;,
 -0.02702;-0.00928;-0.00013;,
 -0.43613;-0.00918;-0.00077;,
 -0.46175;-0.00523;-0.03764;,
 -0.46175;0.00621;-0.03712;;
 
 20;
 3;0,1,2;,
 3;3,4,5;,
 3;6,7,8;,
 3;9,4,7;,
 3;10,1,11;,
 3;12,13,1;,
 3;14,15,13;,
 3;16,17,18;,
 4;19,20,21,22;,
 4;19,23,24,20;,
 3;25,26,27;,
 3;28,29,30;,
 3;31,32,33;,
 3;34,33,30;,
 3;35,36,27;,
 3;37,27,38;,
 3;39,38,40;,
 3;41,42,43;,
 4;44,45,46,47;,
 4;44,47,48,49;;
 
 MeshMaterialList {
  5;
  20;
  4,
  4,
  4,
  4,
  4,
  4,
  4,
  1,
  4,
  4,
  4,
  4,
  4,
  4,
  4,
  4,
  4,
  1,
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
  30;
  -0.823174;0.030829;-0.566951;,
  0.998378;-0.002594;0.056871;,
  0.007554;-0.997996;0.062826;,
  -0.994335;0.001850;-0.106279;,
  0.207278;-0.977492;0.039298;,
  0.000295;-0.999356;0.035887;,
  0.014806;-0.995857;0.089717;,
  0.153747;0.984758;0.081320;,
  0.003100;0.992657;0.120927;,
  -0.000376;0.994146;0.108048;,
  0.823174;0.030829;-0.566951;,
  -0.998378;-0.002594;0.056871;,
  -0.007554;-0.997996;0.062826;,
  0.994335;0.001850;-0.106279;,
  -0.207278;-0.977492;0.039298;,
  -0.000295;-0.999356;0.035887;,
  -0.014806;-0.995857;0.089717;,
  -0.153747;0.984758;0.081320;,
  -0.003100;0.992657;0.120927;,
  0.000376;0.994146;0.108048;,
  0.111600;-0.991636;0.064832;,
  0.078663;0.991728;0.101432;,
  0.001362;0.993424;0.114490;,
  -0.465173;0.040129;-0.884310;,
  -0.000662;0.040240;-0.999190;,
  -0.111600;-0.991636;0.064832;,
  -0.078663;0.991728;0.101432;,
  -0.001362;0.993424;0.114490;,
  0.465173;0.040129;-0.884310;,
  0.000662;0.040240;-0.999190;;
  20;
  3;1,1,1;,
  3;4,20,4;,
  3;5,2,5;,
  3;6,20,2;,
  3;7,21,7;,
  3;8,22,21;,
  3;9,9,22;,
  3;3,3,3;,
  4;23,23,24,24;,
  4;23,0,0,23;,
  3;11,11,11;,
  3;14,14,25;,
  3;15,15,12;,
  3;16,12,25;,
  3;17,17,26;,
  3;18,26,27;,
  3;19,27,19;,
  3;13,13,13;,
  4;28,29,29,28;,
  4;28,28,10,10;;
 }
 MeshTextureCoords {
  50;
  -0.062390;0.329790;
  -0.020820;0.332160;
  -0.062580;0.329790;
  -0.934450;0.445360;
  -0.970820;0.443290;
  -0.946340;0.437100;
  -0.946340;0.437100;
  -0.982880;0.301250;
  -0.946550;0.305130;
  -0.946340;0.437100;
  -0.048540;0.339270;
  -0.062390;0.329790;
  -0.048540;0.339270;
  -0.006970;0.495260;
  -0.048540;0.339270;
  -0.048580;0.490800;
  2.951970;0.489990;
  2.951860;0.489990;
  2.993580;0.494450;
  2.951670;0.338470;
  2.951290;0.338470;
  2.951520;0.489990;
  2.951630;0.489990;
  2.937820;0.328980;
  2.937630;0.328980;
  -0.062390;0.671820;
  -0.062580;0.671820;
  -0.020820;0.669450;
  -0.934450;0.147460;
  -0.946340;0.155720;
  -0.970820;0.149530;
  -0.946340;0.155720;
  -0.946550;0.287700;
  -0.982880;0.291580;
  -0.946340;0.155720;
  -0.048540;0.662340;
  -0.062390;0.671820;
  -0.048540;0.662340;
  -0.006970;0.506360;
  -0.048540;0.662340;
  -0.048580;0.510810;
  2.951970;0.510010;
  2.993580;0.505550;
  2.951860;0.510010;
  2.951670;0.661530;
  2.951630;0.510010;
  2.951520;0.510010;
  2.951290;0.661530;
  2.937630;0.671020;
  2.937820;0.671020;;
 }
}