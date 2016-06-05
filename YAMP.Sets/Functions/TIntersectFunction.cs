using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIntersect function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIntersectFunction : ArgumentFunction
	{
        [Description("Create a new Set, with the Intersection of Sets")]
        [Arguments(1, 1)]
        public SetValue Function(SetValue set1, ArgumentsValue args)
        {
            return SetValue.TIntersect(set1, args);
        }

	}
}

