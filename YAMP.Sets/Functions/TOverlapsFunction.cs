using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TOverlaps function.")]
    [Kind(PopularKinds.Function)]
    sealed class TOverlapsFunction : ArgumentFunction
	{
        [Description("Determines whether the sets overlap over at least one common element")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.Overlaps(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

