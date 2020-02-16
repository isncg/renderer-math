using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class Ray
    {
        public Vector3 origin;
        public Vector3 direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public override string ToString()
        {
            return "( " + origin.ToString() + " )--< " + direction.ToString() + " >---->";
        }


        public RayTriangleIntersection IntersectionTest(Triangle triangle)
        {
            var result = new RayTriangleIntersection();
            result.triangle = triangle;
            result.ray = this;

            var vx = triangle.RightCorner - this.origin;
            var vy = triangle.LeftCorner - this.origin;
            var vz = triangle.RootCorner - this.origin;

            Matrix3 mat = Matrix3.FromColumnVectors(vx, vy, vz);

            if (mat.Det == 0)//ray begin from triangle plane: no intersection
            {
                return result;
            }
            Matrix3 matinv = mat.Inv;
            Vector3 directive = matinv * direction;

            //if(directive.MagnitudeSqr == 0)  // impossible case

            if (directive.X < 0 || directive.Y < 0 || directive.Z < 0)
            {
                return result;
            }


            directive *= (1 / (directive.X + directive.Y + directive.Z));
            result.HitPoint = mat * directive + this.origin;
            result.distance = (result.HitPoint - this.origin).Magnitude;
            result.IsIntersect = true;
            return result;
        }

        public RayAABBSerfaceIntersection IntersectionTest(AABBSurface surface)
        {
            RayAABBSerfaceIntersection result = new RayAABBSerfaceIntersection();

            double distance = origin.ToArray()[(int)surface.axis] - surface.location;
            double speed = direction.ToArray()[(int)surface.axis];
            if (distance == 0 || speed == 0)
            {
                return result;
            }

            double zoom = distance / speed;
            if (zoom <= 0)
            {
                return result;
            }

            result.ray = this;
            result.surface = surface;
            result.IsIntersect = true;
            result.distance = direction.Magnitude * zoom;
            result.HitPoint = direction * zoom + origin;

            return result;
        }


        public RayAABBIntersection IntersectionTest(AABB aabb)
        {
            RayAABBIntersection result = new RayAABBIntersection();
            double tEnter = Double.MinValue;
            double tLeave = Double.MaxValue;
            double ood;
            double tAxisEnter, tAxisLeave, temp;

            // <editor-fold desc="平行于x轴">
            if (Math.Abs(direction.X) < 0.000001)
            {
                if (origin.X < aabb.xmin || origin.X > aabb.xmax)
                {
                    return result;
                }
            }
            else
            {
                ood = 1.0 / direction.X;
                tAxisEnter = (aabb.xmin - origin.X) * ood;
                tAxisLeave = (aabb.xmax - origin.X) * ood;
                //# t1做候选平面，t2做远平面
                if (tAxisEnter > tAxisLeave)
                {
                    temp = tAxisEnter;
                    tAxisEnter = tAxisLeave;
                    tAxisLeave = temp;
                }
                if (tAxisEnter > tEnter)
                    tEnter = tAxisEnter;
                if (tAxisLeave < tLeave)
                    tLeave = tAxisLeave;
                if (tEnter > tLeave)
                    return result;
            }
            //# </editor-fold>

            //# <editor-fold desc="平行于y轴">
            if (Math.Abs(direction.Y) < 0.000001)
            {
                if (origin.Y < aabb.ymin || origin.Y > aabb.ymax)
                {
                    return result;
                }
            }
            else
            {
                ood = 1.0 / direction.Y;
                tAxisEnter = (aabb.ymin - origin.Y) * ood;
                tAxisLeave = (aabb.ymax - origin.Y) * ood;
                //# t1做候选平面，t2做远平面
                if (tAxisEnter > tAxisLeave)
                {
                    temp = tAxisEnter;
                    tAxisEnter = tAxisLeave;
                    tAxisLeave = temp;
                }
                if (tAxisEnter > tEnter)
                    tEnter = tAxisEnter;
                if (tAxisLeave < tLeave)
                    tLeave = tAxisLeave;
                if (tEnter > tLeave)
                    return result;

            }
            //# </editor-fold>

            //# <editor-fold desc="平行于z轴">
            if (Math.Abs(direction.Z) < 0.000001)
            {
                if (origin.Z < aabb.zmin || origin.Z > aabb.zmax)
                    return result;
            }
            else
            {
                ood = 1.0 / direction.Z;
                tAxisEnter = (aabb.zmin - origin.Z) * ood;
                tAxisLeave = (aabb.zmax - origin.Z) * ood;
                //# t1做候选平面，t2做远平面
                if (tAxisEnter > tAxisLeave)
                {
                    temp = tAxisEnter;
                    tAxisEnter = tAxisLeave;
                    tAxisLeave = temp;
                }
                if (tAxisEnter > tEnter)
                    tEnter = tAxisEnter;
                if (tAxisLeave < tLeave)
                    tLeave = tAxisLeave;
                if (tEnter > tLeave)
                    return result;
            }
            //# </editor-fold>
            result.IsIntersect = true;
            return result;
        }
    }


    class RayTriangleIntersection
    {
        public Ray ray;
        public Triangle triangle;
        public bool IsIntersect;
        public double distance;
        public Vector3 HitPoint;

        public override string ToString()
        {
            if (IsIntersect)
            {
                return HitPoint.ToString();
            }
            else
            {
                return "N/A";
            }
        }
        public RayTriangleIntersection()
        {
            ray = null;
            triangle = null;
            IsIntersect = false;
            distance = -1;
            HitPoint = null;
        }

    }

    class RayAABBSerfaceIntersection
    {
        public Ray ray;
        public AABBSurface surface;

        public bool IsIntersect;
        public double distance;
        public Vector3 HitPoint;


        public RayAABBSerfaceIntersection()
        {
            ray = null;
            surface = null;
            IsIntersect = false;
            distance = 0;
            HitPoint = null;
        }
    }

    class RayAABBIntersection
    {
        public Ray ray;
        public AABB aabb;
        public bool IsIntersect;

        public RayAABBIntersection()
        {
            IsIntersect = false;
        }

    }

}
