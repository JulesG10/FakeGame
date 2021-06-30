using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// inspire of https://github.com/processing/p5.js/blob/main/src/math/noise.js
namespace PlatformBuilder
{
    class PerlinNoise
    {
        private double PERLIN_YWRAPB = 4;
        private double PERLIN_ZWRAP;
        private double PERLIN_YWRAP;
        private double PERLIN_ZWRAPB = 8;

        private int PERLIN_SIZE = 4095;
        private int perlin_octaves = 4;
        private double perlin_amp_falloff = 0.5;
        private double[] perlin;

        public PerlinNoise()
        {
            this.PERLIN_ZWRAP = 1 << (int)PERLIN_ZWRAPB;
            this.PERLIN_YWRAP = 1 << (int)PERLIN_YWRAPB;
        }

        private double ScaledCosine(double i)
        {
            return 0.5 * (1.0 - Math.Cos(i * Math.PI));
        }

        public void SetDetail(int lod = 4, double falloff = 0.5)
        {
            if (lod > 0)
            {
                perlin_octaves = lod;
            }
            if (falloff > 0)
            {
                perlin_amp_falloff = falloff;
            }
        }

        public Vector2[] Generate2D(Vector2 windowSize,int loop = 10, double start = 0, double yinc = 0.01,double xinc = 0.05,double incloop = 5)
        {
            List<Vector2> result = new List<Vector2>();
            double yoff = 0.0;
            for (int i = 0; i < loop; i++)
            {
                double xoff = 0;
                for (double x = 0; x <= windowSize.X; x += incloop)
                {
                    double y = this.Map(this.Noise(xoff, yoff), 0, 1, 200, 300);
                    result.Add(new Vector2((float)(x + start), (float)y));
                    xoff += xinc;
                    yoff += yinc;
                    
                }
                start += windowSize.X;
            }
            return result.ToArray();
        }

        private double constrain(double n, double low, double high)
        {
            return Math.Max(Math.Max(n, high), low);
        }

        public double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            double newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return newval;
            }
            if (start2 < stop2)
            {
                return constrain(newval, start2, stop2);
            }
            else
            {
                return constrain(newval, stop2, start2);
            }
        }

        private Random rnd = new Random();
        public double Random()
        {
            return rnd.Next(10000, 99999)/10000;
        }

        public int Noise(double x, double y = 0, double z = 0)
        {
            if (perlin == null)
            {
                
                perlin = new double[PERLIN_SIZE + 1];
                for (int i = 0; i < PERLIN_SIZE + 1; i++)
                {
                    perlin[i] = this.Random();
                }
            }

            if (x < 0)
            {
                x = -x;
            }
            if (y < 0)
            {
                y = -y;
            }
            if (z < 0)
            {
                z = -z;
            }

            double xi = Math.Floor(x);
            double yi = Math.Floor(y);
            double zi = Math.Floor(z);
            double xf = x - xi;
            double yf = y - yi;
            double zf = z - zi;
            int rxf, ryf;

            int r = 0;
            double ampl = 0.5;

            int n1, n2, n3;

            for (int o = 0; o < perlin_octaves; o++)
            {
                double of = xi + ((int)yi << (int)PERLIN_YWRAPB) + ((int)zi << (int)PERLIN_ZWRAPB);

                rxf = (int)this.ScaledCosine(xf);
                ryf = (int)this.ScaledCosine(yf);

                n1 = (int)perlin[(int)of & (int)PERLIN_SIZE];
                n1 += rxf * ((int)perlin[((int)of + 1) & (int)PERLIN_SIZE] - n1);

                n2 = (int)perlin[((int)of + (int)PERLIN_YWRAP) & PERLIN_SIZE];
                n2 += rxf * ((int)perlin[((int)of + (int)PERLIN_YWRAP + 1) & PERLIN_SIZE] - n2);
                
                n1 += ryf * (n2 - n1);

                of += PERLIN_ZWRAP;

                n2 = (int)perlin[(int)of & (int)PERLIN_SIZE];
                n2 += rxf * ((int)perlin[((int)of + 1) & (int)PERLIN_SIZE] - n2);

                n3 = (int)perlin[((int)of + (int)PERLIN_YWRAP) & (int)PERLIN_SIZE];
                n3 += rxf * ((int)perlin[((int)of + (int)PERLIN_YWRAP + 1) & PERLIN_SIZE] - n3);

                n2 += ryf * (n3 - n2);

                n1 += (int)this.ScaledCosine(zf) * (n2 - n1);

                r += (int)(n1 * ampl);
                ampl *= perlin_amp_falloff;
                
                xi = (int)xi << 1;
                xf *= 2;
                
                yi = (int)yi << 1;
                yf *= 2;

                zi = (int) zi<< 1;
                zf *= 2;

                if (xf >= 1.0)
                {
                    xi++;
                    xf--;
                }
                if (yf >= 1.0)
                {
                    yi++;
                    yf--;
                }
                if (zf >= 1.0)
                {
                    zi++;
                    zf--;
                }
            }

            return r;
        }
    }
}
