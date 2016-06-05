using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIsSubsetOf function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIsSubsetOfFunction : ArgumentFunction
	{
        [Description("Determines whether set1 is a subset of set2")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.IsSubsetOf(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

