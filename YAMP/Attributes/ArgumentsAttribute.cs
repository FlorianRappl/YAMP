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
	/// The attribute to store information about optional arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ArgumentsAttribute : Attribute
    {
        #region ctor

        /// <summary>
		/// Creates a new attribute to declare a container for optional arguments.
		/// </summary>
		/// <param name="index">The index that stores optional arguments.</param>
		/// <param name="min">The minimum number of arguments that need to be specified.</param>
		/// <param name="max">The maximum number of arguments that will be delegated to this container.</param>
		/// <param name="delta">The chunks of arguments to include, i.e. 2 is always an even number of arguments.</param>
		public ArgumentsAttribute(int index, int min = 1, int max = int.MaxValue, int delta = 1)
        {
			MinimumArguments = min < 0 ? 0 : min;
			MaximumArguments = max < MinimumArguments ? MinimumArguments : max;
			Index = index;
			StepArguments = delta > 0 ? delta : 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the minimum number of arguments that need to be provided for the specified parameter.
        /// </summary>
		public int MinimumArguments { get; private set; }

		/// <summary>
		/// Gets the maximum number of arguments that can be provided for the specified parameter.
		/// </summary>
		public int MaximumArguments { get; private set; }

		/// <summary>
		/// Gets the number of arguments that need to be provided starting at MinimumArguments, i.e.
		/// if delta = 2 and min = 0 then either 0, 2, 4, ... arguments can be specified.
		/// </summary>
		public int StepArguments { get; private set; }

		/// <summary>
		/// Gets the index of the parameter that can contain optional arguments.
		/// </summary>
		public int Index { get; private set; }

        #endregion
    }
}
