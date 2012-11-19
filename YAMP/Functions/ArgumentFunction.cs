using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all argument functions. (provide all functions with the name function).
    /// </summary>
	public abstract class ArgumentFunction : StandardFunction, IComparer<YParameters>
	{
		#region Members

		Value[] arguments;
		readonly KeyValuePair<YParameters, MethodInfo>[] functions;

		#endregion

		#region ctor

		public ArgumentFunction ()
		{
		    functions = (from method in GetType().GetMethods()
		                 where method.Name.IsArgumentFunction()
		                 select new KeyValuePair<YParameters, MethodInfo>(new YParameters(method.GetParameters(), method), method))
		        .OrderBy(kv => kv.Key, this).ToArray();
		}

		#endregion

		#region Properties

		public int Length
		{
			get
			{
				return arguments.Length;
			}
		}

		#endregion

		#region Methods

		public int Compare(YParameters x, YParameters y)
		{
			return 100 * (y.Length - x.Length) + Math.Sign(y.Weight - x.Weight);
		}

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
		
		public override Value Perform (Value argument)
		{
			if(argument is ArgumentsValue)
				arguments = (argument as ArgumentsValue).Values;
			else
				arguments = new Value[] { argument };
			
			return Execute();
		}
		
		Value Execute()
		{
            var args = arguments.Length;
            var exception = new ArgumentsException(Name, arguments.Length);

			foreach(var kv in functions)
			{
			    var key = kv.Key;

				if (args < key.MinimumArguments || args > key.MaximumArguments)
					continue;

				var f = kv.Value;

				if(BuildArguments(key, exception))
				{
					try
					{
						return f.Invoke(this, arguments) as Value;
					}
					catch (Exception ex)
					{
						throw ex.InnerException ?? ex;
					}
				}
			}
            
			throw exception;
		}

		bool BuildArguments(YParameters yp, Exception exception)
		{
			var attrs = yp.OptionalArguments;
			var success = true;
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
								{
									av.Insert(arguments[i++]);
								}
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
					{
						exception = new ArgumentTypeNotSupportedException(Name, idx, yp.ParameterTypes[idx]);
						success = false;
						break;
					}

					values.Add(arguments[i]);
				}
			}

			if (success)
			{
				while (values.Count < yp.ParameterTypes.Length)
					values.Add(new ArgumentsValue());

				arguments = values.ToArray();
			}

			return success;
		}

		#endregion
	}
}

