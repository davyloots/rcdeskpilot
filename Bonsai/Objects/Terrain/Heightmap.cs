using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Bonsai;
using Bonsai.Core;
using Microsoft.DirectX;

namespace Bonsai.Objects.Terrain
{
    public class Heightmap
    {
        #region Private fields
        private string filename = null;
        private float[,] heightArray;
        private int xSubdivisions = 1;
        private int ySubdivisions = 1;
        private float minHeight = 0.0f;
        private float maxHeight = 10.0f;
        private float size = 1000f;
        private float xSegmentSize;
        private float ySegmentSize;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the minimum height that represents a brightness value of 0.0f.
        /// </summary>
        public float MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }

        /// <summary>
        /// Gets/Sets the maximum height that represents a brightness value of 1.0f.
        /// </summary>
        public float MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        /// <summary>
        /// Gets/Sets the size in meters.
        /// </summary>
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Gets the number of subdivisions in X direction.
        /// </summary>
        public int XSubdivisions
        {
            get { return xSubdivisions; }
        }

        /// <summary>
        /// Gets the number of subdivisions in Y direction.
        /// </summary>
        public int YSubdivisions
        {
            get { return ySubdivisions; }
        }
        #endregion

        #region Constructor
        public Heightmap(string filename, float size, int xSubdivisions, int ySubdivisions)
        {
            this.filename = filename;
            this.size = size;
            this.xSubdivisions = xSubdivisions;
            this.ySubdivisions = ySubdivisions;
            xSegmentSize = size / xSubdivisions;
            ySegmentSize = size / ySubdivisions;
            LoadValues();
        }

        public Heightmap(float size)
        {
            this.Size = size;
            this.xSubdivisions = 1;
            this.ySubdivisions = 1;
            xSegmentSize = size / xSubdivisions;
            ySegmentSize = size / ySubdivisions;
            heightArray = new float[xSubdivisions + 1, ySubdivisions + 1];
        }
        #endregion

        #region Public methods
        public float GetHeightAt(float x, float z)
        {
            float x1 = -z + size / 2;
            float y1 = x + size / 2;
            int ix = (int)Math.Floor(x1 / xSegmentSize);
            int iy = (int)Math.Floor(y1 / ySegmentSize);
            if ((ix < 0) || (iy < 0) || (ix >= xSubdivisions) || (iy >= ySubdivisions))
                return 0;
            float rx = x1/xSegmentSize - ix;
            float ry = y1/ySegmentSize - iy;
                       

            if (rx + ry > 1) // bottom triangle
            {
                float topPoint = rx * GetHeightAt(ix + 1, iy) + (1 - rx) * GetHeightAt(ix, iy + 1);
                float bottomPoint = rx * GetHeightAt(ix + 1, iy + 1) + (1 - rx) * GetHeightAt(ix, iy + 1);
                if (rx == 0)
                    return topPoint;
                else
                    return (topPoint * (1 - ry) + bottomPoint * (ry - 1 + rx)) / rx;
            }
            else // top triangle
            {
                float topPoint = rx * GetHeightAt(ix + 1, iy) + (1 - rx) * GetHeightAt(ix, iy);
                float bottomPoint = rx * GetHeightAt(ix + 1, iy) + (1-rx) * GetHeightAt(ix, iy + 1);
                if (rx == 1)
                    return topPoint;
                else
                    return (topPoint * (1 - rx - ry) + bottomPoint * ry)/(1 - rx);
            }
        }

        public float GetHeightAt(int x, int z)
        {
            if ((x >= 0) && (z >= 0) && (x <= xSubdivisions) && (z <= ySubdivisions))
            {
                float relHeight = heightArray[x, z];
                return relHeight * MaxHeight + (1 - relHeight) * MinHeight;
            }
            else
                return 0.0f;
        }

        /// <summary>
        /// Gets the normal for heightmap vertex (x,z).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 GetNormalAt(int x, int z)
        {
            if ((x >= 0) && (z >= 0) && (x < xSubdivisions) && (z < ySubdivisions))
            {
                Vector3 p1 = new Vector3(0, GetHeightAt(x,z), 0);
                Vector3 p2 = new Vector3(-xSegmentSize, GetHeightAt(x,z+1), 0);
                Vector3 p3 = new Vector3(0, GetHeightAt(x+1, z), +ySegmentSize);
                Vector3 v1 = p3 - p1;
                Vector3 v2 = p2 - p1;
                return Vector3.Normalize(Vector3.Cross(v2, v1));
            }
            else
                return new Vector3(0, 1f, 0);
        }

        /// <summary>
        /// Gets the normal for the direct3d location (x, .., z).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 GetNormalAt(float x, float z)
        {            
            float x1 = -z + size / 2;
            float y1 = x + size / 2;
            int ix = (int)Math.Floor(x1 / xSegmentSize);
            int iy = (int)Math.Floor(y1 / ySegmentSize);
            //return GetNormalAt(ix, iy);
            if ((ix < 0) || (iy < 0) || (ix >= xSubdivisions) || (iy >= ySubdivisions))
                return new Vector3(0, 1f, 0);
            float rx = x1/xSegmentSize - ix;
            float ry = y1/ySegmentSize - iy;
            
            if (rx + ry > 1) // bottom triangle
            {
                Vector3 p1 = new Vector3(xSegmentSize, GetHeightAt(ix + 1, iy + 1), -ySegmentSize);
                Vector3 p2 = new Vector3(xSegmentSize, GetHeightAt(ix, iy + 1), 0);
                Vector3 p3 = new Vector3(0, GetHeightAt(ix + 1, iy), -ySegmentSize);
                Vector3 v1 = p3 - p1;
                Vector3 v2 = p2 - p1;
                return Vector3.Normalize(Vector3.Cross(v1, v2));
            }
            else
            {
                Vector3 p1 = new Vector3(0f, GetHeightAt(ix, iy), 0f);
                Vector3 p2 = new Vector3(xSegmentSize, GetHeightAt(ix, iy + 1), 0f);
                Vector3 p3 = new Vector3(0f, GetHeightAt(ix + 1, iy), -ySegmentSize);
                Vector3 v1 = p3 - p1;
                Vector3 v2 = p2 - p1;
                return Vector3.Normalize(Vector3.Cross(v2, v1));
            }            
        }

        /// <summary>
        /// Gets the normal for the direct3d location (x, .., z).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 GetSmoothNormalAt(float x, float z)
        {
            float rad = 2.5f;
            Vector3 n = GetNormalAt(x, z);
            Vector3 n1 = GetNormalAt(x - rad, z - rad);
            Vector3 n2 = GetNormalAt(x - rad, z + rad);
            Vector3 n3 = GetNormalAt(x + rad, z - rad);
            Vector3 n4 = GetNormalAt(x + rad, z + rad);
            return 0.2f * (n1 + n2 + n3 + n4 + n);

            /*
            float x1 = -z + size / 2;
            float y1 = x + size / 2;
            int ix = (int)Math.Floor(x1 / xSegmentSize);
            int iy = (int)Math.Floor(y1 / ySegmentSize);
            //return GetNormalAt(ix, iy);
            if ((ix < 0) || (iy < 0) || (ix >= xSubdivisions) || (iy >= ySubdivisions))
                return new Vector3(0, 1f, 0);
            float rx = x1/xSegmentSize - ix;
            float ry = y1/ySegmentSize - iy;
            float rad = 1.5f;
            if (rx + ry > 1) // bottom triangle
            {
                Vector3 p1 = new Vector3(rad, GetHeightAt(x + rad, z + rad), -rad);
                Vector3 p2 = new Vector3(rad, GetHeightAt(x, z + rad), 0);
                Vector3 p3 = new Vector3(0, GetHeightAt(x + rad, z), -rad);
                Vector3 v1 = p3 - p1;
                Vector3 v2 = p2 - p1;
                return Vector3.Normalize(Vector3.Cross(v1, v2));
            }
            else
            {
                Vector3 p1 = new Vector3(0f, GetHeightAt(x, z), 0f);
                Vector3 p2 = new Vector3(rad, GetHeightAt(x, z + rad), 0);
                Vector3 p3 = new Vector3(0, GetHeightAt(x + rad, z), -rad);
                Vector3 v1 = p3 - p1;
                Vector3 v2 = p2 - p1;
                return Vector3.Normalize(Vector3.Cross(v2, v1));
            } 
             */
        }

        public void GetPoints(float x, float z, out Vector3 p1, out Vector3 p2, out Vector3 p3)
        {
            float x1 = -z + size / 2;
            float y1 = x + size / 2;
            int ix = (int)Math.Floor(x1 / xSegmentSize);
            int iy = (int)Math.Floor(y1 / ySegmentSize);
            if ((ix < 0) || (iy < 0) || (ix >= xSubdivisions) || (iy >= ySubdivisions))
            {
                p1 = new Vector3(1, 0, 0);
                p2 = new Vector3(0, 0, 0);
                p3 = new Vector3(0, 0, 1);
                return;
            }
            float rx = x1 / xSegmentSize - ix;
            float ry = y1 / ySegmentSize - iy;

            if (rx + ry > 1) // bottom triangle
            {
                p1 = new Vector3((iy + 1) * xSegmentSize - size / 2, GetHeightAt(ix+1, iy+1), -((ix + 1) * ySegmentSize - Size / 2));
                p3 = new Vector3(iy * xSegmentSize - size / 2, GetHeightAt(ix+1, iy), -((ix + 1) * ySegmentSize - Size / 2));
                p2 = new Vector3((iy + 1) * xSegmentSize - size / 2, GetHeightAt(ix, iy+1), -(ix * ySegmentSize - Size / 2));
            }
            else
            {
                p1 = new Vector3(iy * xSegmentSize - size / 2, GetHeightAt(ix, iy), -(ix * ySegmentSize - Size / 2));
                p2 = new Vector3(iy * xSegmentSize - size / 2, GetHeightAt(ix+1, iy), -((ix + 1) * ySegmentSize - Size / 2));
                p3 = new Vector3((iy + 1) * xSegmentSize - size / 2, GetHeightAt(ix, iy+1), -(ix * ySegmentSize - Size / 2));
            }
        }
        #endregion

        #region Private methods
        private void LoadValues()
        {
            heightArray = new float[xSubdivisions + 1, ySubdivisions + 1];
            filename = Utility.FindMediaFile(filename);
            using (Bitmap bitmap = new Bitmap(this.filename))
            {
                for (int x = 0; x < xSubdivisions + 1; x++)
                {
                    for (int y = 0; y < ySubdivisions + 1; y++)
                    {
                        int bx = bitmap.Width * x / (xSubdivisions + 1);
                        int by = bitmap.Height * y / (ySubdivisions + 1);
                        float height = bitmap.GetPixel(by, bx).GetBrightness();
                        heightArray[x, y] = height;
                    }
                }
            }
        }
        #endregion
    }
}
