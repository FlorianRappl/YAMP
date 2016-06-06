using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TUnion function.")]
    [Kind(PopularKinds.Function)]
    sealed class TUnionFunction : ArgumentFunction
	{
        [Description("Create a new Set, with the Union of Sets")]
        [Arguments(1, 1)]
        public SetValue Function(SetValue set1, ArgumentsValue args)
        {
            return SetValue.TUnion(set1, args);
        }

	}
}

