using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EarthSim.Utils
{
    public class RGB2HSL
    {
        public static HSL ToHSL(Color c1)
        {
            double themin, themax, delta;
            HSL c2 = new HSL();

            themin = Math.Min(c1.R, Math.Min(c1.G, c1.B));
            themax = Math.Max(c1.R, Math.Max(c1.G, c1.B));
            delta = themax - themin;
            c2.L = (themin + themax) / 2;
            c2.S = 0;
            if (c2.L > 0 && c2.L < 1)
                c2.S = delta / (c2.L < 0.5 ? (2 * c2.L) : (2 - 2 *
                c2.L));
            c2.H = 0;
            if (delta > 0)
            {
                if (themax == c1.R && themax != c1.G)
                    c2.H += (c1.G - c1.B) / delta;
                if (themax == c1.G && themax != c1.B)
                    c2.H += (2 + (c1.B - c1.R) / delta);
                if (themax == c1.B && themax != c1.R)
                    c2.H += (4 + (c1.R - c1.G) / delta);
                c2.H *= 60;
            }
            return (c2);
        }
    }
}