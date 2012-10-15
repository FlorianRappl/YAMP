using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all argument functions. (provide all functions with the name function).
    /// </summary>
	public abstract class ArgumentFunction : StandardFunction
	{
		protected Value[] arguments;
		IDictionary<ParameterInfo[], MethodInfo> functions;
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
			functions = new Dictionary<ParameterInfo[], MethodInfo>();
			var methods = this.GetType().GetMethods();
			
			foreach(var method in methods)
			{
				if(method.Name.IsArgumentFunction())
					functions.Add(method.GetParameters(), method);
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
            var exception = new ArgumentsException(Name, arguments.Length); 

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
                    a[i - args + 2] = old[i];

                arguments[args - 1] = a;
            }

            foreach (var key in functions.Keys)
            {
                if (key.Length != args)
                    continue;

                var method = functions[key];

                if (SignatureFits(method, exception))
                {
                    try
                    {
                        return method.Invoke(this, arguments) as Value;
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException ?? ex;
                    }
                }
            }
            
			throw exception;
		}

        bool SignatureFits(MethodInfo method, Exception exception)
        {
            var pis = method.GetParameters();

            for (int i = 0; i < pis.Length; i++)
            {
                if (!pis[i].ParameterType.IsInstanceOfType(arguments[i]))
                {
                    exception = new ArgumentTypeNotSupportedException(Name, i, pis[i].ParameterType);
                    return false;
                }
            }

            return true;
        }
	}
}

