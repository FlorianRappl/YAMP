using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all argument functions. (provide all functions with the name function).
    /// </summary>
	abstract class ArgumentFunction : StandardFunction
	{
		protected Value[] arguments;
		IDictionary<int, MethodInfo> functions;
        int _optArg;
        bool _hasOpt;

        public ArgumentFunction() : this(0, false)
        {
        }

        public ArgumentFunction(int optArg) : this(optArg, true)
        {
        }
		
		ArgumentFunction (int optArg, bool hasArg)
		{
            _hasOpt = hasArg;
            _optArg = optArg;
			functions = new Dictionary<int, MethodInfo>();
			var methods = this.GetType().GetMethods();
			
			foreach(var method in methods)
			{
				if(method.Name.IsArgumentFunction())
				{
					var args = method.GetParameters().Length;
					functions.Add(args, method);
				}
			}
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

            if (_hasOpt && args >= _optArg)
            {
                var i = 0;
                var length = arguments.Length;
                var old = arguments;
                var a = new ArgumentsValue();
                arguments = new Value[_optArg];
                args = _optArg;

                for (; i < args - 1; i++)
                    arguments[i] = old[i];

                for (; i < length; i++)
                    a[i - args + 1] = old[i];

                arguments[args - 1] = a;
            }
            
            if(functions.ContainsKey(args))
			{
				var method = functions[args];

                try
                {
                    var pis = method.GetParameters();

                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (!pis[i].ParameterType.IsInstanceOfType(arguments[i]))
                            throw new ArgumentTypeNotSupportedException(name, i, pis[i].ParameterType);
                    }

                    return method.Invoke(this, arguments) as Value;
                }
                catch (Exception ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }
			
			throw new ArgumentsException(name, arguments.Length);
		}
	}
}

