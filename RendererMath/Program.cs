using System;
using System.Collections.Generic;
using System.Drawing;

namespace RendererMath
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Console.WriteLine("Hello World!");
            //Matrix3 mat = new Matrix3(new double[][] {
            //    new double[]{ 3,   7,  -3},
            //    new double[]{-2,  -5,   2 },
            //    new double[]{-4, -10,   3 }
            //});

            //Console.WriteLine(mat);
            //Console.WriteLine(mat.Det);
            //Console.WriteLine(mat.Inv);

            //Console.WriteLine("\n\nTriangle");
            //Triangle t = new Triangle(Vector3.Zero, new Vector3(1, 0, 0), new Vector3(0, 2, 0));
            ////Console.WriteLine(t.PlaneNormal);
            //Console.WriteLine(t);
            //Console.WriteLine("\n\nRay");
            //Ray ray = new Ray(new Vector3(0, 0, 100), new Vector3(0.2, 0.3, -1));
            //Console.WriteLine(ray);

            //Console.WriteLine("\n\nIntersection");
            //var intec = ray.IntersectionTest(t);
            //Console.WriteLine(intec);

            Vector3[] V = new Vector3[]
            {
                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(0,0,1),
                new Vector3(1,0,1),
                new Vector3(0,1,0),
                new Vector3(1,1,0),
                new Vector3(0,1,1),
                new Vector3(1,1,1)
            };

            var matLightSrc = new LightSourceMaterial();


            List<Triangle> triangles = new List<Triangle>()
            {
                //new Triangle(new Vector3(0,0,0), new Vector3(100,0,0), new Vector3(0,0.1,0)),
                //new Triangle(new Vector3(0,0,0), new Vector3(0,100,0), new Vector3(0.1,0,0))
                new Triangle(new Vector3(1,0,-4), new Vector3(2,0,-4), new Vector3(1,5,-4)),
                new Triangle(new Vector3(1,5,-4), new Vector3(2,0,-4), new Vector3(2,5,-4)),
                new Triangle(new Vector3(-10, 0, -10), new Vector3(-10,0,10), new Vector3(10, 0, -10)),
                new Triangle(new Vector3(10, 0, -10), new Vector3(-10, 0, 10), new Vector3(10, 0, 10)),

                new Triangle(V[0], V[1], V[2]),
                new Triangle(V[2], V[1], V[3]),

                new Triangle(V[4], V[6], V[5]),
                new Triangle(V[5], V[6], V[7]),

                new Triangle(V[3], V[1], V[7]),
                new Triangle(V[7], V[1], V[5]),

                new Triangle(V[2], V[6], V[0]),
                new Triangle(V[0], V[6], V[4]),

                new Triangle(V[2], V[3], V[6]),
                new Triangle(V[6], V[3], V[7]),

                new Triangle(V[0], V[4], V[1]),
                new Triangle(V[1], V[4], V[5]),
            };

            triangles[0].material = matLightSrc;
            triangles[1].material = matLightSrc;


            Camera cam = new Camera();

            var pos = new Vector3(1.5, 5, 3);
            var dir = new Vector3(-1.5, -3, -3);
            var up = new Vector3(0, 1, 0);
            int width = 400;
            int height = 300;

            cam.Setup(pos, dir, up, width, height);
            var depthMap = cam.DepthScan(triangles, 1.0, 20.0);
            Camera.GenImage(depthMap).Save("depth.jpg");    

            var intersection = cam.IntersectionTest(cam.pixelRays[145, 230], triangles);

            List<Vector3[,]> frames = new List<Vector3[,]>();
            for (int i = 0; i < 100; i++)
            {
                var frame = cam.Capture(triangles,2);
                frames.Add(frame);
                var bmp = Camera.GenImage(frames);
                bmp.Save(string.Format("frame_{0}.jpg", i));
            }

        }
    }
}
