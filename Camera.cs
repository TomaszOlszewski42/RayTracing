using Point3 = RayTracing.Vec3;
using Color = RayTracing.Vec3;

namespace RayTracing;

internal class Camera
{
    public double aspectRatio = 1.0;
    public int imageWidth = 100;
    private int _imageHeight;
    private Point3 _center;
    private Point3 pixel00_loc;
    private Vec3 pixel_delta_u;
    private Vec3 pixel_delta_v;

    public Camera()
    {
        _center = new Point3();
        pixel00_loc = new Point3();
        pixel_delta_u = new Vec3();
        pixel_delta_v = new Vec3();
    }

    private void Initialize()
    {
        _imageHeight = (int)(imageWidth / aspectRatio);
        _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;
        _center = new Point3(0, 0, 0);

        var focal_length = 1.0;
        var viewport_height = 2.0;
        var viewport_width = viewport_height * ((double)imageWidth / _imageHeight);

        var viewport_u = new Vec3(viewport_width, 0, 0);
        var viewport_v = new Vec3(0, -viewport_height, 0);

        pixel_delta_u = viewport_u / imageWidth;
        pixel_delta_v = viewport_v / _imageHeight;

        var viewport_upper_left = _center - new Vec3(0, 0, focal_length) - viewport_u / 2 - viewport_v / 2;
        pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);
    }

    public void Render(ref HittableList world)
    {
        Initialize();
        var file = File.Create(".\\..\\..\\..\\image.ppm");
        var output = new StreamWriter(file);

        output.WriteLine($"P3\n{imageWidth} {_imageHeight}\n255");

        for (int j = 0; j < _imageHeight; j++)
        {
            Console.WriteLine($"Scanlines remaining: {_imageHeight - j}");
            for (int i = 0; i < imageWidth; i++)
            {
                var pixel_center = pixel00_loc + (i * pixel_delta_u) + (j * pixel_delta_v);
                var ray_direction = pixel_center - _center;
                Ray r = new(_center, ray_direction);

                Color pixel_color = Camera.RayColor(r, world);
                output.WriteLine(Color.WriteColor(pixel_color));
            }
        }

        Console.WriteLine("Done");
    }

    private static Color RayColor(Ray r, Hittable world)
    {
        var rec = new HitRecord();
        if (world.Hit(r, new Interval(0, float.PositiveInfinity), ref rec))
        {
            return 0.5 * (rec.normal + new Color(1, 1, 1));
        }

        Vec3 unit_direction = Vec3.UnitVector(r.Direction);
        var a = 0.5 * (unit_direction.Y + 1.0);
        return (1.0 - a) * (new Color(1.0, 1.0, 1.0)) + a * (new Color(0.5, 0.7, 1.0));
    }

}
