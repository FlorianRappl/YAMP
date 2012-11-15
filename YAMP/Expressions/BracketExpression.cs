using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace YAMP
{
	abstract class BracketExpression : TreeExpression
	{
		#region Members

		char _openBracket;
		char _closeBracket;

		#endregion

		#region ctor

		public BracketExpression(string pattern, char openBracket, char closeBracket) : base(pattern)
		{
			_openBracket = openBracket;
			_closeBracket = closeBracket;
		}

		#endregion

		#region Methods

		internal override string Input 
		{
			get
			{
				return _openBracket + _input + _closeBracket;
			}
		}

		public override string Set(string input)
		{
			var brackets = 1;

			for (var i = 1; i < input.Length; i++)
			{
				if (input[i] == _closeBracket)
					brackets--;
				else if (input[i] == _openBracket)
					brackets++;

				if (brackets == 0)
				{
					if (input[i] != _closeBracket)
						throw new BracketException(Offset + i, _closeBracket.ToString(), input);

					_input = input.Substring(1, i - 1);
					Tree = CreateParseTree(_input);
					return input.Substring(i + 1);
				}
			}

			throw new BracketException(Offset, _openBracket.ToString(), input);
		}

		protected virtual ParseTree CreateParseTree(string input)
		{
			return new ParseTree(Query, input, Offset);
		}

		#endregion
	}
}