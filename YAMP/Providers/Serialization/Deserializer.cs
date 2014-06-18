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
