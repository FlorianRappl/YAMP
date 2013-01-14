/*
	Copyright (c) 2012-2013, Florian Rappl.
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
using System.Reflection;

namespace YAMP
{
    /// <summary>
    /// Container for conserving information about parameters.
    /// </summary>
	public class FunctionParameters
    {
        #region ctor

        public FunctionParameters(ParameterInfo[] parameterInfo, MethodInfo methodInfo)
		{
			Length = parameterInfo.Length;
			ParameterTypes = new Type[Length];

			for (var i = 0; i < Length; i++)
				ParameterTypes[i] = parameterInfo[i].ParameterType;

			OptionalArguments = methodInfo.GetCustomAttributes(typeof(ArgumentsAttribute), false) as ArgumentsAttribute[];
		}

        #endregion

        #region Properties

        public ArgumentsAttribute[] OptionalArguments { get; private set; }

		public Type[] ParameterTypes { get; private set; }

        public int Length { get; private set; }

		public int Weight
		{
			get
			{
				int sum = 0;

				foreach (Type t in ParameterTypes)
				{
					for (var i = 0; i < t.Name.Length; i++)
						sum += (int)t.Name[i];
				}

				return sum;
			}
        }

		public int MinimumArguments
		{
			get
			{
				var arg = Length;

				foreach (var opt in OptionalArguments)
				{
					arg--;
					arg += opt.MinimumArguments;
				}

				return arg;
			}
		}

		public int MaximumArguments
		{
			get
			{
				var arg = Length;

				foreach (var opt in OptionalArguments)
				{
					if (opt.MaximumArguments == int.MaxValue)
						return int.MaxValue;

					arg--;
					arg += opt.MaximumArguments;
				}

				return arg;
			}
		}

        #endregion
    }
}
