using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RendererMath
{
    class Camera
    {
        Material defaultMat = new TypicalMaterial();
        Random random = new Random();

        public Vector3 position = null;
        public Vector3 direction = null;
        public Vector3 up = null;
        public int width = 0;
        public int height = 0;
        public double fov = 60;

        public Ray[,] pixelRays;

        public void Setup(Vector3 position, Vector3 direction, Vector3 up, int width, int height, double fov = 60)
        {
            this.position = position;
            this.direction = direction;
            this.up = up;
            this.width = width;
            this.height = height;
            this.pixelRays = CreatePixelRays();
            this.fov = fov;
        }

        public Vector3 Trace(Ray ray, List<Triangle> triangles, int depth)
        {
            RayTriangleIntersection firstIntersection = IntersectionTest(ray, triangles);
            Material mat = defaultMat;
            //foreach (var tri in triangles)
            //{
            //    var intersection = ray.IntersectionTest(tri);
            //    if (intersection.IsIntersect && intersection.distance > 0.000001)
            //    {
            //        if (firstIntersection == null || firstIntersection.distance > intersection.distance)
            //        {
            //            firstIntersection = intersection;
            //        }
            //    }
            //}

            if (firstIntersection != null)
            {
                if (firstIntersection.triangle.material != null)
                {
                    mat = firstIntersection.triangle.material;
                }
            }
            else
            {
                return new ZeroVector3();
            }

            if (depth <= 0)
            {
                return mat.Ambient(firstIntersection.triangle.PlaneNormal, -ray.direction);
            }

            Vector3 sum = Vector3.Zero;
            //var randomDirections = RandomDirections.Get();
            int cnt = (int)Math.Pow(16, depth);
            for (int ri = 0; ri < cnt; ri++)
            {
                var dir = new Vector3(random.NextDouble() - 0.5, random.NextDouble() - 0.5, random.NextDouble() - 0.5);
                dir.Normalize();
                var commingLight = Trace(new Ray(firstIntersection.HitPoint, dir), triangles, depth - 1);
                foreach (var f in mat.lightDistributions)
                {
                    var factor = f.GetFactor(firstIntersection.HitPoint, firstIntersection.triangle.PlaneNormal, -dir, -ray.direction);
                    var additionLight = new Vector3();
                    additionLight.X = commingLight.X * factor.X;
                    additionLight.Y = commingLight.Y * factor.Y;
                    additionLight.Z = commingLight.Z * factor.Z;
                    sum += additionLight;

                }
            }
            sum *= 1.0 / cnt;
            Vector3 result = sum + mat.Ambient(firstIntersection.triangle.PlaneNormal, -ray.direction);

            return result;
        }


        public RayTriangleIntersection IntersectionTest(Ray ray, List<Triangle> triangles)
        {
            RayTriangleIntersection firstIntersection = null;
            foreach (var tri in triangles)
            {
                var intersection = ray.IntersectionTest(tri);
                if (intersection.IsIntersect && intersection.distance > 0.000001)
                {
                    if (firstIntersection == null || firstIntersection.distance > intersection.distance)
                    {
                        firstIntersection = intersection;
                    }
                }
            }
            return firstIntersection;
        }

        public Ray[,] CreatePixelRays()
        {
            Ray[,] result = new Ray[height, width];

            direction.Normalize();
            //Vector3[,] result = new Vector3[height, width];
            Vector3 right = direction ^ up;
            right.Normalize();
            up = right ^ direction;
            up.Normalize();

            double aspect = (double)width / (double)height;

            double stepH = 2.0 / (width - 1);
            double stepV = (2.0 / aspect) / (height - 1);
            Console.WriteLine();

            double fovTanHalf = Math.Tan(fov * Math.PI / 180.0) / 2;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var vx = -fovTanHalf + stepH * j;
                    var vy = fovTanHalf / aspect - stepV * i;
                    var dir = direction + right * vx + up * vy;
                    result[i, j] = new Ray(position, dir);
                }
            }
            return result;
        }

        public Vector3[,] DepthScan(List<Triangle> triangles, double depthMin, double depthMax)
        {
            Vector3[,] result = new Vector3[height, width];
            //var rays = CreatePixelRays(position, direction, up, width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var intersection = IntersectionTest(pixelRays[i, j], triangles);
                    double depth = 1;
                    Vector3 norm = null;
                    if (intersection != null)
                    {
                        depth = (intersection.distance - depthMin) / (depthMax - depthMin);
                        norm = intersection.triangle.PlaneNormal;
                        norm.Normalize();
                    }
                    else
                    {
                        norm = new ZeroVector3();
                    }

                    //result[i, j] = new Vector3(1 - depth, 1 - depth, 1 - depth);
                    result[i, j] = norm * (1 - depth); //new Vector3(1 - depth, 1 - depth, 1 - depth);
                }
            }
            return result;
        }


        public Vector3[,] Capture(List<Triangle> triangles, int depth)
        {
            //direction.Normalize();
            Vector3[,] result = new Vector3[height, width];
            //Vector3 right = direction ^ up;
            //right.Normalize();
            //up = right ^ direction;
            //up.Normalize();

            //double aspect = (double)width / (double)height;

            //double stepH = 2.0 / (width - 1);
            //double stepV = (2.0 / aspect) / (height - 1);
            Console.WriteLine();

            //var rays = CreatePixelRays(position, direction, up, width, height);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //var vx = -1.0 + stepH * j;
                    //var vy = 1.0 / aspect - stepV * i;
                    //var dir = direction + right * vx + up * vy;
                    result[i, j] = Trace(pixelRays[i, j], triangles, depth);
                }
                Console.Write("\rprogress: {0:00.}%      ", 100 * (float)i / (float)height);
            }
            return result;
        }



        public static Bitmap GenImage(Vector3[,] frame)
        {
            int width = frame.GetLength(1);
            int height = frame.GetLength(0);
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bmp.SetPixel(j, i, frame[i, j].ToColor());
                }
            }
            return bmp;
        }

        public static Bitmap GenImage(List<Vector3[,]> frames)
        {
            int width = frames[0].GetLength(1);
            int height = frames[0].GetLength(0);
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 sum = Vector3.Zero;
                    foreach (var f in frames)
                    {
                        sum += f[i, j];
                    }
                    sum *= (1.0 / frames.Count);
                    //bmp.SetPixel(j, i, frame[i, j].ToColor());
                    bmp.SetPixel(j, i, sum.ToColor());
                }
            }
            return bmp;
        }
    }
}
