namespace YAMP
{
    using System;
    using System.Collections.Generic;

    [Description("EvalFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class EvalFunction : SystemFunction
    {
        public EvalFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("EvalFunctionDescriptionForString")]
        [Example("eval(\"2+3\")", "EvalFunctionExampleForString1")]
        [Example("eval(\"17*5\")", "EvalFunctionExampleForString2")]
        [Example("eval(\"[1 2 3]\")", "EvalFunctionExampleForString3")]
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
