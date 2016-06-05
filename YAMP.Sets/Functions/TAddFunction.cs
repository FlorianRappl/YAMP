using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TAdd function.")]
    [Kind(PopularKinds.Function)]
    sealed class TAddFunction : ArgumentFunction
	{
        [Description("Adds the element to the Set, and returns the set. If Matrix, all its elements will be added")]
        public SetValue Function(SetValue set1, Value id)
        {
            if (id is MatrixValue)
            {
                set1.AddElements((id as MatrixValue).ToArray());
            }
            else
            {
                set1.Set.Add(new SetValue.ValueWrap(id));
            }

            return set1;
        }

	}
}

