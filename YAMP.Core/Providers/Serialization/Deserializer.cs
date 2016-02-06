namespace YAMP
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The deserializer class for helping in the serialization process.
    /// </summary>
    sealed class Deserializer : IDisposable
    {
        #region Fields

        readonly MemoryStream _stream;
        readonly Int32 _length;

        #endregion

        #region ctor

        private Deserializer(Byte[] content)
        {
            _length = content.Length;
            _stream = new MemoryStream(content);
            _stream.Position = 0;
        }

        /// <summary>
        /// Creates a new instance of the deserialization helper.
        /// </summary>
        /// <param name="content">The content to deserialize.</param>
        /// <returns>The new instance.</returns>
        public static Deserializer Create(Byte[] content)
        {
            return new Deserializer(content);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Reads the next bytes as boolean.
        /// </summary>
        /// <returns>The boolean.</returns>
        public Boolean GetBoolean()
        {
            var c = new Byte[1];
            _stream.Read(c, 0, c.Length);
            return BitConverter.ToBoolean(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as float.
        /// </summary>
        /// <returns>The float.</returns>
        public Single GetSingle()
        {
            var c = new Byte[4];
            _stream.Read(c, 0, c.Length);
            return BitConverter.ToSingle(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as long.
        /// </summary>
        /// <returns>The long.</returns>
        public Int64 GetLong()
        {
            var c = new Byte[8];
            _stream.Read(c, 0, c.Length);
            return BitConverter.ToInt64(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as string.
        /// </summary>
        /// <returns>The string.</returns>
        public String GetString()
        {
            var buffer = new Byte[4];
            _stream.Read(buffer, 0, buffer.Length);
            var length = BitConverter.ToInt32(buffer, 0);
            buffer = new Byte[length];
            _stream.Read(buffer, 0, buffer.Length);
            return Encoding.Unicode.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads the next bytes as scalar (2 doubles).
        /// </summary>
        /// <returns>The scalar.</returns>
        public ScalarValue GetScalar()
        {
            var c = new Byte[16];
            _stream.Read(c, 0, c.Length);
            var real = BitConverter.ToInt32(c, 0);
            var imag = BitConverter.ToDouble(c, 8);
            return new ScalarValue(real, imag);
        }

        /// <summary>
        /// Reads the next value.
        /// </summary>
        /// <returns>The deserialized value.</returns>
        public Value GetValue()
        {
            var header = GetString();
            var content = GetBytes();
            return Value.Deserialize(header, content);
        }

        /// <summary>
        /// Reads the next bytes as double.
        /// </summary>
        /// <returns>The double.</returns>
        public Double GetDouble()
        {
            var c = new Byte[8];
            _stream.Read(c, 0, c.Length);
            return BitConverter.ToDouble(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as integer.
        /// </summary>
        /// <returns>The integer.</returns>
        public Int32 GetInt()
        {
            var c = new Byte[4];
            _stream.Read(c, 0, c.Length);
            return BitConverter.ToInt32(c, 0);
        }

        /// <summary>
        /// Reads the next bytes as raw bytes.
        /// </summary>
        /// <returns>The binary array.</returns>
        public Byte[] GetBytes()
        {
            var length = GetInt();
            var bytes = new Byte[length];
            _stream.Read(bytes, 0, length);
            return bytes;
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Cleans up the mess.
        /// </summary>
        public void Dispose()
        {
            _stream.Dispose();
        }

        #endregion
    }
}
