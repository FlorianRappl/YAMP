using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class SpecialExpression : Expression
	{
        public string SpecialName
        {
            get { return this._input; }
        }
		
		public SpecialExpression () : base(@"((\$)|(:))")
		{
		}

		public SpecialExpression (Match match) : this()
		{
			mx = match;
		}

		public override Expression Create(Match match)
        {
            return new SpecialExpression(match);
        }
		
		public override Value Interpret (Hashtable symbols)
		{
			if(SpecialName.Equals(":"))
				return new RangeValue();

            var variable = Tokens.Instance.GetVariable(SpecialName);

            if (variable != null)
                return variable;
			
			throw new ParseException(Offset, SpecialName);
		}
	}
}

