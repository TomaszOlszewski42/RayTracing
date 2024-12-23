using Point3 = RayTracing.Vec3;

namespace RayTracing;

internal class HitRecord
{
    public Point3 p;
    public Vec3 normal;
    public double t;
    public bool frontFace;

    public HitRecord()
    {
        normal = new Vec3();
        p = new Point3();
    }

    public HitRecord(Point3 p, Point3 normal, double t)
    {
        this.p = p;
        this.normal = normal;
        this.t = t;
    }

    public void SetFaceNormal(Ray r, Vec3 outward_normal)
    {
        frontFace = Vec3.Dot(r.Direction, outward_normal) < 0;
        normal = frontFace ? outward_normal : -outward_normal;
    }
}

internal abstract class Hittable
{
    public abstract bool Hit(Ray r, Interval ray_t, ref HitRecord rec);
}
