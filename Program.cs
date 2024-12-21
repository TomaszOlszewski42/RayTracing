using Color = RayTracing.Vec3.Vec3;

namespace RayTracing;

internal class Program
{
      
    static void Main(string[] args)
    {
        int imageHeight = 256;
        int imageWidth = 256;

        var file = File.Create(".\\..\\..\\..\\image.ppm");
        var output = new StreamWriter(file);

        output.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");

        for(int i = 0; i < imageHeight; i++)
        {
            Console.WriteLine($"Scanlines remaining: {imageHeight - i}");
            for (int j = 0; j < imageWidth; j++) 
            {
                var pixel_color = new Color((double)j/(imageWidth -1 ), (double)i/(imageHeight -1), 0);
                output.WriteLine(Color.WriteColor(pixel_color));
            }
        }
        
    }
}
