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
	/// Provides a returns attribute to be read by the help method.
	/// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
	public class ReturnsAttribute : Attribute
    {
        #region ctor

        /// <summary>
		/// Creates a new attribute for storing explanations for return values
		/// (should be used in combination with multiple output arguments).
        /// </summary>
        /// <param name="type">The type that will be returned</param>
        /// <param name="explanation">The specific explanations</param>
        /// <param name="order">The rank of the return type</param>
		public ReturnsAttribute(Type type, string explanation, int order = 0)
		{
            ReturnType = type;
			Explanation = explanation;
            Order = order;
		}

        #endregion

        #region Properties

        /// <summary>
		/// Gets the specified explanations for this return type.
		/// </summary>
		public string Explanation
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the type that will be returned.
        /// </summary>
        public Type ReturnType 
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the rank of the return attribute.
        /// </summary>
        public int Order 
        { 
            get; 
            private set;
        }

        #endregion
    }
}
