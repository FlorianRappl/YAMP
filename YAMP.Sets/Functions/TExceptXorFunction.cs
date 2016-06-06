using System;
using YAMP;
using System.Collections.Generic;
using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The TExceptXor (Symmetric Except-XOR) function.")]
    [Kind(PopularKinds.Function)]
    sealed class TExceptXorFunction : ArgumentFunction
	{
        [Description("Create a new Set, with the Symmetric Except(XOR) of all Sets")]
        [Arguments(1, 1)]
        public SetValue Function(SetValue set1, ArgumentsValue args)
        {
            return SetValue.TExceptXor(set1, args);
        }

	}
}

