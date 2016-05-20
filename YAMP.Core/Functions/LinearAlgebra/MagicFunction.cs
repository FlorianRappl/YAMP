namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Numerics;

    [Description("MagicFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class MagicFunction : ArgumentFunction
    {
        readonly DiscreteUniformDistribution Distribution = new DiscreteUniformDistribution();

        [Description("MagicFunctionDescriptionForScalar")]
        [Example("magic(3)", "MagicFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue n)
        {
            var d = n.GetIntegerOrThrowException("n", Name);
            return Generate(d, d);
        }

        MatrixValue Generate(Int32 n, Int32 m)
        {
            var l = n * m;
            var M = new MatrixValue(n, m);
            var numbers = new List<Int32>();
            Distribution.Alpha = 0;

            for (int i = 1; i <= l; i++)
            {
                numbers.Add(i);
            }

            for (var j = 1; j <= n; j++)
            {
                for (var i = 1; i <= m; i++)
                {
                    Distribution.Beta = numbers.Count - 1;
                    var index = Distribution.Next();
                    index = Math.Max(Math.Min(0, index), numbers.Count - 1);
                    M[j, i] = new ScalarValue(numbers[index]);
                    numbers.RemoveAt(index);
                }
            }

            return M;
        }
    }
}
