using Point3 = RayTracing.Vec3;

namespace RayTracing;

public class Sphere : Hittable
{
    private readonly Point3 _center;
    private readonly double _radius;
    private Material _mat;

    public Sphere(Point3 center, double radius, Material mat)
    {
        _center = center;
        _radius = Math.Max(0, radius);
        _mat = mat;
    }

    public override bool Hit(Ray r, Interval ray_t, ref HitRecord rec)
    {
        Vec3 oc = _center - r.Origin;
        var a = r.Direction.LengthSquared();
        var h = Vec3.Dot(r.Direction, oc);
        var c = oc.LengthSquared() - _radius*_radius;

        var discriminant = h * h - a * c;
        if (discriminant < 0) 
            return false;

        var sqrtd = Math.Sqrt(discriminant);

        var root = (h - sqrtd) / a;
        if(!ray_t.Surrounds(root))
        {
            root = (h + sqrtd) / a;
            if (!ray_t.Surrounds(root))
                return false;
        }

        rec.t = root;
        rec.p = r.At(rec.t)!;
        rec.normal = (rec.p - _center) / _radius;
        Vec3 outward_normal = (rec.p - _center) / _radius;
        rec.SetFaceNormal(r, outward_normal);
        rec.mat = _mat;

        return true;
    }
}
