using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIsSupersetOf function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIsSupersetOfFunction : ArgumentFunction
	{
        [Description("Determines whether set1 is a superset of set2")]
        public ScalarValue Function(SetValue set1, SetValue set2)
        {
            bool eq = set1.Set.IsSupersetOf(set2.Set);
            return new ScalarValue(eq);
        }

	}
}

