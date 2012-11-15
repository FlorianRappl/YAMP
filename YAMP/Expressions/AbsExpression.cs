using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YAMP
{
    class AbsExpression : TreeExpression
    {
        AbsFunction abs;

        public AbsExpression()
            : base(@"\|.*\|")
        {
            abs = new AbsFunction();
        }

        public AbsExpression(QueryContext query) : this()
        {
            Query = query;
        }

        public override Expression Create(QueryContext query, Match match)
        {
            return new AbsExpression(query);
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return abs.Perform(base.Interpret(symbols));
        }

        public override string Set(string input)
        {
            var brackets = 0;
            var sb = new StringBuilder();

            for (var i = 1; i < input.Length; i++)
            {
                if (input[i] == ')' || input[i] == ']' || input[i] == '}')
                    brackets--;
                else if (input[i] == '(' || input[i] == '[' || input[i] == '{')
                    brackets++;
                else if (brackets == 0 && input[i] == '|')
                {
                    _input = sb.ToString();
                    Tree = new ParseTree(Query, _input, Offset);
                    return input.Substring(i + 1);
                }

                sb.Append(input[i]);
            }

            throw new BracketException("|", input);
        }

        public override string ToString()
        {
            return "| | [ ExpressionType = Abs ]\n" + Tree.ToString();
        }
    }
}