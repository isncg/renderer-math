using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    public class RandomDirections
    {
        //static Vector3[] _directions = null;

        static Vector3[][] _randomDirectionGroup = null;
        static int _groupIndex;

        static Random random = new Random();
        public static Vector3[] Get()
        {
            int cnt = 512;
            _groupIndex = 0;
            if (_randomDirectionGroup == null)
            {
                _randomDirectionGroup = new Vector3[cnt][];
                for (int i = 0; i < cnt; i++)
                {
                    var _directions = new Vector3[cnt];
                    for (int j = 0; j < cnt; j++)
                    {
                        Vector3 vec = new Vector3(random.NextDouble() - 0.5, random.NextDouble() - 0.5, random.NextDouble() - 0.5);
                        vec.Normalize();
                        _directions[j] = vec;

                    }
                    _randomDirectionGroup[i] = _directions;
                }
            }
            var result = _randomDirectionGroup[_groupIndex];
            _groupIndex++;
            _groupIndex = _groupIndex % cnt;
            return result;

        }

        //public static Vector3[] Directions
        //{
        //    get
        //    {
        //        int cnt = 64;
        //        if (_directions == null)
        //        {
        //            Random random = new Random();
        //            _directions = new Vector3[cnt];
        //            for (int i = 0; i < cnt; i++)
        //            {
        //                Vector3 vec = new Vector3(random.NextDouble() - 0.5, random.NextDouble() - 0.5, random.NextDouble() - 0.5);
        //                vec.Normalize();
        //                _directions[i] = vec;

        //            }
        //        }
        //        return _directions;
        //    }
        //}
    }


    public interface LightDistribution
    {
        public Vector3 GetFactor(Vector3 position, Vector3 normal, Vector3 incomeDirection, Vector3 leaveDirection);
    }


    //class Ambient : LightDistribution
    //{
    //    Vector3 AmbientColor = Vector3.One;
    //    public Vector3 GetFactor(Vector3 position, Vector3 normal, Vector3 incomeDirection, Vector3 leaveDirection)
    //    {
    //        return AmbientColor;
    //    }
    //}

    public class DiffuseReflection : LightDistribution
    {
        public Vector3 GetFactor(Vector3 position, Vector3 normal, Vector3 incomeDirection, Vector3 leaveDirection)
        {
            if (normal * leaveDirection <= 0)
            {
                return new ZeroVector3();
            }

            double factor = normal * (-incomeDirection);

            return new Vector3(factor, factor, factor);
        }
    }

    public class SpecularReflection : LightDistribution
    {
        public Vector3 GetFactor(Vector3 position, Vector3 normal, Vector3 incomeDirection, Vector3 leaveDirection)
        {
            return new ZeroVector3();
        }
    }

    public class Material
    {
        public virtual Vector3 Ambient(Vector3 normal, Vector3 direction)
        {
            return Vector3.Zero;
        }

        public List<LightDistribution> lightDistributions = new List<LightDistribution>();
    }

    public class TypicalMaterial : Material
    {
        public DiffuseReflection diffuse;
        public SpecularReflection specular;

        public override Vector3 Ambient(Vector3 normal, Vector3 direction)
        {
            return new Vector3(0.02, 0.02, 0.02);
        }

        public TypicalMaterial()
        {
            //Ambient = new Vector3(0.02, 0.02, 0.02);
            diffuse = new DiffuseReflection();
            specular = new SpecularReflection();
            this.lightDistributions.Add(diffuse);
            this.lightDistributions.Add(specular);
        }
    }


    public class LightSourceMaterial : Material
    {
        Vector3 strength = new Vector3(50, 50, 50);
        public override Vector3 Ambient(Vector3 normal, Vector3 direction)
        {
            if (normal * direction > 0)
            {
                return strength;
            }
            return new ZeroVector3();
            //return base.Ambient(normal, direction);
        }
        //public LightSourceMaterial()
        //{
        //    Ambient = new Vector3(50, 50, 50);
        //}
    }
}
