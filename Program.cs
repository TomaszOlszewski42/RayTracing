using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;


namespace RayTracing;

internal class Program
{
      
    static void Main(string[] args)
    {
        HittableList world = new HittableList();

        var material_ground = new Lambertian(new Color(0.8, 0.8, 0.0));
        var material_center = new Lambertian(new Color(0.1, 0.2, 0.5));
        var material_left = new Metal(new Color(0.8, 0.8, 0.8), 0.3);
        var material_right = new Metal(new Color(0.8, 0.6, 0.2), 1.0);

        world.Add(new Sphere(new Point3(0, -100.5, -1), 100, material_ground));
        world.Add(new Sphere(new Point3(0, 0, -1.2), 0.5, material_center));
        world.Add(new Sphere(new Point3(-1.0, 0, -1.0), 0.5, material_left));
        world.Add(new Sphere(new Point3(1.0, 0, -1), 0.5, material_right));

        Camera cam = new Camera();

        cam.aspectRatio = 16.0 / 9.0;
        cam.imageWidth = 400;
        cam.samples_per_pixel = 100;
        cam.max_depth = 50;

        cam.Render(ref world);
    }
}
