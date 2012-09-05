using System;
using System.Collections;
using System.Reflection;

namespace YAMP
{
	abstract class ArgumentFunction : StandardFunction
	{
		protected Value[] arguments;
		Hashtable functions;
		
		public ArgumentFunction ()
		{
			functions = new Hashtable();
			var methods = this.GetType().GetMethods();
			
			foreach(var method in methods)
			{
				if(method.Name.Equals("Function"))
				{
					var args = method.GetParameters().Length;
					functions.Add(args, method);
				}
			}
		}
		
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				arguments = new Value[] { argument };
			else if(argument is MatrixValue)
			{
				var vec = argument as MatrixValue;
				
				if(vec.DimensionY > 1)
					throw new ArgumentException(name);
				
				arguments = new Value[vec.DimensionX];
				
				for(var i = 0; i < arguments.Length;)
					arguments[i] = vec[++i];
			}
			else
				throw new ArgumentException(name);
			
			return Execute();
		}
		
		Value Execute()
		{
			if(functions.ContainsKey(arguments.Length))
			{
				var method = (functions[arguments.Length] as MethodInfo);
				return method.Invoke(this, arguments) as Value;
			}
			
			throw new ArgumentsException(name, arguments.Length);
		}
	}
}

