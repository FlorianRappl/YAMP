using System;

namespace YAMP
{
    public class YAMPException : Exception
    {
        public string Symbol { get; protected set; }

        public string Near { get; protected set; }

        public int? AtArgument { get; protected set; }

        public int? AtIndex { get; protected set; }

        public YAMPException(string message) : base(message)
        {
        }

        public YAMPException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}
