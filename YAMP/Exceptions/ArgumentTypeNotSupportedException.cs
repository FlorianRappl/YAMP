using System;

namespace YAMP
{
    public class ArgumentTypeNotSupportedException : Exception
    {
        public ArgumentTypeNotSupportedException(string function) : base("The function " + function + "() is not supported with these arguments.")
        {
        }
    }
}
