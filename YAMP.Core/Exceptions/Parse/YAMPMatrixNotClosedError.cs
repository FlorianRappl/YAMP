using System;

namespace YAMP
{
    class YAMPMatrixNotClosedError : YAMPParseError
    {
        public YAMPMatrixNotClosedError(int line, int column) :
            base(line, column, "The matrix (square bracket) starting at line {0}, column {1} has not been properly closed.", line, column)
        {
        }

        public YAMPMatrixNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
