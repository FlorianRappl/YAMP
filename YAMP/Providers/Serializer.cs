using System;
using System.IO;
using System.Text;

namespace YAMP
{
    class Serializer : IDisposable
    {
        MemoryStream stream;

        private Serializer()
        {
            stream = new MemoryStream();
        }

        public static Serializer Create()
        {
            return new Serializer();
        }


        public Serializer Serialize(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var length = BitConverter.GetBytes(bytes.Length);
            stream.Write(length, 0, length.Length);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(bool value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        public Serializer Serialize(ScalarValue value)
        {
            var real = BitConverter.GetBytes(value.Value);
            stream.Write(real, 0, real.Length);
            var imag = BitConverter.GetBytes(value.ImaginaryValue);
            stream.Write(imag, 0, imag.Length);
            return this;
        }

        public Serializer Serialize(byte[] content)
        {
            stream.Write(content, 0, content.Length);
            return this;
        }

        public byte[] Value
        {
            get
            {
                return stream.ToArray();
            }
        }

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
