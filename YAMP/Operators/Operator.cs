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
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace YAMP
{
    /// <summary>
    /// The abstract base class for any operator (unary, binary, ...).
    /// </summary>
	public abstract class Operator : Block, IRegisterElement
	{
		#region Members

		string _op;

		#endregion

		#region ctor

        /// <summary>
        /// Creates a new operator given the string for the operator.
        /// </summary>
        /// <param name="op">The operator string like +.</param>
		public Operator (string op) : this(op, 0)
		{
		}
		
        /// <summary>
        /// Creates a new operator given the string and level for the operator.
        /// </summary>
        /// <param name="op">The operator string like +.</param>
        /// <param name="level">The operator level like 100.</param>
		public Operator (string op, int level)
		{
			_op = op;
			Level = level;
            Length = op.Length;
		}

		#endregion

        #region Properties

        /// <summary>
        /// Gets a dummy operator for doing nothing.
        /// </summary>
        public static Operator Void
        {
            get { return new VoidOperator(); }
        }

        /// <summary>
        /// Gets how many expressions are eaten by thix operator (1 = unary, 2 = binary, ...).
        /// </summary>
        public int Expressions
        {
            get;
            protected set;
        }
		
		/// <summary>
		/// Gets the operator's string.
		/// </summary>
		public string Op
		{
			get { return _op; }
		}
		
		/// <summary>
		/// Gets the level of the operator.
		/// </summary>
		public int Level
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets if the operator has to be executed from right to left for chained scenarios.
		/// </summary>
		public bool IsRightToLeft
		{
			get;
			protected set;
        }

		#endregion

		#region Methods

        /// <summary>
        /// Begins the evaluation of given expressions.
        /// </summary>
        /// <param name="expressions">The expressions to evaluate.</param>
        /// <param name="symbols">External symbols to consider.</param>
        /// <returns>The result of the evaluation.</returns>
		public abstract Value Evaluate(Expression[] expressions, Dictionary<string, Value> symbols);

        /// <summary>
        /// Creates a new instance of the current operator.
        /// </summary>
        /// <returns>The new instance.</returns>
        public abstract Operator Create();

        /// <summary>
        /// Creates a new instance of the current operator.
        /// </summary>
        /// <param name="engine">The engine that is used for parsing the query.</param>
        /// <returns>The new instance.</returns>
        public virtual Operator Create(ParseEngine engine)
        {
            var op = Create();
            op.Query = engine.Query;
            op.StartColumn = engine.CurrentColumn;
            op.StartLine = engine.CurrentLine;
            engine.Advance(Op.Length);
            return op;
        }
		
        /// <summary>
        /// Registers the operator at its factory.
        /// </summary>
		public virtual void RegisterElement()
		{
			Elements.Instance.AddOperator(_op, this);
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Returns the string representation of the operator.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}) {2}", StartLine, StartColumn, GetType().Name.RemoveOperatorConvention());
		}

        /// <summary>
        /// The code representation of the operator, which is usually just the operator itself.
        /// </summary>
        /// <returns>A valid part of a YAMP query.</returns>
        public override string ToCode()
        {
            return _op;
        }

		#endregion
	}
}