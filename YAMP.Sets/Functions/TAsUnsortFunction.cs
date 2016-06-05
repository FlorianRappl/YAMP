using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The TAsUnsort function.")]
    [Kind(PopularKinds.Function)]
    sealed class TAsUnsortFunction : ArgumentFunction
	{
        [Description("Creates a copied unsorted Set")]
        public SetValue Function(SetValue set)
        {
            var newSet = new SetValue(set.Name, set.Set, false);

            return newSet;
        }

	}
}

