using System;

namespace YAMP
{
    public class ArgumentTypeNotSupportedException : Exception
    {
        public ArgumentTypeNotSupportedException(string function) : base("The function " + function + "() is not supported with these arguments.")
        {
        }

        public ArgumentTypeNotSupportedException(string function, int index) : base("The function " + function + "() is not supported with argument #" + (index + 1) + ".")
        {
        }

        public ArgumentTypeNotSupportedException(string function, int index, Type type) : base("The function " + function + "() is not supported with argument #" + (index + 1) + ". The argument should be of type " + type.Name.RemoveValueConvention() + ".")
        {
        }
    }
}
