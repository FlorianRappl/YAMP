using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace YAMP
{
	class BracketExpression : Expression
	{
		ParseTree _tree;
		int _bracketIndex;
		static readonly char[] openBrackets = new char[] { '(', '[', '{' };
		static readonly char[] closeBrackets = new char[] { ')', ']', '}' };
		
		public BracketExpression(ParseTree tree) : this()
		{
			_tree = tree;
		}
		
		public BracketExpression () : base(@"\(.*\)")
		{
			_bracketIndex = -1;
		}

		internal override string Input 
		{
			get
			{
				if(_bracketIndex == -1)
					return base.Input;

				return openBrackets[_bracketIndex] + _input + closeBrackets[_bracketIndex];
			}
		}

		public override Expression Create(Match match)
        {
            return new BracketExpression();
        }
		
		public ParseTree Tree
		{
			get { return _tree; }
		}
		
		public override Value Interpret (Hashtable symbols)
		{
            if (_tree.Operator != null)
                return _tree.Operator.Evaluate(_tree.Expressions, symbols);
            
			return _tree.Expressions[0].Interpret(symbols);
		}
		
		public override string Set (string input)
		{
			return Set (input, false);
		}

		public string Set (string input, bool isList)
		{
			for(var i = 0; i < openBrackets.Length; i++)
			{
				if(input[0] == openBrackets[i])
				{
					_bracketIndex = i;
					return SetBracketContent(input, isList);
				}
			}
			
			_input = input;
			_tree = new ParseTree(_input, Offset, isList);
			return string.Empty;
		}
		
		public override string ToString ()
		{
			return _tree.ToString();
		}
		
		protected virtual string SetBracketContent(string input, bool isList)
		{
			var brackets = 1;
			
			for(var i = 1; i < input.Length; i++)
			{
				for(var j = 0; j < closeBrackets.Length; j++)
				{
					if(input[i] == closeBrackets[j])
						brackets--;
					else if(input[i] == openBrackets[j])
						brackets++;
					else
						continue;

					break;
				}
				
				if(brackets == 0)
				{
					if(input[i] != closeBrackets[_bracketIndex])
						throw new BracketException(Offset + i, closeBrackets[_bracketIndex].ToString(), input);

					_input = input.Substring(1, i - 1);
					_tree = new ParseTree(_input, Offset, isList);
					return input.Substring(i + 1);
				}
			}
			
			throw new BracketException(Offset, openBrackets[_bracketIndex].ToString(), input);
		}
	}
}