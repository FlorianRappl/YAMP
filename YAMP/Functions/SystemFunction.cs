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
    /// SystemFunction is a special kind of ArgumentFunction, which saves the
    /// passed ParseContext in a variable, which can be accessed over the 
    /// property Context.
    /// </summary>
    public abstract class SystemFunction : ArgumentFunction
	{
		#region ctor

        /// <summary>
        /// Creates a new system function with the default context.
        /// </summary>
		public SystemFunction()
        {
            Context = ParseContext.Default;
		}

        /// <summary>
        /// Creates a new system function with a specific context.
        /// </summary>
        /// <param name="context">The given context.</param>
        public SystemFunction(ParseContext context)
        {
            Context = context;
        }

		#endregion

		#region Properties

        /// <summary>
        /// Gets or sets the associated context.
        /// </summary>
		public ParseContext Context { get; set; }

		#endregion

		#region Methods

        /// <summary>
        /// Performs the function in the given context.
        /// </summary>
        /// <param name="context">The context where the function is executed.</param>
        /// <param name="argument">The argument of the function.</param>
        /// <returns>The evaluted value.</returns>
		public override Value Perform(ParseContext context, Value argument)
        {
			var function = GetType().GetConstructor(Value.EmptyTypes).Invoke(null) as SystemFunction;
            function.Context = context;
            return function.Perform(argument);
		}

		#endregion
    }
}
