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
using System.Linq;
using System.Reflection;

namespace YAMP.Converter
{
    /// <summary>
    /// Abstract base class for any value converter.
    /// </summary>
	public abstract class ValueConverterAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="expected">The expected type (target).</param>
        /// <param name="converter">The conversion function.</param>
		public ValueConverterAttribute(Type expected, Func<Value, object> converter)
		{
			Converter = converter;
			Expected = expected;
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="expected">The expected type (target)</param>
		public ValueConverterAttribute(Type expected)
		{
			Expected = expected;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the converter to use.
        /// </summary>
        public Func<Value, object> Converter
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the expected type.
        /// </summary>
        public Type Expected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the expected type (without the value convention).
        /// </summary>
        public string Type
        {
            get { return Expected.Name.RemoveValueConvention(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the given value to a standard CLR type.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <returns>The standard CLR type.</returns>
        public object Convert(Value argument)
		{
			return Converter.Invoke(argument);
		}

        /// <summary>
        /// Indicates if a given argument can be converted.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <returns>A boolean if this is possible.</returns>
		public bool CanConvertFrom(Value argument)
		{
			return Expected.IsInstanceOfType(argument);
        }

        #endregion
    }
}
