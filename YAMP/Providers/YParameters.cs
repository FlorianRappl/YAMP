using System;
using System.Reflection;

namespace YAMP
{
	public class YParameters
	{
		public YParameters(ParameterInfo[] parameterInfo, MethodInfo methodInfo)
		{
			Length = parameterInfo.Length;
			ParameterTypes = new Type[Length];

			for (var i = 0; i < Length; i++)
				ParameterTypes[i] = parameterInfo[i].ParameterType;

			OptionalArguments = methodInfo.GetCustomAttributes(typeof(ArgumentsAttribute), false) as ArgumentsAttribute[];
		}

		public int Length { get; private set; }

		public ArgumentsAttribute[] OptionalArguments { get; private set; }

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

		public Type[] ParameterTypes { get; private set; }

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
	}
}
