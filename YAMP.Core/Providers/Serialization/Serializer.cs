namespace YAMP
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Helper class for the serialization process.
    /// </summary>
    sealed class Serializer : IDisposable
    {
        #region Fields

        readonly MemoryStream _stream;

        #endregion

        #region ctor

        private Serializer()
        {
            _stream = new MemoryStream();
        }

        /// <summary>
        /// Creates a new instance of the binary serializer helper.
        /// </summary>
        /// <returns>The new instance.</returns>
        public static Serializer Create()
        {
            return new Serializer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The binary value of the serialization.
        /// </summary>
        public Byte[] Value
        {
            get { return _stream.ToArray(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serializes a string value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(String value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var length = BitConverter.GetBytes(bytes.Length);
            _stream.Write(length, 0, length.Length);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a bool value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Boolean value)
        {
            var bytes = BitConverter.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes an integer value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Int32 value)
        {
            var bytes = BitConverter.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a double value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Double value)
        {
            var bytes = BitConverter.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a float value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Single value)
        {
            var bytes = BitConverter.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a long value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Int64 value)
        {
            var bytes = BitConverter.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes an arbitrary value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Value value)
        {
            var name = value.Header;
            var content = value.Serialize();
            Serialize(name);
            return Serialize(content);
        }

        /// <summary>
        /// Serializes a scalar (2 doubles) value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(ScalarValue value)
        {
            var real = BitConverter.GetBytes(value.Re);
            _stream.Write(real, 0, real.Length);
            var imag = BitConverter.GetBytes(value.Im);
            _stream.Write(imag, 0, imag.Length);
            return this;
        }

        /// <summary>
        /// Serializes a raw byte array value.
        /// </summary>
        /// <param name="content">The value to serialize.</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(Byte[] content)
        {
            Serialize(content.Length);
            _stream.Write(content, 0, content.Length);
            return this;
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
