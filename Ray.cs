using Point3 = RayTracing.Vec3;
using Color = RayTracing.Vec3;
namespace RayTracing;

internal class Ray
{
    public Point3 Origin { get; }
    public Vec3 Direction { get; }

    // public Ray() { }
    public Ray(Point3 origin, Vec3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Point3? At(double t)
    {
        if (Origin == null || Direction == null) return null;
        return Origin + t * Direction;
    }

    public static double HitSphere(Point3 center, double radius, Ray r)
    {
        Vec3 oc = center - r.Origin;
        var a = r.Direction.LengthSquared();
        var h = Vec3.Dot(r.Direction, oc);
        var c = oc.LengthSquared() - radius * radius;
        var discriminant = h * h - a * c;

        if (discriminant < 0)
            return -1.0;
        else
            return (h - Math.Sqrt(discriminant)) / a;
    }
}
