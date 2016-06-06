using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The TExcept function.")]
    [Kind(PopularKinds.Function)]
    sealed class TExceptFunction : ArgumentFunction
	{
        [Description("Create a new Set, with the first Set Except(ed) of the other Sets")]
        [Arguments(1, 1)]
        public SetValue Function(SetValue set1, ArgumentsValue args)
        {
            return SetValue.TExcept(set1, args);
        }

	}
}

