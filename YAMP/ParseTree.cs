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
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The class used for the global and local ParseTree
    /// </summary>
	public class ParseTree
	{
		#region Constants

		const string SPACING = "  ";

		#endregion

		#region Members
		
		Operator _operator;
		Expression[] _expressions;
		string _input;
		int _offset;
		bool _isList;

		#endregion

		#region Properties

        /// <summary>
        /// Gets the context of the parse tree.
        /// </summary>
		public bool IsList
		{
			get { return _isList; }
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

		#endregion

		#region ctor
		
		public ParseTree(Operator op, Expression[] expressions)
		{
			_operator = op;
			_expressions = expressions;
		}
		
		public ParseTree(Operator op, Expression expression)
		{
			_operator = op;
			_expressions = new Expression[] { expression };
		}
		
		public ParseTree(Operator op, Expression left, Expression right)
		{
			_operator = op;
			_expressions = new Expression[] { left, right };
		}

		public ParseTree (string input, int offset) : this(input, offset, false)
		{
		}

		public ParseTree (string input) : this(input, 0, false)
		{
		}

		public ParseTree (string input, bool isList) : this(input, 0, isList)
		{
		}

		public ParseTree(string input, int offset, bool isList)
		{
			_offset = offset;
			_input = input;
			_isList = isList;
			Parse();
		}
		
		#endregion

        #region Methods

        void Parse()
		{			
			BracketExpression bracket = null;
			var ops = new List<Operator>();
			var exps = new List<Expression>();
			var shadow = _input;
			var takeop = false;
			var maxLevel = -100;
			var maxIndex = 0;
			var expIndex = 0;
			var tmpExp = 0;
            var offset = _offset;

            if (_input.Length == 0)
            {
                _expressions = new Expression[] { new EmptyExpression() };
                return;
            }

			while(shadow.Length > 0)
			{
				if(shadow[0] == ' ')
				{
					offset++;
					shadow = shadow.Substring(1);
					continue;
				}

				if(shadow.Length > 1)
				{
					var key = new string(new char[] { shadow[0], shadow[1] });

					if(Tokens.Instance.Sanatizers.ContainsKey(key))
					{
						offset++;
						shadow = Tokens.Instance.Sanatizers[key] + shadow.Substring(2);
						continue;
					}
				}

				if(takeop)
				{
					var op = Tokens.Instance.FindOperator(shadow);

					if(op.Level >= maxLevel)
					{
						maxLevel = op.Level;
						maxIndex
							= ops.Count;
						expIndex = exps.Count;
					}

					op.IsList = _isList;
					ops.Add(op);
					shadow = op.Set(shadow);
					offset += op.Input.Length;
					takeop = !op.ExpectExpression;
				}
				else
				{
					var exp = Tokens.Instance.FindExpression(shadow);
					exp.Offset = offset;
					exps.Add(exp);
					shadow = exp.Set(shadow);
					offset += exp.Input.Length;
					takeop = true;
				}
            }

            expIndex--;

			while(ops.Count > 1)
            {
				if(ops[maxIndex].ExpectExpression)
				{
					tmpExp = expIndex + 1;
					bracket = new BracketExpression(new ParseTree(ops[maxIndex], exps[expIndex], exps[tmpExp]));
					exps.RemoveAt(tmpExp);
				}
				else
				{
					bracket = new BracketExpression(new ParseTree(ops[maxIndex], exps[expIndex]));
				}

				exps.RemoveAt(expIndex);
				exps.Insert(expIndex, bracket);
				ops.RemoveAt(maxIndex);
				tmpExp = 1;
				expIndex = 0;
				maxIndex = 0;
				maxLevel = ops[0].Level;

				for(var i = 1; i < ops.Count; i++)
				{	
					if(maxLevel <= ops[i].Level)
					{
						expIndex = tmpExp;
						maxIndex = i;
						maxLevel = ops[i].Level;
                    }

                    if (ops[i].ExpectExpression)
                        tmpExp++;
				}
			}

			_expressions = exps.ToArray();
			
			if(ops.Count == 1)
				_operator = ops[0];
		}

        string PrintExpression(Expression exp)
        {
            var lines = exp.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var line in lines)
                sb.Append(SPACING).AppendLine(line);

            return sb.ToString();
        }
		
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append(PrintExpression(Expressions[0]));
			
			if(Operator != null)
			{
				sb.Append(SPACING).AppendLine(Operator.ToString());
				
				if(Expressions.Length == 2)
					sb.Append(PrintExpression(Expressions[1]));
			}
			
			return sb.ToString();
        }

        #endregion
    }
}

