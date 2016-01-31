namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The variable name missing error.
    /// </summary>
    public class YAMPVariableNameMissing : YAMPParseError
    {
        internal YAMPVariableNameMissing(Int32 line, Int32 column)
            : base(line, column, "The let keyword can only be used in combination with a valid name for the variable.")
        {
        }

        internal YAMPVariableNameMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}

