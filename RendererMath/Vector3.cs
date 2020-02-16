using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RendererMath
{
    public class Vector3
    {
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual double Z { get; set; }

        public virtual bool IsZero
        {
            get
            {
                return MagnitudeSqr > 0;
            }
        }

        public double[] ToArray()
        {
            return new double[3] { X, Y, Z };
        }

        public override string ToString()
        {
            return string.Format("{0,10} {1,10} {2,10}",
                string.Format("{0:#.0000}", X),
                string.Format("{0:#.0000}", Y),
                string.Format("{0:#.0000}", Z)
                );
        }

        public double MagnitudeSqr
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z);
            }
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(this.MagnitudeSqr);
            }
        }

        public void Normalize()
        {
            var l = Magnitude;
            X /= l;
            Y /= l;
            Z /= l;
        }

        public void Assign(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Assign(Vector3 copyfrom)
        {
            X = copyfrom.X;
            Y = copyfrom.Y;
            Z = copyfrom.Z;
        }

        public void Assign(double[] xyz)
        {
            X = xyz[0];
            Y = xyz[1];
            Z = xyz[2];
        }

        public Vector3()
        { }

        public Vector3(double x, double y, double z)
        {
            Assign(x, y, z);
        }

        public Vector3(Vector3 copyfrom)
        {
            Assign(copyfrom);
        }

        public Vector3(double[] xyz)
        {
            Assign(xyz);
        }

        public static Vector3 Zero { get { return new Vector3(0, 0, 0); } }
        public static Vector3 One { get { return new Vector3(1, 1, 1); } }

        public static Vector3 Sub(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return Sub(left, right);
        }

        public static Vector3 Plus(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return Plus(left, right);
        }

        public static Vector3 Minus(Vector3 vec)
        {
            return new Vector3(-vec.X, -vec.Y, -vec.Z);
        }

        public static Vector3 operator -(Vector3 vec)
        {
            return Minus(vec);
        }

        public static Vector3 Mul(Vector3 vec, double val)
        {
            return new Vector3(vec.X * val, vec.Y * val, vec.Z * val);
        }

        public static Vector3 operator *(Vector3 vec, double val)
        {
            return Mul(vec, val);
        }

        public static double Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static double operator *(Vector3 left, Vector3 right)
        {
            return Dot(left, right);
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            var x = left.Y * right.Z - left.Z * right.Y;
            var y = left.Z * right.X - left.X * right.Z;
            var z = left.X * right.Y - left.Y * right.X;
            return new Vector3(x, y, z);
        }

        public static Vector3 operator ^(Vector3 left, Vector3 right)
        {
            return Cross(left, right);
        }

        public Color ToColor()
        {
            int R = (int)(X * 255);
            int G = (int)(Y * 255);
            int B = (int)(Z * 255);
            if (R > 255) R = 255; if (R < 0) R = 0;
            if (G > 255) G = 255; if (G < 0) G = 0;
            if (B > 255) B = 255; if (B < 0) B = 0;

            return Color.FromArgb(R,G,B);
        }

    }

    public class ZeroVector3 : Vector3
    {
        public override bool IsZero => true;
    }
}
