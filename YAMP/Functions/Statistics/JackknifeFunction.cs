using System;

namespace YAMP
{
    [Description("In statistics the Jackknife is a method to estimate the statistical error of observables measured on a set of data.")]
    [Kind(PopularKinds.Function)]
    class JackknifeFunction : SystemFunction
    {
        [Description("This function implements the blocked Jackknife for a functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument and has to return either a ScalarValue or a MatrixValue.")]
        public MatrixValue Function(MatrixValue Configs, ScalarValue numberOfBlocks, FunctionValue Observable)
        {
            return Function(Configs, numberOfBlocks, Observable, new ArgumentsValue());
        }

        [Description("This function implements the blocked Jackknife for a functions of a set of vector data saved as the rows of a matrix. The function has to take a matrix of data as the first argument, can take constant other optional arguments, and has to return either a ScalarValue or a MatrixValue.")]
        [Example("Jackknife([3+randn(100,1),10+2*randn(100,1)], 20, avg)", "Gives the statistical Jackknife estimate for the mean and the error on the mean of a dataset with 100 measurements, mean [3,10], and gaussian noise of width [1,4] for a blocksize of 20.")]
        [Arguments(3, 1)]
        public MatrixValue Function(MatrixValue Configs, ScalarValue numberOfBlocks, FunctionValue Observable, ArgumentsValue Parameters)
        {
            int NumberOfBlocks = numberOfBlocks.IntValue;
            int nConfigs = Configs.DimensionY;
            int nData = Configs.DimensionX;

            if (NumberOfBlocks > nConfigs)
                throw new YAMPException("Jackknife: NumberOfBlocks > nConfigs!");

            if (NumberOfBlocks <= 1)
                throw new YAMPException("Jackknife: NumberOfBlocks <= 1!");

            var parameters = new ArgumentsValue(Configs);

            foreach (var m in Parameters.Values)
                parameters.Insert(m);

            var temp = Observable.Perform(Context, parameters);
            int nResult;//dimension of the result

            if (temp is ScalarValue)
                nResult = 1;
            else if (temp is MatrixValue)
                nResult = (temp as MatrixValue).Length;
            else
                throw new YAMPException("Jackknife: Observable has to return either ScalarValue or MatrixValue!");

            var JackknifeObservable = new MatrixValue(NumberOfBlocks, nResult);
            int BlockSize = nConfigs / NumberOfBlocks;
            int nConfigsBlocked = BlockSize * NumberOfBlocks;
            int residualConfigs = nConfigs - nConfigsBlocked;

            for (int i = 1; i <= NumberOfBlocks; i++)
            {
                MatrixValue JackknifeConfigs;

                if (i <= NumberOfBlocks - residualConfigs)
                {
                    //the first (NumberOfBlocks - residualConfigs) blocks discard (BlockSize) elements ...
                    JackknifeConfigs = new MatrixValue(nConfigs - BlockSize, nData);
                    int j = 1;

                    for (; j <= (i - 1) * BlockSize; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j, k] = Configs[j, k];

                    j += BlockSize;

                    for (; j <= nConfigs; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j - BlockSize, k] = Configs[j, k];
                }
                else
                {
                    //... whereas the residual (residualConfigs) blocks discard (BlockSize + 1) elements
                    JackknifeConfigs = new MatrixValue(nConfigs - BlockSize - 1, nData);
                    int j = 1;

                    for (; j <= nConfigs - (NumberOfBlocks - (i - 1)) * (BlockSize + 1); j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j, k] = Configs[j, k];

                    j += BlockSize + 1;

                    for (; j <= nConfigs; j++)
                        for (int k = 1; k <= nData; k++)
                            JackknifeConfigs[j - BlockSize - 1, k] = Configs[j, k];
                }

                parameters = new ArgumentsValue(JackknifeConfigs);

                foreach (var m in Parameters.Values)
                    parameters.Insert(m);

                temp = Observable.Perform(Context, parameters);

                if (temp is ScalarValue)
                    JackknifeObservable[i] = (ScalarValue)temp;
                else
                {
                    var m = (MatrixValue)temp;

                    for (int k = 1; k <= nResult; k++)
                        JackknifeObservable[i, k] = m[k];
                }
            }

            temp = AvgFunction.Average(JackknifeObservable);

            for (int i = 1; i <= NumberOfBlocks; i++)
            {
                if (temp is ScalarValue)
                {
                    JackknifeObservable[i] -= temp as ScalarValue;
                    JackknifeObservable[i] *= JackknifeObservable[i];
                }
                else
                {
                    for (int k = 1; k <= nResult; k++)
                    {
                        JackknifeObservable[i, k] -= (temp as MatrixValue)[k];
                        JackknifeObservable[i, k] *= JackknifeObservable[i, k];
                    }
                }
            }

            var error = AvgFunction.Average(JackknifeObservable);

            double scale = NumberOfBlocks - 1;

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
                for (int k = 1; k <= nResult; k++)
                {
                    result[1, k] = ((MatrixValue)temp)[k];
                    result[2, k] = ((MatrixValue)error)[k];
                }
            }

            return result;
        }
    }
}