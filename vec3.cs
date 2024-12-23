using System.Net.WebSockets;

namespace RayTracing;

public class Vec3
{
    public double[] e = new double[3];

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

    public static string WriteColor(Vec3 pixel_color)
    {
        var r = pixel_color.X;
        var g = pixel_color.Y;
        var b = pixel_color.Z;

        int rbyte = (int)(r * 255.999);
        int gbyte = (int)(g * 255.999);
        int bbyte = (int)(b * 255.999);

        return $"{rbyte} {gbyte} {bbyte}";
    }
}
