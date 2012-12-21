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
using System.Text.RegularExpressions;

namespace YAMP
{
    class TreeExpression : Expression
    {
        #region Members

        ParseTree _tree;

        #endregion

        #region ctor

        public TreeExpression(QueryContext query) : base("")
        {
            Query = query;
        }

        public TreeExpression(Operator op, Expression exp) : base("")
        {
            _tree = new ParseTree(op, exp);
        }

        public TreeExpression(Operator op, Expression left, Expression right) : base("")
        {
            _tree = new ParseTree(op, left, right);
        }

        protected TreeExpression(string pattern) : base(pattern)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression tree.
        /// </summary>
        public ParseTree Tree
        {
            get { return _tree; }
            protected set { _tree = value; }
        }

        /// <summary>
        /// Gets the input for this tree.
        /// </summary>
        internal override string Input
        {
            get
            {
                return base.Input ?? _tree.Input;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the tree expression.
        /// </summary>
        /// <param name="context">The context of the created tree expression.</param>
        /// <param name="match">The match that corresponds to this tree expression.</param>
        /// <returns>The created three expression.</returns>
        public override Expression Create(QueryContext query, Match match)
        {
            return new TreeExpression(query);
        }

        /// <summary>
        /// Interprets this tree expression.
        /// </summary>
        /// <param name="symbols">The symbols to consider.</param>
        /// <returns>The value of this tree expression.</returns>
        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return Tree.Interpret(symbols);
        }

        public override string Set(string input)
        {
            _input = input;
            _tree = new ParseTree(Query, _input, Offset);
            return string.Empty;
        }

        public override string ToString()
        {
            if (Tree != null)
                return Tree.ToString();

            return base.ToString();
        }

        #endregion
    }
}
