namespace YAMP
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Container for conserving information about parameters.
    /// </summary>
	public class FunctionParameters
    {
        #region ctor

        /// <summary>
        /// Creates a new instance of the function parameter holder.
        /// </summary>
        /// <param name="parameterInfo">The array of parameter infos.</param>
        /// <param name="methodInfo">The method info (from reflection).</param>
        public FunctionParameters(ParameterInfo[] parameterInfo, MethodInfo methodInfo)
		{
            int sum = 0;
            int minArg = parameterInfo.Length;
            int maxArg = parameterInfo.Length;
            var takeMax = false;

			Length = parameterInfo.Length;
			ParameterTypes = new Type[Length];

            for (var i = 0; i < Length; i++)
            {
                var t = parameterInfo[i].ParameterType;

				for (var j = 0; j < t.Name.Length; j++)
					sum += (int)t.Name[j];

                ParameterTypes[i] = t;
            }

            Weight = sum;
            OptionalArguments = methodInfo.GetCustomAttributes(typeof(ArgumentsAttribute), false) as ArgumentsAttribute[];

            foreach (var opt in OptionalArguments)
            {
                if (opt.MaximumArguments == int.MaxValue)
                    takeMax = true;
                else
                {
                    maxArg += opt.MaximumArguments;
                    maxArg--;
                }

                minArg--;
                minArg += opt.MinimumArguments;
            }

            MaximumArguments = takeMax ? int.MaxValue : maxArg;
            MinimumArguments = minArg;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the computed optional arguments.
        /// </summary>
        public ArgumentsAttribute[] OptionalArguments
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the specified parameter types.
        /// </summary>
		public Type[] ParameterTypes 
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the length of the arguments.
        /// </summary>
        public Int32 Length 
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets the computed weight of the arguments.
        /// </summary>
		public Int32 Weight
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the minimum number of arguments for this function call.
        /// </summary>
		public Int32 MinimumArguments
        {
            get;
            private set; 
		}

        /// <summary>
        /// Gets the maximum number of arguments for this function call.
        /// </summary>
		public Int32 MaximumArguments
        {
            get;
            private set; 
		}

        #endregion
    }
}
