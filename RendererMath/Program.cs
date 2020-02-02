using System;

namespace RendererMath
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Matrix3 mat = new Matrix3(new double[][] {
                new double[]{ 3,   7,  -3},
                new double[]{-2,  -5,   2 },
                new double[]{-4, -10,   3 }
            });

            Console.WriteLine(mat);
            Console.WriteLine(mat.Det);
            Console.WriteLine(mat.Inv);
            
            Console.WriteLine("\n\nTriangle");
            Triangle t = new Triangle(Vector3.Zero, new Vector3(1, 0, 0), new Vector3(0, 2, 0));
            //Console.WriteLine(t.PlaneNormal);
            Console.WriteLine(t);
            Console.WriteLine("\n\nRay");
            Ray ray = new Ray(new Vector3(0, 0, 100), new Vector3(0.2, 0.3, -1));
            Console.WriteLine(ray);

            Console.WriteLine("\n\nIntersection");
            var intec = ray.IntersectionTest(t);
            Console.WriteLine(intec);
        }
    }
}
