using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YAMP
{
    class LambdaExpression : Expression
    {
        #region Members

        string arguments;
        LambdaParseTree body;
        List<string> args;

        static readonly Regex symbolExpression = new Regex(SymbolExpression.SymbolRegularExpression);

        #endregion

        #region ctor

        public LambdaExpression() : base("@(.*)=>(.*)")
        {
        }

        public LambdaExpression(QueryContext query) : this()
        {
            args = new List<string>();
            Query = query;
        }

        #endregion

        #region Properties

        public int Arguments
        {
            get { return args.Count; }
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return new FunctionValue(args.ToArray(), body);
        }

        public override Expression Create(QueryContext query, Match match)
        {
            return new LambdaExpression(query);
        }

        public override string Set(string input)
        {
            _input = input;

            if (input.Length < 5)
                throw new ParseException(Offset, input);

            input = ParseFront(input.Substring(1));
            input = ParseBody(input);

			if (body.LastToken == '\0')
				return string.Empty;

            return body.LastToken + input;
        }

        string ParseBody(string input)
        {
            var child = new QueryContext(Query);
            body = new LambdaParseTree(Query, input, Offset + 3 + arguments.Length);
            return input.Substring(body.Length);
        }

        string ParseFront(string p)
        {
            var index = p.Length;
            var openBracket = 0;
            var commaRequired = false;

            for (var i = 0; i < p.Length - 1; i++)
            {
                switch (p[i])
                {
                    case ' ':
                        //Ignore
                        break;

                    case'(':
                        openBracket++;
                        break;

                    case ')':
                        openBracket--;
                        break;

                    case '=':
                        if (openBracket != 0)
                            throw new ParseException(i + 1 + Offset, p.Substring(i));
                        else if(p[i + 1] == '>')
                        {
                            index = i + 2;
                            i = p.Length;
                        }
                        else
                            throw new ParseException(i + 2 + Offset, p.Substring(i + 1));

                        break;

                    case ',':
                        if (commaRequired)
                            commaRequired = false;
                        else
                            throw new ParseException(i + 1 + Offset, p.Substring(i));

                        break;

                    default:
                        var rest = p.Substring(i);

                        if (commaRequired)
                            throw new ParseException(i + 1 + Offset, rest);

                        var match = symbolExpression.Match(rest);

                        if (match.Success)
                        {
                            commaRequired = true;
                            args.Add(match.Value);
                            i += match.Value.Length - 1;
                        }
                        else
                            throw new ParseException(i + 1 + Offset, rest);

                        break;
                }
            }

            arguments = p.Substring(0, index - 2);
            return p.Substring(index);
        }

        #endregion
    }
}
