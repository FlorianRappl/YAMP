using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TContains function.")]
    [Kind(PopularKinds.Function)]
    sealed class TContainsFunction : ArgumentFunction
	{
        [Description("Determines whether the set contains the given element's id")]
        public ScalarValue Function(SetValue set1, Value id)
        {
            bool eq = set1.Set.Contains(new SetValue.ValueWrap(id));
            return new ScalarValue(eq);
        }

	}
}

