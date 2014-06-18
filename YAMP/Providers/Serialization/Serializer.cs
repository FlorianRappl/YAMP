/*
	Copyright (c) 2012-2014, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
        #region Members

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
