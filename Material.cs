using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Color = RayTracing.Vec3;

namespace RayTracing;

public abstract class Material
{
    public virtual bool Scatter(Ray r_in, HitRecord rec, ref Color attenuation, ref Ray scattered)
    {
        return false;
    }
}

public class Lambertian : Material
{
    private Color _albedo;

    public Lambertian(Color albedo)
    {
        _albedo = albedo;
    }

    public override bool Scatter(Ray r_in, HitRecord rec, ref Color attenuation, ref Ray scattered)
    {
        var scatter_direction = rec.normal + Vec3.RandomUnitVector();

        if (scatter_direction.NearZero())
            scatter_direction = rec.normal;

        scattered = new Ray(rec.p, scatter_direction);
        attenuation = _albedo;
        return true;
    }

}

public class Metal : Material
{
    private Color _albedo;
    private double _fuzz;

    public Metal(Color albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz < 1 ? fuzz : 1;
    }

    public override bool Scatter(Ray r_in, HitRecord rec, ref Color attenuation, ref Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(r_in.Direction, rec.normal);
        reflected = Vec3.UnitVector(reflected) + (_fuzz * Vec3.RandomUnitVector());
        scattered = new Ray(rec.p, reflected);
        attenuation = _albedo;
        return (Vec3.Dot(scattered.Direction, rec.normal) > 0);
    }
}