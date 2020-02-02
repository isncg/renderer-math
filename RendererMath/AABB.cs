using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class AABB
    {
        public double xmin, xmax, ymin, ymax, zmin, zmax;

        public AABB(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax)
        {
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
            this.zmin = zmin;
            this.zmax = zmax;
        }

    }
    public class AABBSurface
    {
        public enum Axis { X,Y,Z}
        public Axis axis;
        public double location;

        public static AABBSurface X(double location)
        {
            return new AABBSurface() { axis = Axis.X, location = location };
        }
        public static AABBSurface Y(double location)
        {
            return new AABBSurface() { axis = Axis.Y, location = location };
        }
        public static AABBSurface Z(double location)
        {
            return new AABBSurface() { axis = Axis.Z, location = location };
        }
    }
}
