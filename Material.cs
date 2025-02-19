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

public class Dielectric : Material
{
    private double _refraction_index;

    public Dielectric(double refraction_index)
    {
        _refraction_index = refraction_index;
    }

    private static double Reflectance(double cosine, double refraction_index)
    {
        var r0 = (1 - refraction_index) / (1 + refraction_index);
        r0 = r0 * r0;
        return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
    }

    public override bool Scatter(Ray r_in, HitRecord rec, ref Color attenuation, ref Ray scattered)
    {
        attenuation = new Color(1.0, 1.0, 1.0);
        double ri = rec.frontFace ? (1.0 / _refraction_index) : _refraction_index;

        Vec3 unit_direction = Vec3.UnitVector(r_in.Direction);
        double cos_theta = Math.Min(Vec3.Dot(-unit_direction, rec.normal), 1.0);
        double sin_theta = Math.Sqrt(1.0 - cos_theta * cos_theta);

        bool cannot_refract = ri * sin_theta > 1.0;
        Vec3 direction;

        if(cannot_refract || Reflectance(cos_theta, ri) > Random.Shared.NextDouble())
        {
            direction = Vec3.Reflect(unit_direction, rec.normal);
        }
        else
        {
            direction = Vec3.Refract(unit_direction, rec.normal, ri);
        }

        scattered = new Ray(rec.p, direction);
        return true;
    }
}