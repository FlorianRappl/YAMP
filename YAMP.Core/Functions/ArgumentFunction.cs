namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using YAMP.Exceptions;

    /// <summary>
    /// The abstract base class used for all argument functions. (provide all functions with the name function).
    /// </summary>
	public abstract class ArgumentFunction : BaseFunction, IComparer<FunctionParameters>
	{
		#region Fields

        readonly KeyValuePair<FunctionParameters, MethodInfo>[] _functions;
		Value[] _arguments;

		#endregion

		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public ArgumentFunction ()
		{
		    _functions = (from method in GetType().GetMethods()
		                 where method.Name.IsArgumentFunction()
		                 select new KeyValuePair<FunctionParameters, MethodInfo>(new FunctionParameters(method.GetParameters(), method), method))
		        .OrderBy(kv => kv.Key, this).ToArray();
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets the number of given arguments.
        /// </summary>
		public Int32 Length
		{
			get { return _arguments.Length; }
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
		public Boolean CanExecute(Int32 args)
		{
			foreach (var kv in _functions)
			{
				var key = kv.Key;

                if (args >= key.MinimumArguments && args <= key.MaximumArguments)
                {
                    return true;
                }
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
            if (argument is ArgumentsValue)
            {
                _arguments = ((ArgumentsValue)argument).Values;
            }
            else
            {
                _arguments = new Value[] { argument };
            }
			
			return Execute();
		}

        #endregion

        #region Helpers

        Value Execute()
		{
            var args = _arguments.Length;
            var difference = int.MaxValue;
            var expected = 0;
            YAMPRuntimeException exception = null;

			foreach(var kv in _functions)
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
                        return f.Invoke(this, _arguments) as Value;
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

			for (var i = 0; i < _arguments.Length; i++)
			{
				var opt = false;

				for (var j = 0; j < attrs.Length; j++)
				{
					var attr = attrs[j];

					if (attr.Index == i)
					{
						var rest = _arguments.Length - i;

						if (rest >= attr.MinimumArguments)
						{
							var av = new ArgumentsValue();
							var pt = Math.Min(attr.MaximumArguments / attr.StepArguments, rest / attr.StepArguments);

							for (var k = 0; k < pt; k++)
							{
								for (var l = 0; l < attr.StepArguments; l++)
									av.Insert(_arguments[i++]);
							}

							values.Add(av);
							opt = true;
						}
					}
				}

				if (!opt)
				{
					var idx = values.Count;

					if (!yp.ParameterTypes[idx].IsInstanceOfType(_arguments[idx]))
                        return new YAMPArgumentInvalidException(Name, _arguments[idx].Header, yp.ParameterTypes[idx].Name.RemoveValueConvention(), idx);

					values.Add(_arguments[i]);
				}
			}

			while (values.Count < yp.ParameterTypes.Length)
				values.Add(new ArgumentsValue());

			_arguments = values.ToArray();
			return null;
		}

		#endregion
	}
}

