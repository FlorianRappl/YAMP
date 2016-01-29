namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The parse engine of YAMP. This engine has been introduced with YAMP v1.2
    /// and is called YAMP-PE, which stands for YAMP-ParseEngine.
    /// </summary>
    public sealed class ParseEngine
    {
        #region Fields

        readonly QueryContext query;
        readonly List<YAMPParseError> errors;
        readonly List<Statement> statements;

        Char[] characters;
        ParseEngine parent;
        Int32 currentLine;
        Int32 currentColumn;
        Boolean parsed;
        Boolean parsing;
        Boolean useKeywords;
        Statement currentStatement;
        Boolean terminated;
        Marker markers;
        Int32 ptr;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance of the parse engine.
        /// </summary>
        /// <param name="input">The query context to consider.</param>
        public ParseEngine(QueryContext input)
        {
            query = input;
            useKeywords = Parser.UseScripting;
            characters = input.Input.ToCharArray();
            errors = new List<YAMPParseError>();
            statements = new List<Statement>();
            markers = Marker.None;
            currentLine = 1;
            currentColumn = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the names of all collected (user-defined) symbols.
        /// </summary>
        public String[] CollectedSymbols
        {
            get
            {
                var symbols = new List<String>();

                for (var i = 0; i < statements.Count; i++)
                {
                    if (statements[i] != null && statements[i].Container != null)
                    {
                        var listOfSymbols = statements[i].Container.GetSymbols();

                        for (var j = 0; j < listOfSymbols.Length; j++)
                        {
                            if (symbols.Contains(listOfSymbols[j].SymbolName))
                                continue;

                            if (Context.FindFunction(listOfSymbols[j].SymbolName) != null)
                                continue;

                            if (Context.FindConstants(listOfSymbols[j].SymbolName) != null)
                                continue;

                            symbols.Add(listOfSymbols[j].SymbolName);
                        }
                    }
                }

                return symbols.ToArray();
            }
        }

        /// <summary>
        /// Gets the parent parse engine (NULL if topmost) of the current instance.
        /// </summary>
        public ParseEngine Parent
        {
            get { return parent; }
            internal set { parent = value; }
        }

        /// <summary>
        /// Gets the number of errors found during parsing the input.
        /// </summary>
        public Int32 ErrorCount
        {
            get { return errors.Count; }
        }

        /// <summary>
        /// Gets the current character pointer position.
        /// </summary>
        internal Int32 Pointer
        {
            get { return ptr; }
        }

        /// <summary>
        /// Gets the currently used query context.
        /// </summary>
        public QueryContext Query
        {
            get { return query; }
        }

        /// <summary>
        /// Gets the corresponding parse context.
        /// </summary>
        public ParseContext Context
        {
            get { return query.Context; }
        }

        /// <summary>
        /// Gets the status of the current parse tree. Can the tree be interpreted?
        /// </summary>
        public Boolean CanRun
        {
            get { return parsed && errors.Count == 0; }
        }

        /// <summary>
        /// Gets the status of the parser. Has it been executed?
        /// </summary>
        public Boolean IsParsed
        {
            get { return parsed; }
        }

        /// <summary>
        /// Gets the status of the termination. Has it been terminated properly?
        /// </summary>
        public Boolean IsTerminated
        {
            get { return terminated; }
        }

        /// <summary>
        /// Gets the status of the parser. Is it currently parsing?
        /// </summary>
        public Boolean IsParsing
        {
            get { return parsing; }
        }

        /// <summary>
        /// Gets the current line of the parser.
        /// </summary>
        public Int32 CurrentLine
        {
            get { return currentLine; }
        }

        /// <summary>
        /// Gets the current column of the parser.
        /// </summary>
        public Int32 CurrentColumn
        {
            get { return currentColumn; }
        }

        /// <summary>
        /// Gets a value if the parser found errors in the query.
        /// </summary>
        public Boolean HasErrors
        {
            get { return errors.Count > 0; }
        }

        /// <summary>
        /// Gets an enumerable of all the found errors in the query.
        /// </summary>
        public IEnumerable<YAMPParseError> Errors
        {
            get
            {
                foreach (var error in errors)
                    yield return error;
            }
        }

        /// <summary>
        /// Gets an enumerable of all the found statements in the query.
        /// </summary>
        public IEnumerable<Statement> Statements
        {
            get
            {
                foreach (var statement in statements)
                    yield return statement;
            }
        }

        /// <summary>
        /// Gets the last added statement.
        /// </summary>
        internal Statement LastStatement
        {
            get { return statements.Count != 0 ? statements[statements.Count - 1] : null; }
        }

        /// <summary>
        /// Gets the statement that is currently created.
        /// </summary>
        internal Statement CurrentStatement
        {
            get { return currentStatement; }
        }

        /// <summary>
        /// Gets a boolean indicating if scripting (keywords) is (are) enabled for this parser.
        /// </summary>
        public Boolean UseKeywords
        {
            get { return useKeywords; }
        }

        /// <summary>
        /// Gets the number of statements that have been found in the query.
        /// </summary>
        public Int32 Count
        {
            get { return statements.Count; }
        }

        /// <summary>
        /// Gets the characters of the query input.
        /// </summary>
        public Char[] Characters
        {
            get { return characters; }
        }

        #endregion

        #region Error Management

        /// <summary>
        /// Add a parse error to the list of parse errors.
        /// </summary>
        /// <param name="error">The parse error, which occured.</param>
        /// <returns>The current parse engine.</returns>
        internal ParseEngine AddError(YAMPParseError error)
        {
            if (errors.Count != 0)
            {
                var lerr = errors[errors.Count - 1];

                if (lerr.Column == error.Column && lerr.Line == error.Line)
                {
                    if (ptr < characters.Length - 1)
                        Advance();
                    else
                        parsing = false;

                    return this;
                }
            }

            errors.Add(error);
            return this;
        }

        /// <summary>
        /// Add a parse error to the list of parse errors with the block causing the error.
        /// </summary>
        /// <param name="error">The parse error, which occured.</param>
        /// <param name="part">The part where the error occured.</param>
        /// <returns>The current parse engine.</returns>
        internal ParseEngine AddError(YAMPParseError error, Block part)
        {
            error.Part = part;
            return AddError(error);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the complete parse tree and uses the given input for
        /// the next parse run.
        /// </summary>
        /// <param name="input">The (updated) content (query) to use.</param>
        /// <returns>The current (reseted) parse engine.</returns>
        public ParseEngine Reset(String input)
        {
            useKeywords = Parser.UseScripting;
            query.Input = input;
            characters = input.ToCharArray();
            errors.Clear();
            statements.Clear();
            markers = Marker.None;
            parsed = false;
            parsing = false;
            terminated = false;
            return this;
        }

        /// <summary>
        /// Runs the parser over the inserted query.
        /// </summary>
        /// <returns>The current parse engine.</returns>
        public ParseEngine Parse()
        {
            if (parsed)
                Reset(Query.Input);

            parsing = true;
            ptr = 0;

            while (parsing && ptr < characters.Length)
            {
                var statement = ParseStatement();

                if (!statement.IsEmpty)
                    statements.Add(statement);
            }

            parsing = false;
            parsed = true;
            return this;
        }

        /// <summary>
        /// Standard parsing of a statement (ends with ; or }).
        /// </summary>
        /// <returns>The finalized statment.</returns>
        internal Statement ParseStatement()
        {
            var statement = new Statement();

            while (parsing && ptr < characters.Length)
            {
                if (IsWhiteSpace(characters[ptr]))
                {
                    ptr++;
                    currentColumn++;
                }
                else if (IsNewLine(characters[ptr]))
                {
                    ptr++;
                    currentColumn = 1;
                    currentLine++;
                }
                else if (characters[ptr] == ';')
                {
                    statement.IsMuted = true;
                    currentColumn++;
                    ptr++;
                    break;
                }
                else if (characters[ptr] == '}')
                {
                    terminated = true;
                    parsing = false;
                    ptr++;
                    currentColumn++;
                    break;
                }
                else if (ptr < characters.Length - 1 && IsComment(characters[ptr], characters[ptr + 1]))
                {
                    if (IsLineComment(characters[ptr], characters[ptr + 1]))
                        AdvanceToNextLine();
                    else
                        AdvanceTo("*/");
                }
                else if (ParseBlock(statement).IsFinished)
                {
                    break;
                }
            }

            return statement.Finalize(this);
        }

        /// <summary>
        /// More custom parsing of a statement (ends with a custom termination char).
        /// </summary>
        /// <param name="termination">The custom termination character (e.g. ; for the standard case).</param>
        /// <param name="terminationMissing">Function (to generate an error) that is called if the termination character is not found.</param>
        /// <param name="handleCharacter">An optional function that is invoked for every character.</param>
        /// <returns>The finalized statement.</returns>
        internal Statement ParseStatement(Char termination, Func<ParseEngine, YAMPParseError> terminationMissing = null, Func<Char, Statement, Boolean> handleCharacter = null)
        {
            var terminated = false;
            var statement = new Statement();

            while (parsing && ptr < characters.Length)
            {
                if (handleCharacter != null && handleCharacter(characters[ptr], statement))
                    continue;

                if (IsWhiteSpace(characters[ptr]))
                {
                    ptr++;
                    currentColumn++;
                }
                else if (IsNewLine(characters[ptr]))
                {
                    ptr++;
                    currentColumn = 1;
                    currentLine++;
                }
                else if (characters[ptr] == termination)
                {
                    terminated = true;
                    currentColumn++;
                    ptr++;
                    break;
                }
                else if (ptr < characters.Length - 1 && IsComment(characters[ptr], characters[ptr + 1]))
                {
                    if (IsLineComment(characters[ptr], characters[ptr + 1]))
                        AdvanceToNextLine();
                    else
                        AdvanceTo("*/");
                }
                else
                {
                    ParseBlock(statement);
                }
            }

            if (!terminated)
                AddError(terminationMissing != null ? terminationMissing(this) : new YAMPTerminatorMissingError(currentLine, currentColumn, termination));

            return statement.Finalize(this);
        }

        /// <summary>
        /// Parses a block with a default operator if no operator has been found.
        /// </summary>
        /// <param name="statement">The statement that should get the block.</param>
        /// <param name="defaultOperator">The default operator if no operator has been found.</param>
        /// <returns>The statement again (allows chaining).</returns>
        internal Statement ParseBlock(Statement statement, Operator defaultOperator = null)
        {
            currentStatement = statement;

            if (statement.IsOperator)
            {
                var op = Elements.Instance.FindOperator(this) ?? defaultOperator;

                if (op == null)
                    AddError(new YAMPOperatorMissingError(currentLine, currentColumn));

                statement.Push(this, op);
            }
            else
            {
                var op = Elements.Instance.FindLeftUnaryOperator(this);

                if (op == null)
                {
                    var exp = Elements.Instance.FindExpression(this);

                    if (exp == null)
                        AddError(new YAMPExpressionExpectedError(currentLine, currentColumn));

                    statement.Push(this, exp);
                }
                else statement.Push(this, op);
            }

            return statement;
        }

        #endregion

        #region Pointer Management

        /// <summary>
        /// Replaces a character at a specified index by the given one.
        /// </summary>
        /// <param name="index">The index of the character to be replaced.</param>
        /// <param name="replacement">The character to replace the old one.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine Replace(Int32 index, Char replacement)
        {
            if(index >= 0 && index < characters.Length)
                characters[index] = replacement;

            return this;
        }

        /// <summary>
        /// Advances by one character.
        /// </summary>
        /// <returns>The current instance.</returns>
        internal ParseEngine Advance()
        {
            if (IsNewLine(characters[ptr]))
            {
                currentLine++;
                currentColumn = 1;
            }
            else
                currentColumn++;

            ptr++;
            return this;
        }

        /// <summary>
        /// Advances by the specified amount of characters.
        /// </summary>
        /// <param name="shift">The positive or negative shift.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine Advance(Int32 shift)
        {
            return SetPointer(ptr + shift);
        }

        /// <summary>
        /// Advances to the next line, i.e. the next new line character.
        /// </summary>
        /// <returns>The current instance.</returns>
        internal ParseEngine AdvanceToNextLine()
        {
            while (ptr < characters.Length)
            {
                var ch = characters[ptr];
                Advance();

                if (IsNewLine(ch))
                    break;
            }

            return this;
        }

        /// <summary>
        /// Advances to a sequence of characters given by string.
        /// </summary>
        /// <param name="target">The sequence of characters fo find.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine AdvanceTo(String target)
        {
            return AdvanceTo(target.ToCharArray());
        }

        /// <summary>
        /// Advances to a specified character.
        /// </summary>
        /// <param name="target">The character to find.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine AdvanceTo(Char target)
        {
            return AdvanceTo(new [] { target });
        }

        /// <summary>
        /// Advances to a sequence of characters.
        /// </summary>
        /// <param name="target">The sequence of characters to find.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine AdvanceTo(Char[] target)
        {
            while (ptr < characters.Length)
            {
                if (characters.Length - ptr >= target.Length)
                {
                    var found = true;

                    for (var i = 0; i < target.Length; i++)
                    {
                        if (characters[i + ptr] != target[i])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        Advance(target.Length);
                        break;
                    }
                }

                Advance();
            }

            return this;
        }

        /// <summary>
        /// Skips to the next significant character avoiding
        /// line comments, block comments, whitespaces or new lines.
        /// </summary>
        /// <returns>The current instance.</returns>
        internal ParseEngine Skip()
        {
            while (ptr < characters.Length)
            {
                if (IsWhiteSpace(characters[ptr]))
                {
                    ptr++;
                    currentColumn++;
                }
                else if (IsNewLine(characters[ptr]))
                {
                    ptr++;
                    currentColumn = 1;
                    currentLine++;
                }
                else if (ptr < characters.Length - 1 && IsComment(characters[ptr], characters[ptr + 1]))
                {
                    if (IsLineComment(characters[ptr], characters[ptr + 1]))
                        AdvanceToNextLine();
                    else
                        AdvanceTo("*/");
                }
                else
                    break;
            }

            return this;
        }

        /// <summary>
        /// Sets an offset of the parser, i.e. sets line and column
        /// to (different) values.
        /// </summary>
        /// <param name="line">The new line value.</param>
        /// <param name="column">The new column value.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine SetOffset(Int32 line, Int32 column)
        {
            currentLine = line;
            currentColumn = column;
            return this;
        }

        /// <summary>
        /// Sets the character pointer to a new position.
        /// </summary>
        /// <param name="newPosition">The new position of the character pointer ptr.</param>
        /// <returns>The current instance.</returns>
        internal ParseEngine SetPointer(Int32 newPosition)
        {
            var shift = newPosition > ptr ? 1 : -1;

            while (ptr != newPosition)
            {
                if (IsNewLine(characters[ptr]))
                {
                    currentLine += shift;
                    currentColumn = 1;
                }
                else
                    currentColumn += shift;

                ptr += shift;
            }

            return this;
        }

        #endregion

        #region Markers

        internal ParseEngine InsertMarker(Marker marker)
        {
            markers = markers | marker;
            return this;
        }

        internal ParseEngine RemoveMarker(Marker marker)
        {
            if (HasMarker(marker))
                markers = markers ^ marker;

            return this;
        }

        internal Boolean HasMarker(Marker marker)
        {
            return (markers & marker) == marker;
        }

        #endregion

        #region Static Helpers

        static readonly String unicodeWhitespaces =
            "\u1680\u180E\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\uFEFF";

        /// <summary>
        /// Determines if the given character is a letter character.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents a letter (a-zA-Z).</returns>
        public static Boolean IsLetter(Char ch)
        {
            return (ch >= 65 && ch <= 90) ||
                (ch >= 97 && ch <= 123);
        }

        /// <summary>
        /// Determines if the given character is the start of an identifier.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents the start of an identifier (a-zA-Z$_).</returns>
        public static Boolean IsIdentifierStart(Char ch) {
            return (ch == 36) || (ch == 95) ||  // $ (dollar) and _ (underscore)
                (ch >= 65 && ch <= 90) ||       // A..Z
                (ch >= 97 && ch <= 122);        // a..z
        }

        /// <summary>
        /// Determines if the given character is the part of an identifier character.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents the part of an identifier (a-zA-Z$_).</returns>
        public static Boolean IsIdentifierPart(Char ch) {
            return (ch == 36) || (ch == 95) ||    // $ (dollar) and _ (underscore)
                (ch >= 65 && ch <= 90) ||         // A..Z
                (ch >= 97 && ch <= 122) ||        // a..z
                (ch >= 48 && ch <= 57);           // 0..9
        }

        /// <summary>
        /// Determines if the given character is a number character.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents a number (0-9).</returns>
        public static Boolean IsNumber(Char ch)
        {
            return ch >= 48 && ch <= 57;
        }
        
        /// <summary>
        /// Determines if the given character is a white space character.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents a white space.</returns>
        public static Boolean IsWhiteSpace(Char ch)
        {
            return (ch == 32) ||  // space
                (ch == 9) ||      // horizontal tab
                (ch == 0xB) ||	  // vertical tab
                (ch == 0xC) ||	  // form feed / new page
                (ch == 0xA0) ||	  // non-breaking space
                (ch >= 0x1680 && unicodeWhitespaces.IndexOf(ch.ToString()) >= 0);
        }

        /// <summary>
        /// Determines if the given character is a new line character.
        /// </summary>
        /// <param name="ch">The character to be examined.</param>
        /// <returns>True if the character represents a new line.</returns>
        public static Boolean IsNewLine(Char ch)
        {
            return (ch == 10) ||  // line feed
                (ch == 13) ||	  // carriage return
                (ch == 0x2028) || // line seperator
                (ch == 0x2029);	  // paragraph seperator
        }

        /// <summary>
        /// Determines if the given content is a (block or line) comment.
        /// </summary>
        /// <param name="ch1">The leading character to be examined.</param>
        /// <param name="ch2">The succeeding character to be investigated.</param>
        /// <returns>True if a line comment has been found.</returns>
        public static Boolean IsComment(Char ch1, Char ch2)
        {
            return IsLineComment(ch1, ch2) || IsBlockComment(ch1, ch2);
        }

        /// <summary>
        /// Determines if the given content is a line comment.
        /// </summary>
        /// <param name="ch1">The leading character to be examined.</param>
        /// <param name="ch2">The succeeding character to be investigated.</param>
        /// <returns>True if a line comment has been found.</returns>
        public static Boolean IsLineComment(Char ch1, Char ch2)
        {
            return ch1 == '/' && ch2 == '/';
        }

        /// <summary>
        /// Determines if the given content is a block comment.
        /// </summary>
        /// <param name="ch1">The leading character to be examined.</param>
        /// <param name="ch2">The succeeding character to be investigated.</param>
        /// <returns>True if a block comment has been found.</returns>
        public static Boolean IsBlockComment(Char ch1, Char ch2)
        {
            return ch1 == '/' && ch2 == '*';
        }

        #endregion

        #region General Stuff

        /// <summary>
        /// Transforms the contained expression blocks into one string.
        /// </summary>
        /// <returns>The string representation of the parser content.</returns>
        public override String ToString()
        {
            var sb = new StringBuilder();

            foreach (var statement in statements)
                sb.Append("::").AppendLine(statement.ToString());

            return sb.ToString();
        }

        #endregion
    }
}
