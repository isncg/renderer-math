using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class Matrix3
    {
        public static Matrix3 Zero
        {
            get
            {
                Matrix3 mat = new Matrix3();
                mat.m11 = mat.m12 = mat.m13 = 0;
                mat.m21 = mat.m22 = mat.m23 = 0;
                mat.m31 = mat.m32 = mat.m33 = 0;
                return mat;
            }
        }

        public static Matrix3 I
        {
            get
            {
                Matrix3 mat = new Matrix3();
                mat.m12 = mat.m13 = 0;
                mat.m21 = mat.m23 = 0;
                mat.m31 = mat.m32 = 0;
                mat.m11 = mat.m22 = mat.m33 = 1;
                return mat;
            }
        }

        public double m11, m12, m13;
        public double m21, m22, m23;
        public double m31, m32, m33;

        public Matrix3()
        { }

        public Matrix3(double[][] values)
        {
            m11 = values[0][0]; m12 = values[0][1]; m13 = values[0][2];
            m21 = values[1][0]; m22 = values[1][1]; m23 = values[1][2];
            m31 = values[2][0]; m32 = values[2][1]; m33 = values[2][2];
        }

        public static Matrix3 FromColumnVectors(Vector3 col1, Vector3 col2, Vector3 col3)
        {
            var ret = new Matrix3();
            ret.m11 = col1.X; ret.m21 = col1.Y; ret.m31 = col1.Z;
            ret.m12 = col2.X; ret.m22 = col2.Y; ret.m32 = col2.Z;
            ret.m13 = col3.X; ret.m23 = col3.Y; ret.m33 = col3.Z;
            return ret;
        }

        public static Matrix3 RandomRotate()
        {
            Vector3 randX = Vector3.Random();
            randX.Normalize();
            Vector3 randY = Vector3.Random();
            Vector3 randZ = randX ^ randY;
            randZ.Normalize();
            randY = randZ ^ randX;
            return FromColumnVectors(randX, randY, randZ);
        }

        public override string ToString()
        {
            return string.Format("\n{0,10} {1,10} {2,10}\n{3,10} {4,10} {5,10}\n{6,10} {7,10} {8,10}",
                string.Format("{0:#.0000}", m11),
                string.Format("{0:#.0000}", m12),
                string.Format("{0:#.0000}", m13),

                string.Format("{0:#.0000}", m21),
                string.Format("{0:#.0000}", m22),
                string.Format("{0:#.0000}", m23),

                string.Format("{0:#.0000}", m31),
                string.Format("{0:#.0000}", m32),
                string.Format("{0:#.0000}", m33)
                ); //base.ToString();
        }

        public Matrix3(Matrix3 copyfrom)
        {
            this.m11 = copyfrom.m11; this.m12 = copyfrom.m12; this.m13 = copyfrom.m13;
            this.m21 = copyfrom.m21; this.m22 = copyfrom.m22; this.m23 = copyfrom.m23;
            this.m31 = copyfrom.m31; this.m32 = copyfrom.m32; this.m33 = copyfrom.m33;
        }

        public double Det
        {
            get
            {
                return
                    m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32 -
                    m13 * m22 * m31 - m23 * m32 * m11 - m33 * m12 * m21;
            }
        }


        public static Vector3 Mul(Matrix3 mat, Vector3 vec)
        {
            var ret = new Vector3();
            ret.X = mat.m11 * vec.X + mat.m12 * vec.Y + mat.m13 * vec.Z;
            ret.Y = mat.m21 * vec.X + mat.m22 * vec.Y + mat.m23 * vec.Z;
            ret.Z = mat.m31 * vec.X + mat.m32 * vec.Y + mat.m33 * vec.Z;
            return ret;
        }

        public static Vector3 operator * (Matrix3 mat, Vector3 vec)
        {
            return Mul(mat, vec);
        }


        public Matrix3 Inv
        {
            // m11, m12, m13;
            // m21, m22, m23;
            // m31, m32, m33;
            get
            {
                Matrix3 ret = new Matrix3();
                double det = Det;

                double r11 = m22 * m33 - m23 * m32;
                double r12 = m21 * m33 - m23 * m31;
                double r13 = m21 * m32 - m22 * m31;

                double r21 = m12 * m33 - m13 * m32;
                double r22 = m11 * m33 - m13 * m31;
                double r23 = m11 * m32 - m12 * m31;

                double r31 = m12 * m23 - m13 * m22;
                double r32 = m11 * m23 - m13 * m21;
                double r33 = m11 * m22 - m12 * m21;

                ret.m11 = r11 / det;
                ret.m12 = -r21 / det;
                ret.m13 = r31 / det;

                ret.m21 = -r12 / det;
                ret.m22 = r22 / det;
                ret.m23 = -r32 / det;

                ret.m31 = r13 / det;
                ret.m32 = -r23 / det;
                ret.m33 = r33 / det;

                return ret;
            }
        }

    }
}
