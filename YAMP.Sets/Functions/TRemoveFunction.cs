using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TRemove function.")]
    [Kind(PopularKinds.Function)]
    sealed class TRemoveFunction : ArgumentFunction
	{
        [Description("Removes the specified element from the Set, and returns the set. If Matrix, all its elements will be removed")]
        public SetValue Function(SetValue set1, Value id)
        {
            if (id is MatrixValue)
            {
                set1.RemoveElements((id as MatrixValue).ToArray());
            }
            else
            {
                set1.Set.Remove(new SetValue.ValueWrap(id));
            }

            return set1;
        }

	}
}

