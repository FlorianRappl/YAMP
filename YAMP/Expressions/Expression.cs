/*
	Copyright (c) 2012-2013, Florian Rappl.
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

namespace YAMP
{
    /// <summary>
    /// Represents the abstract base class for expressions.
    /// </summary>
	public abstract class Expression : Block, IRegisterElement
	{
		#region ctor

		public Expression()
		{
		}

        public Expression(int line, int column)
        {
            StartLine = line;
            StartColumn = column;
        }

        public Expression(QueryContext query)
        {
            Query = query;
        }

        public Expression(QueryContext query, int line, int column) : this(line, column)
        {
            Query = query;
        }

        public Expression(ParseEngine engine)
            : this(engine.Query, engine.CurrentLine, engine.CurrentColumn)
        {
        }

		#endregion

        #region Properties

        /// <summary>
        /// Gets a dummy expression for doing nothing.
        /// </summary>
        public static Expression Empty
        {
            get { return new EmptyExpression(); }
        }

        /// <summary>
        /// Gets a value indicating if the expression is a whole statement.
        /// </summary>
        public bool IsSingleStatement
        {
            get;
            protected set;
        }

		#endregion

		#region Methods

        public abstract Value Interpret(Dictionary<string, Value> symbols);

        public abstract Expression Scan(ParseEngine engine);
		
		public virtual void RegisterElement()
		{
			Elements.Instance.AddExpression(this);
		}

        #endregion

        #region String Representations

        public override string ToString ()
		{
            return string.Format("({0}, {1}) {2}", StartLine, StartColumn, GetType().Name.RemoveExpressionConvention());
		}

		#endregion
	}
}

