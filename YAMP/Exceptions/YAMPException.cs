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

namespace YAMP
{
    /// <summary>
    /// Basic YAMP exception. This lets everyone know that the exception
    /// did not occur because something was fishy in the (C#) code, but
    /// rather in the query.
    /// </summary>
    public class YAMPException : Exception
    {
        /// <summary>
        /// Creates an anonymous YAMP exception.
        /// </summary>
        public YAMPException()
            : this("An unknown exception occured by executing your query.")
        {
        }

        /// <summary>
        /// Creates a YAMP exception with a simple message.
        /// </summary>
        /// <param name="message">Which message do you want to display?</param>
        public YAMPException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a YAMP exception with a formatted message.
        /// </summary>
        /// <param name="message">The associated message.</param>
        /// <param name="args">Some parameters for your message.</param>
        public YAMPException(string message, params object[] args) 
            : base(string.Format(message, args))
        {
        }
    }
}
