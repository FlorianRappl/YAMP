using System;

namespace YAMP
{
    [Description("In statistics the Jackknife is a method to estimate the statistical error of observables measured on a set of data.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Resampling_(statistics)")]
    sealed class JackknifeFunction : SystemFunction
    {
        [Description("This function implements the blocked Jackknife for functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument and has to return either a scalar or a matrix.")]
        [Example("Jackknife([3 + randn(100, 1), 10 + 2 * randn(100, 1)], 20, avg)", "Gives the statistical Jackknife estimate for the mean and the error on the mean of a dataset with 100 measurements, mean [3, 10], and gaussian noise of width [1, 4] for a blocksize of 20.")]
        public MatrixValue Function(MatrixValue cfgs, ScalarValue n, FunctionValue f)
        {
            return Function(cfgs, n, f, new ArgumentsValue());
        }

        [Description("This function implements the blocked Jackknife for functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument, can take constant other optional arguments, and has to return either a scalar or a matrix.")]
        [Arguments(3, 1)]
        public MatrixValue Function(MatrixValue cfgs, ScalarValue n, FunctionValue f, ArgumentsValue P)
        {
            var numberOfBlocks = n.GetIntegerOrThrowException("n", Name);
            var nConfigs = cfgs.DimensionY;
            var nData = cfgs.DimensionX;

            if (numberOfBlocks > nConfigs)
                throw new YAMPException("Jackknife: The number of measurements n is greater than the number of configurations cfgs!");

            if (numberOfBlocks <= 1)
                throw new YAMPException("Jackknife: The number of measurements n <= 1!");

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
                throw new YAMPException("Jackknife: Observable f has to return either a scalar or a matrix!");

            var JackknifeObservable = new MatrixValue(numberOfBlocks, nResult);
            var BlockSize = nConfigs / numberOfBlocks;
            var nConfigsBlocked = BlockSize * numberOfBlocks;
            var residualConfigs = nConfigs - nConfigsBlocked;

            for (int i = 1; i <= numberOfBlocks; i++)
            {
                if (i <= numberOfBlocks - residualConfigs)
                {
                    //the first (NumberOfBlocks - residualConfigs) blocks discard (BlockSize) elements ...
                    var JackknifeConfigs = new MatrixValue(nConfigs - BlockSize, nData);
                    int j = 1;

                    for (; j <= (i - 1) * BlockSize; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j, k] = cfgs[j, k];

                    j += BlockSize;

                    for (; j <= nConfigs; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j - BlockSize, k] = cfgs[j, k];

                    parameters = new ArgumentsValue(JackknifeConfigs);
                }
                else
                {
                    //... whereas the residual (residualConfigs) blocks discard (BlockSize + 1) elements
                    var JackknifeConfigs = new MatrixValue(nConfigs - BlockSize - 1, nData);
                    int j = 1;

                    for (; j <= nConfigs - (numberOfBlocks - (i - 1)) * (BlockSize + 1); j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j, k] = cfgs[j, k];

                    j += BlockSize + 1;

                    for (; j <= nConfigs; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j - BlockSize - 1, k] = cfgs[j, k];

                    parameters = new ArgumentsValue(JackknifeConfigs);
                }

                foreach (var m in P.Values)
                    parameters.Insert(m);

                temp = f.Perform(Context, parameters);

                if (temp is ScalarValue)
                    JackknifeObservable[i] = (ScalarValue)temp;
                else
                {
                    var T = (MatrixValue)temp;

                    for (int k = 1; k <= nResult; k++)
                        JackknifeObservable[i, k] = T[k];
                }
            }

            temp = YMath.Average(JackknifeObservable);

            for (int i = 1; i <= numberOfBlocks; i++)
            {
                if (temp is ScalarValue)
                {
                    JackknifeObservable[i] -= temp as ScalarValue;
                    JackknifeObservable[i] *= JackknifeObservable[i];
                }
                else
                {
                    var m = (MatrixValue)temp;

                    for (int k = 1; k <= nResult; k++)
                    {
                        JackknifeObservable[i, k] -= m[k];
                        JackknifeObservable[i, k] *= JackknifeObservable[i, k];
                    }
                }
            }

            var error = YMath.Average(JackknifeObservable);
            var scale = numberOfBlocks - 1.0;

            if (error is ScalarValue)
                error = ((ScalarValue)error) * scale;
            else
            {
                var e = (MatrixValue)error;

                for (var i = 1; i <= e.DimensionY; i++)
                    for (int j = 1; j <= e.DimensionX; j++)
                        e[i, j] *= scale;
            }

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