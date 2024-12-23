using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;


namespace RayTracing;

internal class Program
{
      
    static void Main(string[] args)
    {
        // Output

        var file = File.Create(".\\..\\..\\..\\image.ppm");
        var output = new StreamWriter(file);

        // Image

        var aspect_ratio = 16.0 / 9.0;
        int image_width = 400;

        int image_height = (int)(image_width / aspect_ratio);
        image_height = (image_height < 1) ? 1 : image_height;

        // World

        var world = new HittableList();
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));

        // Camera

        var focal_length = 1.0;
        var viewport_height = 2.0;
        var viewport_width = viewport_height * ((double)(image_width) / image_height);
        var camera_center = new Point3(0, 0, 0);

        // Vectors across horizontal and dwon the vercial viewport edges

        var viewport_u = new Vec3(viewport_width, 0, 0);
        var viewport_v = new Vec3(0, -viewport_height, 0);

        // Horizontal and vertical delta vectors between pixels

        var pixel_delta_u = viewport_u / image_width;
        var pixel_delta_v = viewport_v / image_height;

        // Calculating location of upper left pixel

        var viewport_upper_left = camera_center - new Vec3(0, 0, focal_length) - viewport_u / 2 - viewport_v / 2;
        var pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);

        // Render

        output.WriteLine($"P3\n{image_width} {image_height}\n255");

        for(int j = 0; j < image_height; j++)
        {
            Console.WriteLine($"Scanlines remaining: {image_height - j}");
            for (int i = 0; i < image_width; i++) 
            {
                var pixel_center = pixel00_loc + (i * pixel_delta_u) + (j * pixel_delta_v);
                var ray_direction = pixel_center - camera_center;
                Ray r = new(camera_center, ray_direction);

                Color pixel_color = Ray.RayColor(r, world);
                output.WriteLine(Color.WriteColor(pixel_color));
            }
        }

        Console.WriteLine("Done");
    }
}
