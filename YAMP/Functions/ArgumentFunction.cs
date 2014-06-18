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
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all argument functions. (provide all functions with the name function).
    /// </summary>
	public abstract class ArgumentFunction : BaseFunction, IComparer<FunctionParameters>
	{
		#region Members

		Value[] arguments;
		readonly KeyValuePair<FunctionParameters, MethodInfo>[] functions;

		#endregion

		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public ArgumentFunction ()
		{
		    functions = (from method in GetType().GetMethods()
		                 where method.Name.IsArgumentFunction()
		                 select new KeyValuePair<FunctionParameters, MethodInfo>(new FunctionParameters(method.GetParameters(), method), method))
		        .OrderBy(kv => kv.Key, this).ToArray();
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets the number of given arguments.
        /// </summary>
		public int Length
		{
			get
			{
				return arguments.Length;
			}
		}

		#endregion

		#region Methods

        /// <summary>
        /// Compares to FunctionParameters to find out the equality factor.
        /// </summary>
        /// <param name="x">The source to compare with.</param>
        /// <param name="y">The target to compare by.</param>
        /// <returns>The computed equality factor, which is 0 if both paramter spaces are equal.</returns>
		public int Compare(FunctionParameters x, FunctionParameters y)
		{
			return 100 * (y.Length - x.Length) + Math.Sign(y.Weight - x.Weight);
		}

        /// <summary>
        /// Computes a boolean if the function can be executed with the number of parameters.
        /// </summary>
        /// <param name="args">The number of parameters independent of the specific types.</param>
        /// <returns>A boolean indicating the status.</returns>
		public bool CanExecute(int args)
		{
			foreach (var kv in functions)
			{
				var key = kv.Key;

				if (args < key.MinimumArguments || args > key.MaximumArguments)
					continue;

				return true;
			}

			return false;
		}
		
        /// <summary>
        /// Performs the function execution.
        /// </summary>
        /// <param name="argument">The argument(s) (if presented in an "ArgumentValue").</param>
        /// <returns>The evaluated value.</returns>
		public override Value Perform (Value argument)
		{
			if(argument is ArgumentsValue)
				arguments = (argument as ArgumentsValue).Values;
			else
				arguments = new Value[] { argument };
			
			return Execute();
		}

        #endregion

        #region Helpers

        Value Execute()
		{
            var args = arguments.Length;
            var difference = int.MaxValue;
            var expected = 0;
            YAMPRuntimeException exception = null;

			foreach(var kv in functions)
			{
			    var key = kv.Key;

                if (args < key.MinimumArguments || args > key.MaximumArguments)
                {
                    var diff = Math.Min(Math.Abs(key.MinimumArguments - args), Math.Abs(args - key.MaximumArguments));

                    if (diff < difference)
                    {
                        difference = diff;
                        expected = args < key.MinimumArguments ? key.MinimumArguments : key.MaximumArguments;
                    }

                    continue;
                }

				var f = kv.Value;
                exception = BuildArguments(key);

                if (exception == null)
                {
                    try
                    {
                        return f.Invoke(this, arguments) as Value;
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            throw ex.InnerException;

                        throw;
                    }
                }
            }

            if (exception != null)
                throw exception;
            
			throw new YAMPArgumentNumberException(Name, args, expected);
		}

		YAMPRuntimeException BuildArguments(FunctionParameters yp)
		{
			var attrs = yp.OptionalArguments;
			var values = new List<Value>();

			for (var i = 0; i < arguments.Length; i++)
			{
				var opt = false;

				for (var j = 0; j < attrs.Length; j++)
				{
					var attr = attrs[j];

					if (attr.Index == i)
					{
						var rest = arguments.Length - i;

						if (rest >= attr.MinimumArguments)
						{
							var av = new ArgumentsValue();
							var pt = Math.Min(attr.MaximumArguments / attr.StepArguments, rest / attr.StepArguments);

							for (var k = 0; k < pt; k++)
							{
								for (var l = 0; l < attr.StepArguments; l++)
									av.Insert(arguments[i++]);
							}

							values.Add(av);
							opt = true;
						}
					}
				}

				if (!opt)
				{
					var idx = values.Count;

					if (!yp.ParameterTypes[idx].IsInstanceOfType(arguments[idx]))
                        return new YAMPArgumentInvalidException(Name, arguments[idx].Header, yp.ParameterTypes[idx].Name.RemoveValueConvention(), idx);

					values.Add(arguments[i]);
				}
			}

			while (values.Count < yp.ParameterTypes.Length)
				values.Add(new ArgumentsValue());

			arguments = values.ToArray();
			return null;
		}

		#endregion
	}
}

