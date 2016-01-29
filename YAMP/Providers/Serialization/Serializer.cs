using System;
using System.IO;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// Helper class for the serialization process.
    /// </summary>
    class Serializer : IDisposable
    {
        #region Fields

        MemoryStream stream;

        #endregion

        #region ctor

        private Serializer()
        {
            stream = new MemoryStream();
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
        public byte[] Value
        {
            get
            {
                return stream.ToArray();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serializes a string value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var length = BitConverter.GetBytes(bytes.Length);
            stream.Write(length, 0, length.Length);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a bool value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(bool value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes an integer value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a double value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a float value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a long value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            return this;
        }

        /// <summary>
        /// Serializes a scalar (2 doubles) value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(ScalarValue value)
        {
            var real = BitConverter.GetBytes(value.Re);
            stream.Write(real, 0, real.Length);
            var imag = BitConverter.GetBytes(value.Im);
            stream.Write(imag, 0, imag.Length);
            return this;
        }

        /// <summary>
        /// Serializes a raw byte array value.
        /// </summary>
        /// <param name="content">The value to serialize</param>
        /// <returns>The current instance.</returns>
        public Serializer Serialize(byte[] content)
        {
            Serialize(content.Length);
            stream.Write(content, 0, content.Length);
            return this;
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
