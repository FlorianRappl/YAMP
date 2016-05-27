using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The UnsortSet function.")]
    [Kind(PopularKinds.Function)]
    sealed class UnsortSetFunction : ArgumentFunction
	{
        [Description("Creates a copied unsorted Set")]
        public SetValue Function(SetValue set)
        {
            var newSet = new SetValue(set.Name, set.Set, false);

            return newSet;
        }

	}
}

