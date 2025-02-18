using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;

internal class Rtweekend
{
    public static double Pi = 3.1415926535897932385;

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Pi / 180.0;
    }

    public static double RandomDouble(double min, double max)
    {
        return Random.Shared.NextDouble() * (max - min) + min;
    }
}
