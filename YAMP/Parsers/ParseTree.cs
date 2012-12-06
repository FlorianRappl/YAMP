/*
	Copyright (c) 2012, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
	/// <summary>
	/// The class used for the global and local ParseTree
	/// </summary>
	public class ParseTree
	{
		#region Members

		Operator _operator;
		Expression[] _expressions;
		string _input;
		int _offset;
        int _final;
		QueryContext _query;
		Stack<Tokens> _skips;

		static readonly string unicodeWhitespaces = "\u1680\u180E\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\uFEFF";

		#endregion

		#region ctor

		internal ParseTree(Operator op, Expression[] exps)
		{
			_operator = op;
			_expressions = exps;
		}

		internal ParseTree(Operator op, Expression exp)
		{
			_operator = op;
			_expressions = new Expression[] { exp };
		}

		internal ParseTree(Operator op, Expression left, Expression right)
		{
			_operator = op;
			_expressions = new Expression[] { left, right };
		}

		internal ParseTree(QueryContext query, string input, int offset)
		{
			_query = query;
			_offset = offset;
			_input = input;
			Parse();
		}

		/// <summary>
		/// Main constructor - use as entry point!
		/// </summary>
		/// <param name="query">The query context.</param>
		/// <param name="input">The input to parse.</param>
		public ParseTree(QueryContext query, string input) : this(query, input, 0)
		{
		}

		#endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating if the parse tree consists only of symbols that 
        /// are seperated by columns (commas).
        /// </summary>
        public bool IsSymbolList
        {
            get
            {
                if (Expressions == null)
                    return false;

                if (Operator != null && !(Operator is MatrixColumnOperator))
                    return false;

                foreach (var expression in Expressions)
                {
                    if (expression is TreeExpression)
                    {
                        if (((TreeExpression)expression).Tree.IsSymbolList)
                            continue;
                    }
                    else if (expression is SymbolExpression)
                        continue;

                    return false;
                }

                return true;
            }
        }

		/// <summary>
		/// Gets the operator used for this parse tree (can be null).
		/// </summary>
		public Operator Operator
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
			}
		}

		/// <summary>
		/// Gets the array with all found expressions in the parse tree.
		/// </summary>
		public Expression[] Expressions
		{
			get
			{
				return _expressions;
			}
			set
			{
				_expressions = value;
			}
		}

		/// <summary>
		/// Gets a value if the last output was actually saved in a variable.
		/// </summary>
		public bool IsAssignment
		{
			get
			{
				if (Operator != null)
					return Operator is AssignmentOperator;

				if (Expressions == null || Expressions.Length != 1)
					return false;

				if (Expressions[0] is TreeExpression)
                    return ((TreeExpression)Expressions[0]).Tree.IsAssignment;

				return false;
			}
		}

		/// <summary>
		/// Gets a value if the interpreter has any content (can do interpretation).
		/// </summary>
		public bool HasContent
		{
			get
			{
				return _expressions != null && _expressions.Length > 0;
			}
		}

		/// <summary>
		/// Gets the assigned QueryContext.
		/// </summary>
		public QueryContext Query
		{
			get { return _query; }
		}

		/// <summary>
		/// Gets the last skipped character.
		/// </summary>
		protected Stack<Tokens> Skips
		{
			get { return _skips; }
		}

        /// <summary>
        /// Gets the initial offset at the beginning of the parsing.
        /// </summary>
        public int Offset
        {
            get { return _offset; }
        }

        /// <summary>
        /// Gets the length of the expression being parsed.
        /// </summary>
        public int Length
        {
            get { return _final - _offset; }
        }

        /// <summary>
        /// Gets the string that has been parsed by this instance.
        /// </summary>
        public string Input
        {
            get { return _input; }
            protected set { _input = value; }
        }

		#endregion

		#region Methods

		protected virtual Operator FindOperator(string input)
		{
			return Elements.Instance.FindOperator(_query, input);
		}

		protected virtual Value DefaultValue()
		{
			return null;
		}

		void Parse()
		{
			var operators = new Stack<Operator>();
			var expressions = new Stack<Expression>();
			var takeop = false;
            var maxLevel = -100;
            var shadow = _input;
			_final = _offset;
            _skips = new Stack<Tokens>();

			while (shadow.Length > 0)
            {
				if (IsNewLine(shadow[0]))
				{
					shadow = shadow.Substring(1);
					_skips.Push(Tokens.Newline);
					_final++;
					continue;
				}
				else if (IsWhiteSpace(shadow[0]))
				{
					shadow = shadow.Substring(1);
					_skips.Push(Tokens.Whitespace);
					_final++;
					continue;
				}
				else if (IsBlockComment(shadow))
				{
					var index = shadow.IndexOf("*/") + 2;
					shadow = shadow.Substring(index == 1 ? shadow.Length : index);
					_final += index;
					continue;
				}
				else if (IsLineComment(shadow))
				{
					var index = shadow.IndexOf('\n') + 1;
					shadow = shadow.Substring(index == 0 ? shadow.Length : index);
					_final += index;
					continue;
				}

				if (takeop)
				{
					var op = FindOperator(shadow);

					if (!op.ExpectExpression)
						expressions.Push(new TreeExpression(op, expressions.Pop()));
					else
					{
						if (op.Level >= (op.IsRightToLeft ? maxLevel : maxLevel + 1))
							maxLevel = op.Level;
						else
						{
							while (true)
							{
								var right = expressions.Pop();
								var left = expressions.Pop();
								expressions.Push(new TreeExpression(operators.Pop(), left, right));

								if (operators.Count == 0 || operators.Peek().Level <= op.Level)
								{
									maxLevel = op.Level;
									break;
								}
							}
						}

						takeop = false;
						operators.Push(op);
					}

					shadow = op.Set(shadow);
					_final += op.Input.Length;
				}
				else
				{
					var exp = Elements.Instance.FindExpression(_query, shadow);
					exp.Offset = _final;
					expressions.Push(exp);
					shadow = exp.Set(shadow);
					_final += exp.Input.Length;
					takeop = true;
				}
			}

			if(expressions.Count > 0 && operators.Count != expressions.Count - 1)
				throw new ExpressionNotFoundException(_input.Substring(_input.Length < 5 ? 0 : _input.Length - 5));

			while (operators.Count > 1)
			{
				var right = expressions.Pop();
				var left = expressions.Pop();
				expressions.Push(new TreeExpression(operators.Pop(), left, right));
			}

			if (operators.Count == 1)
			{
				Operator = operators.Pop();
				var right = expressions.Pop();
				var left = expressions.Pop();
				_expressions = new Expression[] { left, right };
			}
			else if (expressions.Count == 1)
			{
				_expressions = new Expression[] { expressions.Pop() };
			}
		}

		internal virtual Value Interpret(Dictionary<string, Value> symbols)
		{
			if (Operator != null)
				return Operator.Evaluate(Expressions, symbols);
			else if (HasContent)
				return Expressions[0].Interpret(symbols);
				
			return DefaultValue();
		}

		public override string ToString()
		{
			if (!HasContent)
				return "-- empty --";

			var sb = new StringBuilder();
            sb.Append("[ ").Append(Expressions[0]).Append(" ]");

			if (Operator != null)
			{
				sb.Append(" [ ").Append(Operator).Append(" ]");

                if (Expressions.Length == 2)
                    sb.Append(" [ ").Append(Expressions[1]).Append(" ]");
			}

			return sb.ToString();
		}

		#endregion

		#region Helpers

		protected bool IsWhiteSpace(char ch) 
		{
			return (ch == 32) ||  // space
				(ch == 9) ||      // horizontal tab
				(ch == 0xB) ||	  // vertical tab
				(ch == 0xC) ||	  // form feed / new page
				(ch == 0xA0) ||	  // non-breaking space
				(ch >= 0x1680 && unicodeWhitespaces.IndexOf(ch.ToString()) >= 0);
		}

		protected bool IsNewLine(char ch) 
		{
			return (ch == 10) ||  // line feed
				(ch == 13) ||	  // carriage return
				(ch == 0x2028) || // line seperator
				(ch == 0x2029);	  // paragraph seperator
		}

		protected bool IsLineComment(string shadow)
		{
			return shadow.Length > 1 && shadow[0] == '/' && shadow[1] == '/';
		}

		protected bool IsBlockComment(string shadow)
		{
			return shadow.Length > 1 && shadow[0] == '/' && shadow[1] == '*';
		}

		#endregion
	}
}

