using System;
using System.IO;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// The deserializer class for helping in the serialization process.
    /// </summary>
    class Deserializer : IDisposable
    {
        #region Members

        MemoryStream stream;
        int length;

        #endregion

        #region ctor

        private Deserializer(byte[] content)
        {
            length = content.Length;
            stream = new MemoryStream(content);
            stream.Position = 0;
        }

        /// <summary>
        /// Creates a new instance of the deserialization helper.
        /// </summary>
        /// <param name="content">The content to deserialize.</param>
        /// <returns>The new instance.</returns>
        public static Deserializer Create(byte[] content)
        {
            return new Deserializer(content);
        }

        #endregion

        #region Members

        /// <summary>
        /// Reads the next bytes as boolean.
        /// </summary>
        /// <returns>The boolean.</returns>
        public bool GetBoolean()
        {
            var c = new byte[1];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToBoolean(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as float.
        /// </summary>
        /// <returns>The float.</returns>
        public float GetSingle()
        {
            var c = new byte[4];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToSingle(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as long.
        /// </summary>
        /// <returns>The long.</returns>
        public long GetLong()
        {
            var c = new byte[8];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToInt64(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as string.
        /// </summary>
        /// <returns>The string.</returns>
        public string GetString()
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            var length = BitConverter.ToInt32(buffer, 0);
            buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.Unicode.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads the next bytes as scalar (2 doubles).
        /// </summary>
        /// <returns>The scalar.</returns>
        public ScalarValue GetScalar()
        {
            var c = new byte[16];
            stream.Read(c, 0, c.Length);
            var real = BitConverter.ToInt32(c, 0);
            var imag = BitConverter.ToDouble(c, 8);
            return new ScalarValue(real, imag);
        }

        /// <summary>
        /// Reads the next bytes as double.
        /// </summary>
        /// <returns>The double.</returns>
        public double GetDouble()
        {
            var c = new byte[8];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToDouble(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as integer.
        /// </summary>
        /// <returns>The integer.</returns>
        public int GetInt()
        {
            var c = new byte[4];
            stream.Read(c, 0, c.Length);
            return BitConverter.ToInt32(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as raw bytes.
        /// </summary>
        /// <returns>The binary array.</returns>
        public byte[] GetBytes()
        {
            var length = GetInt();
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Cleans up the mess.
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
        }

        #endregion
    }
}
