using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
    class UnaryExpression : Expression
    {
        #region Members

        Expression _child;
        bool _changeSign;

        #endregion

        #region ctors

        public UnaryExpression() : base(@"[\+\-]+")
        {
            _changeSign = false;
        }

        public UnaryExpression(ParseContext context)
            : this()
        {
            Context = context;
        }

        #endregion

        #region Methods

        public override Expression Create(ParseContext context, Match match)
        {
            return new UnaryExpression(context);
        }

        public override Value Interpret(Hashtable symbols)
        {
            var birth = _child.Interpret(symbols);

            if (_changeSign)
            {
                if (birth is ISign)
                    return (birth as ISign).ChangeSign();
                
                throw new ArgumentTypeNotSupportedException("-", Offset, birth.GetType());
            }

            return birth;
        }

        public override string Set(string input)
        {
            int index = 0;
            int minusCount = 0;

            for (; index < input.Length; index++)
            {
                if (input[index] == '+')
                    continue;
                else if (input[index] == '-')
                    minusCount++;
                else
                    break;
            }

            _changeSign = (minusCount & 1) == 1;

            if (index == input.Length)
                throw new ParseException(Offset + index - 1, input);

            var rest = input.Substring(index);
            _child = Tokens.Instance.FindExpression(Context, rest);
            _child.Offset = Offset + index;
            var delivery = _child.Set(rest);
            _input = input.Substring(0, index) + _child.Input;
            return delivery;
        }

        #endregion
    }
}
