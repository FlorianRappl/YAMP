using System;
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// This class represents a group of statements.
    /// </summary>
    class GroupExpression : Expression
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public GroupExpression()
		{
		}

        /// <summary>
        /// Creates a new instance with some parameters.
        /// </summary>
        /// <param name="line">The line where the scope expression starts.</param>
        /// <param name="column">The column in the line where the scope exp. starts.</param>
        /// <param name="length">The length of the scope expression.</param>
        /// <param name="scope">The associated query context (scope).</param>
        public GroupExpression(int line, int column, int length, QueryContext scope)
            : base(scope.Parent, line, column)
		{
            Scope = scope;
            IsSingleStatement = true;
            Length = length;
		}

        #endregion

        #region Properties

        public QueryContext Scope { get; private set; }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            var localSymbols = new Dictionary<string, Value>(symbols);
            Scope.Interpret(localSymbols);
            return Scope.Output;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var chars = engine.Characters;

            if (chars[start] == '{')
            {
                var index = start;
                var line = engine.CurrentLine;
                var column = engine.CurrentColumn;
                engine.Advance();
                index++;
                var scope = new QueryContext(engine.Query);
                var eng = scope.Parser;
                eng.Reset(scope.Input.Substring(index))
                    .SetOffset(line, column + 1)
                    .Parse();

                if (!eng.IsTerminated)
                    engine.AddError(new YAMPScopeNotClosedError(line, column));

                foreach (var error in eng.Errors)
                    engine.AddError(error);

                engine.Advance(eng.Pointer);
                return new GroupExpression(line, column, engine.Pointer - start, scope);
            }

            return null;
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Transforms the expression into YAMP query code.
        /// </summary>
        /// <returns>The string representation of the part of the query.</returns>
        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");

            foreach (var statement in Scope.Parser.Statements)
                sb.Append("\t").Append(statement.Container.ToCode()).AppendLine(";");

            sb.Append("}");
            return sb.ToString();
        }

        #endregion
    }
}
