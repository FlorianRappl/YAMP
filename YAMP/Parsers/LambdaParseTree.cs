using System;

namespace YAMP
{
    class LambdaParseTree : ParseTree
    {
        private string p;

        public LambdaParseTree(QueryContext context, string input, int offset)
            : base(context, input, offset)
        {
        }

        protected override Operator FindOperator(string input)
        {
            var op = Tokens.FindAvailableOperator(Query, input);

            if (op != null)
                return op;

            return new TerminateOperator(Query);
        }
    }
}
