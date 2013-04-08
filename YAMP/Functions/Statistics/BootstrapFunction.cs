using System;

namespace YAMP
{
    [Description("In statistics the Bootstrap is a method to estimate the statistical error of observables measured on a set of data.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Bootstrapping")]
    sealed class BootstrapFunction : SystemFunction
    {
        [Description("This function implements the Bootstrap method for functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument and has to return either a scalar or a matrix.")]
        [Example("Bootstrap([3 + randn(100, 1), 10 + 2 * randn(100, 1)], 200, avg)", "Gives the statistical Bootstrap estimate for the mean and the error on the mean of a dataset with 100 measurements, mean [3, 10], and gaussian noise of width [1, 4] for 200 bootstrap samples.")]
        public MatrixValue Function(MatrixValue cfgs, ScalarValue n, FunctionValue f)
        {
            return Function(cfgs, n, f, new ArgumentsValue());
        }

        [Description("This function implements the Bootstrap method for functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument, can take constant other optional arguments, and has to return either a scalar or a matrix.")]
        [Example("Bootstrap([3 + randn(100, 1), 10 + 2 * randn(100, 1)], 200, avg)", "Gives the statistical Bootstrap estimate for the mean and the error on the mean of a dataset with 100 measurements, mean [3, 10], and gaussian noise of width [1, 4] for 200 bootstrap samples.")]
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
                parameters.Insert(m);

            var temp = f.Perform(Context, parameters);
            int nResult;//dimension of the result

            if (temp is ScalarValue)
                nResult = 1;
            else if (temp is MatrixValue)
                nResult = (temp as MatrixValue).Length;
            else
                throw new YAMPException("Bootstrap: The observable f has to return either a scalar or a matrix!");

            var BootstrapObservable = new MatrixValue(numberOfBootstrapSamples, nResult);
            var rand = RandiFunction.Generator;

            for (int i = 1; i <= numberOfBootstrapSamples; i++)
            {
                var BootstrapConfigs = new MatrixValue(nConfigs, nData);
                rand.Beta = nConfigs;
                rand.Alpha = 1;

                for (int j = 1; j <= nConfigs; j++)
                {
                    int idx = rand.Next();

                    for (int k = 1; k <= nData; k++)
                        BootstrapConfigs[j, k] = cfgs[idx, k];
                }

                parameters = new ArgumentsValue(BootstrapConfigs);

                foreach (var m in P.Values)
                    parameters.Insert(m);

                temp = f.Perform(Context, parameters);

                if (temp is ScalarValue)
                    BootstrapObservable[i] = (ScalarValue)temp;
                else
                {
                    var m = (MatrixValue)temp;

                    for (int k = 1; k <= nResult; k++)
                        BootstrapObservable[i, k] = m[k];
                }
            }

            temp = YMath.Average(BootstrapObservable);

            for (int i = 1; i <= numberOfBootstrapSamples; i++)
            {
                if (temp is ScalarValue)
                {
                    BootstrapObservable[i] -= temp as ScalarValue;
                    BootstrapObservable[i] *= BootstrapObservable[i];
                }
                else
                {
                    var T = temp as MatrixValue;

                    for (int k = 1; k <= nResult; k++)
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

                for (int k = 1; k <= nResult; k++)
                {
                    result[1, k] = T[k];
                    result[2, k] = E[k];
                }
            }

            return result;
        }
    }
}