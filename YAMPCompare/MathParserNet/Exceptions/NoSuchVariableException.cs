using System;
using System.Runtime.Serialization;

namespace MathParserNet.Exceptions
{
    [Serializable]
    public class NoSuchVariableException : Exception
    {
        public NoSuchVariableException()
            : base(StringResources.Undefined_Variable)
        {

        }

        public NoSuchVariableException(string message)
            : base(message)
        {

        }

        public NoSuchVariableException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected NoSuchVariableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
