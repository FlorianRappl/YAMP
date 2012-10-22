using System;
using System.Collections.Generic;

namespace YAMP
{
    class NewLineTransform : Transform
    {
        static Dictionary<Type, NewLineTransform> specifics = new Dictionary<Type, NewLineTransform>();

        public NewLineTransform() : base('\n')
        {
        }

        public static void RegisterSpecificType(Type dependency, NewLineTransform instance)
        {
            specifics.Add(dependency, instance);
        }

        public override string Modify(ParseContext context, string original, Expression premise)
        {
            var index = 0;

            while (index < original.Length)
                if (original[index] != Trigger)
                    break;

            original = original.Substring(index);

            foreach (var key in specifics.Keys)
                if (Fulfills(key, premise))
                    return specifics[key].Modify(context, original, premise);

            return original;
        }
    }
}
