using System;
using System.IO;
using System.Text;

namespace YAMP
{
    class Deserializer : IDisposable
    {
        MemoryStream stream;
        int length;

        private Deserializer(byte[] content)
        {
            length = content.Length;
            stream = new MemoryStream(content);
            stream.Position = 0;
        }

        public static Deserializer Create(byte[] content)
        {
            return new Deserializer(content);
        }

        public bool GetBoolean()
        {
            var c = new byte[1];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToBoolean(c, 0);
        }

        public float GetSingle()
        {
            var c = new byte[4];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToSingle(c, 0);
        }

        public long GetLong()
        {
            var c = new byte[8];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToInt64(c, 0);
        }

        public string GetString()
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            var length = BitConverter.ToInt32(buffer, 0);
            buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.Unicode.GetString(buffer, 0, buffer.Length);
        }

        public ScalarValue GetScalar()
        {
            var c = new byte[16];
            stream.Read(c, 0, c.Length);
            var real = BitConverter.ToInt32(c, 0);
            var imag = BitConverter.ToDouble(c, 8);
            return new ScalarValue(real, imag);
        }

        public double GetDouble()
        {
            var c = new byte[8];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToDouble(c, 0);
        }

        public int GetInt()
        {
            var c = new byte[4];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToInt32(c, 0);
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
