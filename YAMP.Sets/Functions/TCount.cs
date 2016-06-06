using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TCount function.")]
    [Kind(PopularKinds.Function)]
    sealed class TCountFunction : ArgumentFunction
	{
        [Description("Get the number of elements in the Set")]
        public ScalarValue Function(SetValue set1)
        {
            if (ReferenceEquals(set1, null))
                throw new YAMPArgumentValueException("null", new string[] { "Set" });

            return new ScalarValue(set1.Set.Count);
        }

	}
}

