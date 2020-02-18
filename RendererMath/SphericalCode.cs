using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RendererMath
{
    public class SphericalCode
    {
        public static List<Vector3> Build(List<double> nums)
        {
            List<Vector3> result = new List<Vector3>();
            int i = 0;
            while (i * 3 + 2 < nums.Count)
            {
                Vector3 v = new Vector3(nums[i * 3], nums[i * 3 + 1], nums[i * 3 + 2]);
                result.Add(v);
                i++;
            }
            return result;
        }

        public static List<Vector3> Build(string fname)
        {
            var f = File.OpenText(fname);
            var nums = new List<double>();
            while (!f.EndOfStream)
            {
                var l = f.ReadLine();
                if (l == null || l.Length <= 0)
                {
                    break;
                }
                double n = double.Parse(l);
                nums.Add(n);
            }
            return Build(nums);
        }

        //public static List<Vector3> SC20_PACK_260 = Build("ipack.3.260.txt");
        //public static List<Vector3> SC20_IPACK_512 = Build("ipack.3.512.txt");
        //public static List<Vector3> SC20_IPACK_1022 = Build("ipack.3.1022.txt");

        //public static List<Vector3> SC_PACK_32 = Build("pack.3.32.txt");
        //public static List<Vector3> SC_PACK_64 = Build("pack.3.64.txt");
        //public static List<Vector3> SC_PACK_128 = Build("pack.3.128.txt");

        static List<Vector3>[] depth_dest_map = null;
        public static List<Vector3>[] DepthDestMap
        {
            get
            {
                if (depth_dest_map == null)
                {
                    depth_dest_map = new List<Vector3>[]
                    {
                        null,
                         Build("pack.3.32.txt"),
                         Build("ipack.3.1022.txt")
                    };
                }
                return depth_dest_map;
            }

        }


    }
}
