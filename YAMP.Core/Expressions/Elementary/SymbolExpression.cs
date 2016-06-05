namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// Class for scanning and building symbol expressions
    /// </summary>
	class SymbolExpression : Expression
    {
        #region Fields

        readonly String _symbolName;

        #endregion

        #region ctor

        public SymbolExpression()
		{
		}

        public SymbolExpression(String symbolName)
        {
            _symbolName = symbolName;
        }

        public SymbolExpression(ParseEngine engine, String symbolName)
            : base(engine)
        {
            _symbolName = symbolName;
            Length = symbolName.Length;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the found symbol.
        /// </summary>
        public String SymbolName
        {
            get { return _symbolName; }
        }

        #endregion

        #region Methods

		public override Value Interpret(IDictionary<String, Value> symbols)
		{
            if (symbols.ContainsKey(_symbolName))
            {
                return symbols[_symbolName];
            }

            var variable = Context.GetVariable(_symbolName);

            if (variable != null)
            {
                return variable;
            }

            var constant = Context.FindConstants(_symbolName);

            if (constant != null)
            {
                return constant.Value;
            }

            var function = Context.FindFunction(_symbolName);

            if (function == null)
            {
                function = Query.GetFromBuffer(_symbolName);

                if (function == null)
                {
                    function = Context.LoadFunction(_symbolName);

                    if (function == null)
                    {
                        throw new YAMPSymbolMissingException(_symbolName);
                    }

                    Query.SetToBuffer(_symbolName, function);
                    return new FunctionValue(function);
                }
            }

            return new FunctionValue(function);
        }

        public override Expression Scan(ParseEngine engine)
        {
            var index = engine.Pointer;
            var chars = engine.Characters;

            if (ParseEngine.IsIdentifierStart(chars[index]))
            {
                index++;

                while (index < chars.Length && ParseEngine.IsIdentifierPart(chars[index]))
                {
                    index++;
                }

                var name = new String(chars, engine.Pointer, index - engine.Pointer);

                if (engine.UseKeywords)
                {
                    var keyword = engine.Elements.FindKeywordExpression(name, engine);

                    if (keyword != null)
                    {
                        return keyword;
                    }
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
            return _symbolName;
        }

        /// <summary>
        /// Returns a string to allow visualization of a Expression tree
        /// </summary>
        /// <returns>The string that represents the part of the expression tree element.</returns>
        public override String ToDebug(int padLeft, int tabsize)
        {
            string baseDebug = base.ToDebug(padLeft, tabsize);

            string pad = new string(' ', padLeft);

            return string.Format("{0}[{1} <{2}>]", pad, baseDebug, ToCode());
        }


        #endregion
    }
}

