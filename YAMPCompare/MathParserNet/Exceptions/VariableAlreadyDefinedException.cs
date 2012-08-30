using System;
using System.Runtime.Serialization;

namespace MathParserNet.Exceptions
{
    [Serializable]
    public class VariableAlreadyDefinedException : Exception
    {
        public VariableAlreadyDefinedException()
            : base(StringResources.Variable_already_defined)
        {

        }

        public VariableAlreadyDefinedException(string message)
            : base(message)
        {

        }

        public VariableAlreadyDefinedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected VariableAlreadyDefinedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
