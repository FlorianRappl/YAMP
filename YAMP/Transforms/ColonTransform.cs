using System;

namespace YAMP
{
    class ColonTransform : Transform
    {
        public ColonTransform() : base(';')
        {
        }

        public override string Modify(ParseContext context, string original, Expression premise)
        {
            //TODO
            return string.Empty;
        }

        public override bool WillTransform(Expression premise)
        {
            return (premise == null);
        }
    }
}
