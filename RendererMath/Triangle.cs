using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class Triangle
    {
        public Vector3[] Corners = new Vector3[3];
        public Vector3 RootCorner { get { return Corners[0]; } set { Corners[0] = value; } }
        public Vector3 LeftCorner { get { return Corners[1]; } set { Corners[1] = value; } }
        public Vector3 RightCorner { get { return Corners[2]; } set { Corners[2] = value; } }

        public Triangle()
        {

        }

        public Triangle(Vector3 root, Vector3 left, Vector3 right)
        {
            RootCorner = root;
            LeftCorner = left;
            RightCorner = right;
        }

        public override string ToString()
        {
            return RootCorner.ToString() + "\n" + LeftCorner.ToString() + "\n" + RightCorner.ToString();
        }

        public Vector3 PlaneNormal
        {
            get
            {
                var ret = (LeftCorner - RootCorner) ^ (RightCorner - RootCorner);
                ret.Normalize();
                return ret;
            }
        }
    }
}
