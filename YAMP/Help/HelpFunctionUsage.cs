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

namespace YAMP.Help
{
    /// <summary>
    /// Represents one usage of the described function.
    /// </summary>
	public class HelpFunctionUsage
	{
        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public HelpFunctionUsage()
		{
			Examples = new List<HelpExample>();
			Arguments = new List<string>();
            ArgumentNames = new List<string>();
            Returns = new List<string>();
		}

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
		public string Usage { get; set; }

        /// <summary>
        /// Gets or sets a description about the usage.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a list of names for the given arguments of this function usage.
        /// </summary>
        public List<string> ArgumentNames { get; private set; }

        /// <summary>
        /// Gets a list of arguments for this function usage.
        /// </summary>
		public List<string> Arguments { get; private set; }

        /// <summary>
        /// Gets a list of available return values of this function usage.
        /// </summary>
        public List<string> Returns { get; set; }

        /// <summary>
        /// Gets a list of examples corresponding to this function usage.
        /// </summary>
		public List<HelpExample> Examples { get; private set; }
	}
}
