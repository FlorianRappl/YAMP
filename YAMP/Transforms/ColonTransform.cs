using System;

namespace YAMP
{
    class ColonTransform : Transform
    {
        public ColonTransform() : base(';')
        {
        }

        public override string Modify(QueryContext context, string original, Expression premise)
        {
            context.IsMuted = true;
            return string.Empty;
        }

        public override bool WillTransform(Expression premise)
        {
            return (premise == null);
        }
    }
}
