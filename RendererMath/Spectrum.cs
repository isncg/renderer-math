using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RendererMath
{

    class Spectrum
    {
        public double r, g, b;
        public Color ToColor(double whiteBalance)
        {
            return Color.FromArgb(
                (int)(255 * r / whiteBalance),
                (int)(255 * g / whiteBalance),
                (int)(255 * b / whiteBalance),
                255);
        }

        public static Spectrum Zero
        {
            get
            {
                return new Spectrum() { r = 0, g = 0, b = 0 };
            }

        }

        public static Spectrum One
        {
            get
            {
                return new Spectrum() { r = 1, g = 1, b = 1 };
            }
        }
    }
}
