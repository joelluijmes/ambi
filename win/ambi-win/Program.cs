using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ambi_win
{
    internal struct Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        
        public Color(byte red = 0, byte green = 0, byte blue = 0)
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
            var frameWriter = new SerialFrameWriter("COM3", 115200);
            var frame = new Frame(Enumerable.Range(0, 60).Select(i => new Color(42, 42, 42)).ToArray());

            frameWriter.Update(frame);
        }
    }
}
