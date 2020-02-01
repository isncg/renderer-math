using System;
using System.Collections.Generic;
using System.Text;

namespace RendererMath
{
    class Vector3
    {
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual double Z { get; set; }

        public double MagnitudeSqr
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z);
            }
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(this.MagnitudeSqr);
            }
        }

        public void Normalize()
        {
            var l = Magnitude;
            X /= l;
            Y /= l;
            Z /= l;
        }
    }
}
