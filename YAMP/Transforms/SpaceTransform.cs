using System;
using System.Collections.Generic;

namespace YAMP
{
    class SpaceTransform : Transform
    {
        public SpaceTransform() : base(' ')
        {
        }

        public override string Modify(ParseContext context, string original, Expression premise)
        {
            var index = 0;

            while (index < original.Length)
                if (original[index] != Trigger)
                    break;

            return original.Substring(index);
        }
    }
}
