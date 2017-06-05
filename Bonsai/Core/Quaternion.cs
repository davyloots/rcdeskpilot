using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Bonsai.Core
{
    public struct BonsaiQuaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        private readonly static float EPSILON = 0.00001f;
        public readonly static BonsaiQuaternion Zero = new BonsaiQuaternion(0.0f, 0.0f, 0.0f, 0.0f);
        public readonly static BonsaiQuaternion Identity = new BonsaiQuaternion(0.0f, 0.0f, 0.0f, 1.0f);

        public BonsaiQuaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// BonsaiQuaternion multiplication
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BonsaiQuaternion operator *(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            // to maintain DirectX compatibility the factor is inversed (res = right*left)...
            // valid
            BonsaiQuaternion res = Zero;

            res.W = right.W * left.W - right.X * left.X - right.Y * left.Y - right.Z * left.Z;
            res.X = right.W * left.X + right.X * left.W + right.Y * left.Z - right.Z * left.Y;
            res.Y = right.W * left.Y + right.Y * left.W + right.Z * left.X - right.X * left.Z;
            res.Z = right.W * left.Z + right.Z * left.W + right.X * left.Y - right.Y * left.X;


            return res;
        }


        public static Vector3 operator *(BonsaiQuaternion left, Vector3 vector)
        {
            Vector3 uv, uuv;
            Vector3 qvec = new Vector3(left.X, left.Y, left.Z);

            uv = Vector3.Cross(qvec, vector);
            uuv = Vector3.Cross(qvec, uv);
            uv *= (2.0f * left.W);
            uuv *= 2.0f;

            return vector + uv + uuv;
        }

        public static BonsaiQuaternion operator +(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            return new BonsaiQuaternion(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }
        public static BonsaiQuaternion operator -(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            return new BonsaiQuaternion(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        public static BonsaiQuaternion operator -(BonsaiQuaternion right)
        {
            return new BonsaiQuaternion(-right.X, -right.Y, -right.Z, -right.W);
        }

        public static bool operator ==(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            return (left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W);
        }

        public static bool operator !=(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return (int)X ^ (int)Y ^ (int)Z ^ (int)W;
        }
        public override bool Equals(object obj)
        {
            BonsaiQuaternion quat = (BonsaiQuaternion)obj;

            return quat == this;
        }

        public float LengthSq()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public static float Dot(BonsaiQuaternion left, BonsaiQuaternion right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        /// <summary>
        /// Normalize BonsaiQuaternion
        /// </summary>
        public void Normalize()
        {
            float magnitude = this.Length();
            if (magnitude != 0.0f && magnitude != 1.0f)
            {
                magnitude = 1.0f / magnitude;
                X *= magnitude;
                Y *= magnitude;
                Z *= magnitude;
                W *= magnitude;
            }
        }

        /// <summary>
        /// BonsaiQuaternion linear interpolation 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static BonsaiQuaternion Slerp(BonsaiQuaternion source, BonsaiQuaternion dest, float time)
        {
            // valid
            BonsaiQuaternion res;
            float[] to1 = { 0, 0, 0, 0 };
            float omega, cosom, sinom, scale0, scale1;


            // calc cosine
            cosom = source.X * dest.X + source.Y * dest.Y + source.Z * dest.Z
                       + source.W * dest.W;


            // adjust signs (if necessary)
            if (cosom < 0.0)
            {
                cosom = -cosom; to1[0] = -dest.X;
                to1[1] = -dest.Y;
                to1[2] = -dest.Z;
                to1[3] = -dest.W;
            }
            else
            {
                to1[0] = dest.X;
                to1[1] = dest.Y;
                to1[2] = dest.Z;
                to1[3] = dest.W;
            }


            // calculate coefficients


            if ((1.0 - cosom) > EPSILON)
            {
                // standard case (slerp)
                omega = (float)Math.Acos(cosom);
                sinom = (float)Math.Sin(omega);
                scale0 = (float)Math.Sin((1.0f - time) * omega) / sinom;
                scale1 = (float)Math.Sin(time * omega) / sinom;


            }
            else
            {
                // "from" and "to" BonsaiQuaternions are very close 
                //  ... so we can do a linear interpolation
                scale0 = 1.0f - time;
                scale1 = time;
            }
            // calculate final values
            res.X = scale0 * source.X + scale1 * to1[0];
            res.Y = scale0 * source.Y + scale1 * to1[1];
            res.Z = scale0 * source.Z + scale1 * to1[2];
            res.W = scale0 * source.W + scale1 * to1[3];

            return res;
        }

        /// <summary>
        /// return a BonsaiQuaternion from axis and angle 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static BonsaiQuaternion RotationAxis(Vector3 axis, float angle)
        {
            
            // valid
            BonsaiQuaternion res = BonsaiQuaternion.Zero;
            Vector3 normAxis = Vector3.Normalize(axis);


            float sin = (float)Math.Sin(angle / 2.0f);

            res.X = sin * normAxis.X;
            res.Y = sin * normAxis.Y;
            res.Z = sin * normAxis.Z;
            res.W = (float)Math.Cos(angle / 2.0f);

            return res;
        }

        /// <summary>
        /// returns a BonsaiQuaternion from matrix 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static BonsaiQuaternion RotationMatrix(Matrix matrix)
        {
            
            // valid
            BonsaiQuaternion res = Zero;
            float [,]m = get44(matrix);
            float tr, s;
            float[] q = { 0, 0, 0, 0 };
            int i, j, k;


            int []nxt = {1, 2, 0};


            tr = m[0,0] + m[1,1] + m[2,2];


            // check the diagonal
            if (tr > 0.0)
            {
              s = (float)Math.Sqrt(tr + 1.0);
              res.W = s / 2.0f;
              s = 0.5f / s;
              res.X = (m[1, 2] - m[2, 1]) * s;
              res.Y = (m[2, 0] - m[0, 2]) * s;
              res.Z = (m[0, 1] - m[1, 0]) * s;
            }
            else
            {
              // diagonal is negative
              i = 0;
              if (m[1, 1] > m[0, 0]) i = 1;
              if (m[2, 2] > m[i, i]) i = 2;
              j = nxt[i];
              k = nxt[j];


              s = (float)Math.Sqrt((m[i, i] - (m[j, j] + m[k, k])) + 1.0);

              q[i] = s * 0.5f;

              if (s != 0.0) s = 0.5f / s;


              q[3] = (m[j, k] - m[k, j]) * s;
              q[j] = (m[i, j] + m[j, i]) * s;
              q[k] = (m[i, k] + m[k, i]) * s;


              res.X = q[0];
              res.Y = q[1];
              res.Z = q[2];
              res.W = q[3];
            }
            return res;
        }

        public static BonsaiQuaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            BonsaiQuaternion res = Zero;


            float fSinPitch = (float)Math.Sin(pitch * 0.5F);
            float fCosPitch = (float)Math.Cos(pitch * 0.5F);
            float fSinYaw = (float)Math.Sin(yaw * 0.5F);
            float fCosYaw = (float)Math.Cos(yaw * 0.5F);
            float fSinRoll = (float)Math.Sin(roll * 0.5F);
            float fCosRoll = (float)Math.Cos(roll * 0.5F);
            float fCosPitchCosYaw = fCosPitch * fCosYaw;
            float fSinPitchSinYaw = fSinPitch * fSinYaw;
            res.X = fSinRoll * fCosPitchCosYaw - fCosRoll * fSinPitchSinYaw;
            res.Y = fCosRoll * fSinPitch * fCosYaw + fSinRoll * fCosPitch * fSinYaw;
            res.Z = fCosRoll * fCosPitch * fSinYaw - fSinRoll * fSinPitch * fCosYaw;
            res.W = fCosRoll * fCosPitchCosYaw + fSinRoll * fSinPitchSinYaw;
            return res;
        }

        private static float[,] get44(Matrix mat)
        {
            float[,] res = new float[4, 4];
            res[0, 0] = mat.M11;
            res[0, 1] = mat.M12;
            res[0, 2] = mat.M13;
            res[0, 3] = mat.M14;

            res[1, 0] = mat.M21;
            res[1, 1] = mat.M22;
            res[1, 2] = mat.M23;
            res[1, 3] = mat.M24;

            res[2, 0] = mat.M31;
            res[2, 1] = mat.M32;
            res[2, 2] = mat.M33;
            res[2, 3] = mat.M34;

            res[3, 0] = mat.M41;
            res[3, 1] = mat.M42;
            res[3, 2] = mat.M43;
            res[3, 3] = mat.M44;

            return res;
        }
    }
}
