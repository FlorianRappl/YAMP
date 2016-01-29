using System;
using System.Runtime.Serialization;

namespace MathParserNet.Exceptions
{
    [Serializable]
    public class CouldNotParseExpressionException : Exception
    {
        public CouldNotParseExpressionException()
            : base(StringResources.Could_not_parse_this_expression)
        {

        }

        public CouldNotParseExpressionException(string message)
            : base(message)
        {

        }

        public CouldNotParseExpressionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected CouldNotParseExpressionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
