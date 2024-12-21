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

    public static Color RayColor(Ray r)
    {
        Vec3 unit_direction = Vec3.UnitVector(r.Direction);
        var a = 0.5 * (unit_direction.Y + 1.0);
        return (1.0 - a) * (new Color(1.0, 1.0, 1.0)) + a * (new Color(0.5, 0.7, 1.0));
    }
}
