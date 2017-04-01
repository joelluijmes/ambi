using System;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace ambi_win
{
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

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            _serialPort.Close();
            _serialPort.Dispose();
            _binaryWriter.Dispose();

            _disposed = true;
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

            var ack = _serialPort.ReadByte();
            if (ack != 0xCC)
                throw new InvalidOperationException("The driver did not respond with the acknowledge");
        }
    }
}
