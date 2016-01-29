using System;
using System.Runtime.Serialization;

namespace MathParserNet.Exceptions
{
    [Serializable]
    public class NoSuchFunctionException : Exception
    {
        public NoSuchFunctionException()
            : base(StringResources.No_such_function_defined)
        {

        }

        public NoSuchFunctionException(string message)
            : base(message)
        {

        }

        public NoSuchFunctionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected NoSuchFunctionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
