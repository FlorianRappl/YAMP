using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    /// <summary>
    /// Math parser exception class
    /// </summary>
    [Serializable()]
    public class MathParserException : Exception
    {
        public MathParserException()
            : base()
        { }
        public MathParserException(string message)
            : base(message)
        { }
        public MathParserException(string message, Exception innerException)
            : base(message, innerException)
        { }
        protected MathParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
