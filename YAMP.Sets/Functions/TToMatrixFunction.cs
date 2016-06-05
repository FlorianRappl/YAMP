using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Sets
{
    [Description("The TToMatrix function.")]
    [Kind(PopularKinds.Function)]
    sealed class TToMatrixFunction : ArgumentFunction
	{
        [Description("Create a single row Matrix with all Numeric keys")]
        [Arguments(1, 1)]
        public MatrixValue Function(SetValue set1)
        {
            return SetValue.TToMatrix(set1);
        }

	}
}

