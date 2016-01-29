using System;
using System.Collections.Generic;

namespace YAMP
{
    [Description("Evaluates some user input by executing the given code.")]
    [Kind(PopularKinds.System)]
    sealed class EvalFunction : SystemFunction
    {
        [Description("Evaluates the given string as code and returns the value.")]
        [Example("eval(\"2+3\")", "Evaluates the input and returns the value 5.")]
        [Example("eval(\"17*5\")", "Evaluates the query and returns the result of 17 * 5 = 85.")]
        [Example("eval(\"[1 2 3]\")", "Creates a new vector with the entries 1, 2 and 3.")]
        public Value Function(StringValue code)
        {
            var q = new QueryContext(code.Value);
            q.Context = new ParseContext();
            var p = new ParseEngine(q).Parse();

            if (p.CanRun)
            {
                foreach (var statement in p.Statements)
                    return statement.Interpret(new Dictionary<string, Value>());
            }

            return new StringValue();
        }
    }
}
