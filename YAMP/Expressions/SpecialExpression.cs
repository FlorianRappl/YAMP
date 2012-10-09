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

        public SpecialExpression(ParseContext context, Match match) : this()
		{
            Context = context;
			mx = match;
		}

		public override Expression Create(ParseContext context, Match match)
        {
            return new SpecialExpression(context, match);
        }
		
		public override Value Interpret (Hashtable symbols)
		{
			if(SpecialName.Equals(":"))
				return new RangeValue();

            var variable = Context.GetVariable(SpecialName);

            if (variable != null)
                return variable;
			
			throw new ParseException(Offset, SpecialName);
		}
	}
}

