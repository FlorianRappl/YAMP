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
    /// Any error during parsing will be noted as an instance of this class.
    /// </summary>
    public abstract class YAMPParseError
    {
        #region ctor

        /// <summary>
        /// Creates a new parse error.
        /// </summary>
        /// <param name="line">The line of the error.</param>
        /// <param name="column">The column of the error.</param>
        /// <param name="message">The message for the error.</param>
        /// <param name="args">The arguments for formatting the message.</param>
        public YAMPParseError(int line, int column, string message, params object[] args)
        {
            Line = line;
            Column = column;
            Message = string.Format(message, args);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message for this error.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the line for this error.
        /// </summary>
        public int Line
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the column for this error.
        /// </summary>
        public int Column
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the block responsible for the parse error.
        /// </summary>
        public Block Part
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the length of the error in characters.
        /// </summary>
        public int Length
        {
            get
            {
                if (Part != null)
                    return Part.Length;

                return 1;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts to error to a string.
        /// </summary>
        /// <returns>The string with the error.</returns>
        public override string ToString()
        {
            return string.Format("Line {0:000}, Pos. {1:000} : {2}", Line, Column, Message);
        }

        #endregion
    }
}
