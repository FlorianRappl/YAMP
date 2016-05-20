namespace YAMP
{
    using Numerics;
    using YAMP.Exceptions;

    [Description("BootstrapFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("BootstrapFunctionLink")]
    sealed class BootstrapFunction : SystemFunction
    {
        readonly DiscreteUniformDistribution Distribution = new DiscreteUniformDistribution();

        public BootstrapFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("BootstrapFunctionDescriptionForMatrixScalarFunction")]
        [Example("Bootstrap([3 + randn(100, 1), 10 + 2 * randn(100, 1)], 200, avg)", "BootstrapFunctionExampleForMatrixScalarFunction1")]
        public MatrixValue Function(MatrixValue cfgs, ScalarValue n, FunctionValue f)
        {
            return Function(cfgs, n, f, new ArgumentsValue());
        }

        [Description("BootstrapFunctionDescriptionForMatrixScalarFunctionArguments")]
        [Example("Bootstrap([3 + randn(100, 1), 10 + 2 * randn(100, 1)], 200, avg)", "BootstrapFunctionExampleForMatrixScalarFunctionArguments1")]
        [Arguments(3, 1)]
        public MatrixValue Function(MatrixValue cfgs, ScalarValue n, FunctionValue f, ArgumentsValue P)
        {
            var numberOfBootstrapSamples = n.GetIntegerOrThrowException("n", Name);
            var nConfigs = cfgs.DimensionY;
            var nData = cfgs.DimensionX;

            if (numberOfBootstrapSamples <= 1)
                throw new YAMPException("Bootstrap: The number of bootstrap samples n is smaller or equal to 1!");

            var parameters = new ArgumentsValue(cfgs);

            foreach (var m in P.Values)
            {
                parameters.Insert(m);
            }

            var temp = f.Perform(Context, parameters);
            var nResult = 0;//dimension of the result

            if (temp is ScalarValue)
            {
                nResult = 1;
            }
            else if (temp is MatrixValue)
            {
                nResult = ((MatrixValue)temp).Length;
            }
            else
            {
                throw new YAMPException("Bootstrap: The observable f has to return either a scalar or a matrix!");
            }

            var BootstrapObservable = new MatrixValue(numberOfBootstrapSamples, nResult);
            Distribution.Beta = nConfigs;
            Distribution.Alpha = 1;

            for (var i = 1; i <= numberOfBootstrapSamples; i++)
            {
                var BootstrapConfigs = new MatrixValue(nConfigs, nData);

                for (var j = 1; j <= nConfigs; j++)
                {
                    var idx = Distribution.Next();

                    for (var k = 1; k <= nData; k++)
                    {
                        BootstrapConfigs[j, k] = cfgs[idx, k];
                    }
                }

                parameters = new ArgumentsValue(BootstrapConfigs);

                foreach (var m in P.Values)
                {
                    parameters.Insert(m);
                }

                temp = f.Perform(Context, parameters);

                if (temp is ScalarValue)
                {
                    BootstrapObservable[i] = (ScalarValue)temp;
                }
                else
                {
                    var m = (MatrixValue)temp;

                    for (var k = 1; k <= nResult; k++)
                    {
                        BootstrapObservable[i, k] = m[k];
                    }
                }
            }

            temp = YMath.Average(BootstrapObservable);

            for (var i = 1; i <= numberOfBootstrapSamples; i++)
            {
                if (temp is ScalarValue)
                {
                    BootstrapObservable[i] -= temp as ScalarValue;
                    BootstrapObservable[i] *= BootstrapObservable[i];
                }
                else
                {
                    var T = temp as MatrixValue;

                    for (var k = 1; k <= nResult; k++)
                    {
                        BootstrapObservable[i, k] -= T[k];
                        BootstrapObservable[i, k] *= BootstrapObservable[i, k];
                    }
                }
            }

            var error = YMath.Average(BootstrapObservable);
            var sqrt = new SqrtFunction();
            error = sqrt.Perform(error);
            var result = new MatrixValue(2, nResult);

            if (temp is ScalarValue)
            {
                result[1] = (ScalarValue)temp;
                result[2] = (ScalarValue)error;
            }
            else
            {
                var T = (MatrixValue)temp;
                var E = (MatrixValue)error;

                for (var k = 1; k <= nResult; k++)
                {
                    result[1, k] = T[k];
                    result[2, k] = E[k];
                }
            }

            return result;
        }
    }
}