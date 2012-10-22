using System;

namespace YAMP
{
    class MatrixRowTransform : NewLineTransform
    {
        public override void RegisterToken()
        {
            NewLineTransform.RegisterSpecificType(typeof(MatrixBracketExpression), this);
        }

        public override string Modify(ParseContext context, string original, Expression premise)
        {
            if (original.Length > 0)
                return ";" + original;

            return original;
        }
    }
}
