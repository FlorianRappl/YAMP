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

namespace YAMP
{
    /// <summary>
    /// The let keyword to create local variables. Basic syntax:
    /// let NAME [OP STATEMENT];
    /// </summary>
    class LetKeyword : Keyword
    {
        #region Members

        SymbolExpression name;

        #endregion

        #region ctor

        public LetKeyword()
            : base("let")
        {
        }

        public LetKeyword(int line, int column, QueryContext query)
            : this()
        {
            Query = query;
            StartLine = line;
            StartColumn = column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the created local variable.
        /// </summary>
        public string Name
        {
            get { return name.SymbolName; }
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            if (symbols.ContainsKey(Name))
                symbols.Remove(Name);

            symbols.Add(Name, ScalarValue.Empty);
            return null;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new LetKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            engine.Advance(Token.Length);
            kw.Length = engine.Pointer - start;
            engine.Skip();
            start = engine.Pointer;
            kw.name = Elements.Instance.FindExpression<SymbolExpression>().Scan(engine) as SymbolExpression;

            if (kw.name == null)
            {
                engine.AddError(new YAMPVariableNameMissing(engine), kw);
                return kw;
            }

            engine.SetPointer(start);
            return kw;
        }

        #endregion

        #region String Representation

        public override string ToCode()
        {
            return "let ";
        }

        #endregion
    }
}
