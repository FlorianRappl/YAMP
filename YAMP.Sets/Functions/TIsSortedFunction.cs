using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TIsSorted function.")]
    [Kind(PopularKinds.Function)]
    sealed class TIsSortedFunction : ArgumentFunction
	{
        [Description("Determines whether set is of Sorted type")]
        public ScalarValue Function(SetValue set1)
        {
            bool eq = set1.Sorted;
            return new ScalarValue(eq);
        }
	}
}

