using System;

namespace YAMP
{
    abstract class SystemFunction : ArgumentFunction
    {
        public SystemFunction() : base()
        {
            Context = ParseContext.Default;
        }

        public ParseContext Context { get; set; }

        public override Value Perform(ParseContext context, Value argument)
        {
            var function = GetType().GetConstructor(new Type[0]).Invoke(null) as SystemFunction;
            function.Context = context;
            return function.Perform(argument);
        }
    }
}
