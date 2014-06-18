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
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// Represents the exception that will be thrown if parse errors occured.
    /// </summary>
    public class YAMPParseException : YAMPException
    {
        /// <summary>
        /// Creates a new YAMP Parse Exception.
        /// </summary>
        /// <param name="engine">The engine where this happend.</param>
        public YAMPParseException(ParseEngine engine) 
            : base("The query can not run, since the parser encountered {0} error(s).", engine.ErrorCount)
        {
            Errors = engine.Errors;
        }

        /// <summary>
        /// Returns an enumerable of errors.
        /// </summary>
        public IEnumerable<YAMPParseError> Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a string representation of the errors.
        /// </summary>
        /// <returns>The string with the errors.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var error in Errors)
                sb.AppendLine(error.ToString());
            
            return sb.ToString();
        }
    }
}
