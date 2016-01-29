using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// Class for scanning and building symbol expressions
    /// </summary>
	class SymbolExpression : Expression
    {
        #region Fields

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

