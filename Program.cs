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
        var material_left = new Dielectric(1.5);
        var material_bubble = new Dielectric(1.0 / 1.5);
        var material_right = new Metal(new Color(0.8, 0.6, 0.2), 1.0);

        world.Add(new Sphere(new Point3( 0.0, -100.5, -1.0), 100.0, material_ground));
        world.Add(new Sphere(new Point3( 0.0,    0.0, -1.2),   0.5, material_center));
        world.Add(new Sphere(new Point3(-1.0,    0.0, -1.0),   0.5, material_left));
        world.Add(new Sphere(new Point3(-1.0,    0.0, -1.0),   0.4, material_bubble));
        world.Add(new Sphere(new Point3( 1.0,    0.0, -1.0),   0.5, material_right));

        Camera cam = new Camera();

        cam.aspectRatio = 16.0 / 9.0;
        cam.imageWidth = 400;
        cam.samples_per_pixel = 100;
        cam.max_depth = 50;
        
        cam.vfov = 20;
        cam.lookfrom = new Point3(-2, 2, 1);
        cam.lookat = new Point3(0, 0, -1);
        cam.vup = new Vec3(0, 1, 0);

        cam.Render(ref world);
    }
}
