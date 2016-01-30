namespace YAMP
{
    using System;
    using System.Collections.Generic;

    [Description("Evaluates some user input by executing the given code.")]
    [Kind(PopularKinds.System)]
    sealed class EvalFunction : SystemFunction
    {
        public EvalFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("Evaluates the given string as code and returns the value.")]
        [Example("eval(\"2+3\")", "Evaluates the input and returns the value 5.")]
        [Example("eval(\"17*5\")", "Evaluates the query and returns the result of 17 * 5 = 85.")]
        [Example("eval(\"[1 2 3]\")", "Creates a new vector with the entries 1, 2 and 3.")]
        public Value Function(StringValue code)
        {
            var c = new ParseContext(Context);
            var q = new QueryContext(c, code.Value);
            var p = new ParseEngine(q, c).Parse();

            if (p.CanRun)
            {
                foreach (var statement in p.Statements)
                {
                    return statement.Interpret(new Dictionary<String, Value>());
                }
            }

            return new StringValue();
        }
    }
}
