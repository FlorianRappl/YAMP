using System;
using System.Collections;

namespace YAMP
{
	public class SpecialExpression : Expression
	{
        public string SpecialName
        {
            get { return this._input; }
        }
		
		public SpecialExpression () : base(@"((\$)|(:))")
		{
		}

        public override Expression Create()
        {
            return new SpecialExpression();
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

