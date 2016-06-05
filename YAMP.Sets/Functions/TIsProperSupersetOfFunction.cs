using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIsProperSupersetOf function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIsProperSupersetOfFunction : ArgumentFunction
	{
        [Description("Determines whether set1 is a proper superset of set2")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.IsProperSupersetOf(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

