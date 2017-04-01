using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ambi_win
{
    internal struct Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        
        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    internal class Frame
    {
        public Frame(IList<Color> pixels)
        {
            Pixels = pixels ?? throw new ArgumentNullException(nameof(pixels));
        }

        public IList<Color> Pixels { get; }
    }

    internal interface IFrameWriter
    {
        void Update(Frame frame);
    }
    
    internal class SerialFrameWriter : IFrameWriter, IDisposable
    {
        private readonly BinaryWriter _binaryWriter;
        private readonly SerialPort _serialPort;
        private bool _disposed;

        public SerialFrameWriter(string com, int baud)
        {
            _serialPort = new SerialPort(com, baud);
            _serialPort.Open();

            if (!_serialPort.IsOpen)
                throw new InvalidOperationException($"Failed to open serial '{com}'");

            _binaryWriter = new BinaryWriter(_serialPort.BaseStream, Encoding.UTF8);
        }

        public void Update(Frame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            var pixels = frame.Pixels;
            if (pixels.Count == 0)
                throw new InvalidOperationException("There aren't any pixels stored in the frame");

            _binaryWriter.Write((short) pixels.Count);
            foreach (var pixel in pixels)
            {
                _binaryWriter.Write(pixel.Red);
                _binaryWriter.Write(pixel.Green);
                _binaryWriter.Write(pixel.Blue);
            }

            int i = _serialPort.ReadByte();
            
        }

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            _serialPort.Close();
            _serialPort.Dispose();
            _binaryWriter.Dispose();

            _disposed = true;
        }
    }

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
