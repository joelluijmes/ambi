using System.Linq;
using ambi_win.driver;

namespace ambi_win
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var frameWriter = new SerialFrameWriter("COM3", 9600);

            byte red = 255;
            byte green = 0;
            byte blue = 0;

            const int delta = 1;
            const int itr = 255 / delta;

            while (true)
            {
                for (var i = 0; i < itr; ++i)
                {
                    red -= delta;
                    green += delta;

                    var frame = new Frame(Enumerable.Range(0, 60).Select(x => new Color(red, green, blue)).ToArray());
                    frameWriter.Update(frame);
                }

                for (var i = 0; i < itr; ++i)
                {
                    green -= delta;
                    blue += delta;

                    var frame = new Frame(Enumerable.Range(0, 60).Select(x => new Color(red, green, blue)).ToArray());
                    frameWriter.Update(frame);
                }

                for (var i = 0; i < itr; ++i)
                {
                    blue -= delta;
                    red += delta;

                    var frame = new Frame(Enumerable.Range(0, 60).Select(x => new Color(red, green, blue)).ToArray());
                    frameWriter.Update(frame);
                }
            }
        }
    }
}
