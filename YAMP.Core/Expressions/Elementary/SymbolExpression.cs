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

        public SymbolExpression(String content)
        {
            _symbolName = content;
        }

        public SymbolExpression(ParseEngine engine, String name)
            : base(engine)
        {
            _symbolName = name;
            Length = name.Length;
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

        #endregion
    }
}

