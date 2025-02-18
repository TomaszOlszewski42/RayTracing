using System.Net.WebSockets;

namespace RayTracing;

public class Vec3
{
    public double[] e = new double[3];
    public static Interval intensity = new Interval(0.0, 0.999);

    public Vec3(double e0, double e1, double e2)
    {
        e[0] = e0;
        e[1] = e1;
        e[2] = e2;
    }

    public Vec3()
    {
        e = [0.0, 0.0, 0.0];
    }

    public double X => e[0];
    public double Y => e[1];
    public double Z => e[2];

    public static Vec3 operator -(Vec3 v) => new(-v.e[0], -v.e[1], -v.e[2]);
    public static Vec3 operator +(Vec3 v1, Vec3 v2) => new(v1.e[0] + v2.e[0], v1.e[1] + v2.e[1], v1.e[2] + v2.e[2]);
    public static Vec3 operator -(Vec3 v1, Vec3 v2) => new(v1.e[0] - v2.e[0], v1.e[1] - v2.e[1], v1.e[2] - v2.e[2]);
    public static Vec3 operator *(Vec3 v, double t) => new(v.e[0] * t, v.e[1] * t, v.e[2] * t);
    public static Vec3 operator *(double t, Vec3 v) => v * t;
    public static Vec3 operator /(Vec3 v, double t) => v * (1 / t);

    public double this[int i]
    {
        get => e[i];
    }

    public ref double GetRef(int i)
    {
        return ref e[i];
    }

    public double LengthSquared() => e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
    public double Length() => Math.Sqrt(LengthSquared());

    public override string ToString()
    {
        return $"{e[0]} {e[1]} {e[2]}";
    }

    public static double Dot(Vec3 u, Vec3 v) => u.e[0] * v.e[0] + u.e[1] * v.e[1] + u.e[2] * v.e[2];
    public static Vec3 Cross(Vec3 u, Vec3 v) => new(u.e[1] * v.e[2] - u.e[2] * v.e[1], u.e[2] * v.e[0] - u.e[0] * v.e[2], u.e[0] * v.e[1] - u.e[1] * v.e[0]);
    public static Vec3 UnitVector(Vec3 v) => v / v.Length();

    private static double LinearToGamma(double linear_component)
    {
        if(linear_component > 0)
            return Math.Sqrt(linear_component);

        return 0;
    }

    public static string WriteColor(Vec3 pixel_color)
    {
        var r = pixel_color.X;
        var g = pixel_color.Y;
        var b = pixel_color.Z;

        r = LinearToGamma(r);
        g = LinearToGamma(g);
        b = LinearToGamma(b);

        int rbyte = (int)(256 * intensity.Clamp(r));
        int gbyte = (int)(256 * intensity.Clamp(g));
        int bbyte = (int)(256 * intensity.Clamp(b));

        return $"{rbyte} {gbyte} {bbyte}";
    }

    public static Vec3 RandomVec()
    {
        return new Vec3(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble());
    }

    public static Vec3 RandomVec(double min, double max)
    {
        return new Vec3(Rtweekend.RandomDouble(min, max), Rtweekend.RandomDouble(min, max), Rtweekend.RandomDouble(min, max));
    }

    public static Vec3 RandomUnitVector()
    {
        while(true)
        {
            var p = RandomVec(-1, 1);
            var lensq = p.LengthSquared();
            if ((1e-160 < lensq) && (lensq <= 1))
            {
                return p / Math.Sqrt(lensq);
            }
        }
    }

    public static Vec3 RandomOnHemisphere(ref Vec3 normal)
    {
        Vec3 on_unit_sphere = RandomUnitVector();
        if(Dot(on_unit_sphere, normal) > 0.0)
            return on_unit_sphere;
        else
            return -on_unit_sphere;
    }
}
