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
    /// Class for scanning and building symbol expressions
    /// </summary>
	class SymbolExpression : Expression
    {
        #region Members

        string symbolName;

        #endregion

        #region ctor

        public SymbolExpression()
		{
		}

        public SymbolExpression(string content)
        {
            symbolName = content;
        }

        public SymbolExpression(ParseEngine engine, string name)
            : base(engine)
        {
            symbolName = name;
            Length = name.Length;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the found symbol.
        /// </summary>
        public string SymbolName
        {
            get { return symbolName; }
        }

        #endregion

        #region Methods

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
            if (symbols.ContainsKey(symbolName))
                return symbols[symbolName];

            var variable = Context.GetVariable(symbolName);

            if (variable != null)
                return variable;

            var constant = Context.FindConstants(symbolName);

            if (constant != null)
                return constant.Value;

            var function = Context.FindFunction(symbolName);

            if (function != null)
                return new FunctionValue(function);

            function = Query.GetFromBuffer(symbolName);

            if (function != null)
                return new FunctionValue(function);

            function = Context.LoadFunction(symbolName);

            if (function != null)
            {
                Query.SetToBuffer(symbolName, function);
                return new FunctionValue(function);
            }
            
            throw new YAMPSymbolMissingException(symbolName);
        }

        public override Expression Scan(ParseEngine engine)
        {
            var index = engine.Pointer;
            var chars = engine.Characters;

            if (ParseEngine.IsIdentifierStart(chars[index]))
            {
                index++;

                while (index < chars.Length && ParseEngine.IsIdentifierPart(chars[index]))
                    index++;

                var name = new String(chars, engine.Pointer, index - engine.Pointer);

                if (engine.UseKeywords)
                {
                    var keyword = Elements.Instance.FindKeywordExpression(name, engine);

                    if (keyword != null)
                        return keyword;
                }

                var exp = new SymbolExpression(engine, name);
                engine.SetPointer(index);
                return exp;
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return symbolName;
        }

        #endregion
    }
}

