using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The SortSet function.")]
    [Kind(PopularKinds.Function)]
    sealed class SortSetFunction : ArgumentFunction
	{
        [Description("Creates a copied sorted Set")]
        public SetValue Function(SetValue set)
        {
            var newSet = new SetValue(set.Name, set.Set, true);

            return newSet;
        }

	}
}

