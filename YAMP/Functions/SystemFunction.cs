using System;

namespace YAMP
{
    abstract class SystemFunction : ArgumentFunction
    {
        public SystemFunction() : base()
        {
            Context = ParseContext.Default;
        }

        public SystemFunction(int optArg) : base(optArg)
        {
            Context = ParseContext.Default;
        }

        public ParseContext Context { get; set; }

        public override Value Perform(ParseContext context, Value argument)
        {
            var function = GetType().GetConstructor(Type.EmptyTypes).Invoke(null) as SystemFunction;
            function.Context = context;
            return function.Perform(argument);
        }
    }
}
