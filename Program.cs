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
            for (int j = 0; j < imageWidth; j++) 
            {
                var r = (double)i / (imageWidth - 1);
                var g = (double)j / (imageHeight - 1);
                var b = 0.0;

                int ir = (int)(255.999 * r);
                int ig = (int)(255.999 * g);
                int ib = (int)(255.999 * b);

                output.WriteLine($"{ir} {ig} {ib}");
            }
        }
        
    }
}
