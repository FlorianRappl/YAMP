using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIsProperSubsetOf function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIsProperSubsetOfFunction : ArgumentFunction
	{
        [Description("Determines whether set1 is a proper subset of set2")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.IsProperSubsetOf(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

