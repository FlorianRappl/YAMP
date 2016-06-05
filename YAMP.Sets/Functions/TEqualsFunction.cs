using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TEquals function.")]
    [Kind(PopularKinds.Function)]
    sealed class TEqualsFunction : ArgumentFunction
	{
        [Description("Compares two sets")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.SetEquals(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

