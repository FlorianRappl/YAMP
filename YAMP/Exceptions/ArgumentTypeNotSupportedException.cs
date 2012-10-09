using System;

namespace YAMP
{
    public class ArgumentTypeNotSupportedException : YAMPException
    {
        public ArgumentTypeNotSupportedException(string function)
            : base("The function {0}() is not supported with these arguments.", function)
        {
            Symbol = function;
        }

        public ArgumentTypeNotSupportedException(string function, int index)
            : base("The function {0}() is not supported with argument #{1}.", function, index + 1)
        {
            Symbol = function;
            AtArgument = index;
        }

        public ArgumentTypeNotSupportedException(string function, int index, Type type)
            : base("The function {0}() is not supported with argument #{1}. The argument should be of type {2}.", function, index + 1, type.Name.RemoveValueConvention())
        {
            Symbol = function;
            AtArgument = index;
        }
    }
}
