/*
	Copyright (c) 2012-2014, Florian Rappl.
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
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This class represents the basis of the assignment operators
    /// as well as the simple assignment operator (=).
    /// </summary>
	class AssignmentOperator : BinaryOperator
    {
        #region ctor

        public AssignmentOperator () : this(string.Empty)
		{
		}

		public AssignmentOperator(string prefix) : base(prefix + "=", -1)
		{
		}

        #endregion

        #region Methods

        public override Operator Create()
		{
			return new AssignmentOperator();
		}

		public override Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
		{
			var bottom = right.Interpret(symbols);
			return Assign(left, bottom, symbols);
		}
		
		public override Value Perform (Value left, Value right)
		{
			return right;
		}

		protected Value Assign(Expression left, Value value, Dictionary<string, Value> symbols)
		{
			if (left is SymbolExpression)
                return Assign((SymbolExpression)left, value, symbols);
			else if(left is ContainerExpression)
			{
                var tree = (ContainerExpression)left;

				if (tree.Operator == null)
					return Assign(tree.Expressions[0], value, symbols);
				else if(tree.Operator is ArgsOperator)
				{
					var ix = (ArgsOperator)tree.Operator;
					return ix.Handle(tree.Expressions[0], value, symbols);
				}
                else if (tree.IsSymbolList)
                {
                    var vars = tree.GetSymbols();
                    return HandleMultipleOutputs(value, vars, symbols);
                }
                else
                    throw new YAMPAssignmentException(Op);
			}
			
			return value;
		}

        #endregion

        #region Helpers

        Value HandleMultipleOutputs(Value value, SymbolExpression[] vars, Dictionary<string, Value> symbols)
        {
            if (value is ArgumentsValue)
            {
                var av = (ArgumentsValue)value;
                var l = Math.Min(vars.Length, av.Length);

                for (var i = 0; i != l; i++)
                    Assign(vars[i], av.Values[i], symbols);

                return av;
            }

            foreach (var sym in vars)
                Assign(sym, value, symbols);

            return value;
        }

        Value Assign(SymbolExpression left, Value value, Dictionary<string, Value> symbols)
		{
            if (symbols.ContainsKey(left.SymbolName))
                symbols[left.SymbolName] = value.Copy();
            else
                Context.AssignVariable(left.SymbolName, value.Copy());

			return value;
        }

        #endregion
    }
}

