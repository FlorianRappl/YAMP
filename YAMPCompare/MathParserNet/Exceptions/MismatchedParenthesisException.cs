using System;
using System.Runtime.Serialization;

namespace MathParserNet.Exceptions
{
    [Serializable]
    public class MismatchedParenthesisException : Exception
    {
        public MismatchedParenthesisException() : base(StringResources.Mismatched_Parenthesis)
        {
            
        }

        public MismatchedParenthesisException(string message)
            : base(message)
        {

        }

        public MismatchedParenthesisException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected MismatchedParenthesisException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
