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
            Console.WriteLine(mat.Reversed);
        }
    }
}
