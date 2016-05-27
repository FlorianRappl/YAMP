using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The Except function.")]
    [Kind(PopularKinds.Function)]
    sealed class ExceptFunction : ArgumentFunction
	{
        [Description("Create a new Set, with the Except of the two Sets")]
        public SetValue Function(SetValue set1, SetValue set2)
        {
            string newName = string.Format("({0}-{1})", set1.Name, set2.Name);

            var newSet = set1.Copy() as SetValue;
            newSet.Set.ExceptWith(set2.Set);
            newSet.Name = newName;

            return newSet;
        }

	}
}

