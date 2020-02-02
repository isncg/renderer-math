using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class Ray
    {
        Vector3 origin;
        Vector3 direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public override string ToString()
        {
            return "( "+origin.ToString() + " )--< " + direction.ToString() + " >---->";
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
            
            if(mat.Det == 0)//ray begin from triangle plane: no intersection
            {
                return result;
            }
            Matrix3 matinv = mat.Inv;
            Vector3 directive = matinv * direction;
            
            //if(directive.MagnitudeSqr == 0)  // impossible case
            
            if(directive.X<0 || directive.Y<0|| directive.Z < 0)
            {
                return result;
            }


            directive *= (1 / (directive.X + directive.Y + directive.Z));
            result.HitPoint = mat * directive + this.origin;
            result.distance = (result.HitPoint - this.origin).Magnitude;
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
            HitPoint = Vector3.Zero;
        }

    }
}
