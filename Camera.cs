using Point3 = RayTracing.Vec3;
using Color = RayTracing.Vec3;

namespace RayTracing;

public class Camera
{
    public double aspectRatio = 1.0;
    public int imageWidth = 100;
    public int samples_per_pixel = 10;
    public int max_depth = 10;

    private int _imageHeight;
    private Point3 _center;
    private Point3 _pixel00_loc;
    private Vec3 _pixel_delta_u;
    private Vec3 _pixel_delta_v;
    private double _pixel_samples_scale;

    public Camera()
    {
        _center = new Point3();
        _pixel00_loc = new Point3();
        _pixel_delta_u = new Vec3();
        _pixel_delta_v = new Vec3();
    }

    private void Initialize()
    {
        _imageHeight = (int)(imageWidth / aspectRatio);
        _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;
        _center = new Point3(0, 0, 0);
        _pixel_samples_scale = 1.0 / samples_per_pixel;

        var focal_length = 1.0;
        var viewport_height = 2.0;
        var viewport_width = viewport_height * ((double)imageWidth / _imageHeight);

        var viewport_u = new Vec3(viewport_width, 0, 0);
        var viewport_v = new Vec3(0, -viewport_height, 0);

        _pixel_delta_u = viewport_u / imageWidth;
        _pixel_delta_v = viewport_v / _imageHeight;

        var viewport_upper_left = _center - new Vec3(0, 0, focal_length) - viewport_u / 2 - viewport_v / 2;
        _pixel00_loc = viewport_upper_left + 0.5 * (_pixel_delta_u + _pixel_delta_v);
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
                Color pixel_color = new Color(0, 0, 0);
                for(int sample = 0; sample < samples_per_pixel; sample++)
                {
                    Ray r = GetRay(i, j);
                    pixel_color += RayColor(r, max_depth, world);
                }
                output.WriteLine(Color.WriteColor(pixel_color * _pixel_samples_scale));
            }
        }
        output.Flush();
        output.Close();
        Console.WriteLine("Done");
    }

    private static Color RayColor(Ray r, int depth, Hittable world)
    {
        if (depth <= 0)
            return new Color(0, 0, 0);

        var rec = new HitRecord();
        if (world.Hit(r, new Interval(0.001, float.PositiveInfinity), ref rec))
        {
            // Vec3 direction = Vec3.RandomOnHemisphere(ref rec.normal);
            //Vec3 direction = rec.normal + Vec3.RandomUnitVector();
            //return 0.5 * RayColor(new Ray(rec.p, direction), depth - 1, world);

            Ray scattered = new Ray();
            Color attenuation = new Color();
            if(rec.mat ==  null) return new Color(0, 0, 0);
            if (rec.mat.Scatter(r, rec, ref attenuation, ref scattered))
                return attenuation * RayColor(scattered, depth - 1, world);
            return new Color(0, 0, 0);

        }

        Vec3 unit_direction = Vec3.UnitVector(r.Direction);
        var a = 0.5 * (unit_direction.Y + 1.0);
        return (1.0 - a) * (new Color(1.0, 1.0, 1.0)) + a * (new Color(0.5, 0.7, 1.0));
    }

    Vec3 SampleSquare()
    {
        return new Vec3(Random.Shared.NextDouble() - 0.5, Random.Shared.NextDouble() - 0.5, 0);
    }

    private Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixel_sample = _pixel00_loc + ((i + offset.X) * _pixel_delta_u) + ((j + offset.Y) * _pixel_delta_v);
        var ray_origin = _center;
        var ray_direction = pixel_sample - ray_origin;

        return new Ray(ray_origin, ray_direction);
    }
}
