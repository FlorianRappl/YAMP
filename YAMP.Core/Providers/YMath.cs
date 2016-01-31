namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    /// <summary>
    /// Capsulates an ensemble of internally (frequently) used math functions.
    /// </summary>
    static class YMath
    {
        public static Value Average(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return ScalarValue.Zero;
            }

            if (M.IsVector)
            {
                var q = ScalarValue.Zero;

                for (var i = 1; i <= M.Length; i++)
                {
                    q += M[i];
                }

                return q / M.Length;
            }

            var scale = 1.0;
            var s = new MatrixValue(1, M.DimensionX);

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    s[1, j] += M[i, j];
                }
            }

            scale /= M.DimensionY;

            for (var j = 1; j <= s.DimensionX; j++)
                s[1, j] *= scale;

            return s;
        }

        public static MatrixValue Histogram(MatrixValue v, Double[] centers)
        {
            if (centers.Length == 0)
                throw new YAMPWrongLengthException(0, 1);

            var H = new MatrixValue(centers.Length, 1);
            var N = new int[centers.Length];
            var last = centers.Length - 1;

            for (var i = 1; i <= v.Length; i++)
            {
                var y = v[i].Re;

                if (y < centers[0])
                {
                    N[0]++;
                }
                else if (y > centers[last])
                {
                    N[last]++;
                }
                else
                {
                    var min = Double.MaxValue;
                    var index = 0;

                    for (var j = 0; j < centers.Length; j++)
                    {
                        var dist = Math.Abs(y - centers[j]);

                        if (dist < min)
                        {
                            index = j;
                            min = dist;
                        }
                    }

                    N[index]++;
                }
            }

            for (var i = 1; i <= centers.Length; i++)
            {
                H[i, 1] = new ScalarValue(N[i - 1]);
            }

            return H;
        }

        public static MatrixValue Histogram(MatrixValue v, Int32 nbins)
        {
            var min = Double.MaxValue;
            var max = Double.MinValue;

            for (var i = 1; i <= v.Length; i++)
            {
                if (v[i].Re > max)
                {
                    max = v[i].Re;
                }

                if (v[i].Re < min)
                {
                    min = v[i].Re;
                }
            }

            var delta = (max - min) / nbins;
            var D = new Double[nbins];

            for (var i = 0; i < nbins; i++)
            {
                D[i] = delta * (i + 0.5) + min;
            }

            return Histogram(v, D);
        }

        public static Value Mean(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return ScalarValue.Zero;
            }

            if (M.IsVector)
            {
                var q = ScalarValue.One;

                for (var i = 1; i <= M.Length; i++)
                {
                    q *= M[i];
                }

                return q.Pow(new ScalarValue(1.0 / M.Length));
            }

            var s = new MatrixValue(1, M.DimensionX);

            for (var i = 1; i < M.DimensionX; i++)
            {
                s[1, i] = ScalarValue.One;
            }

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    s[1, j] *= M[i, j];
                }
            }

            for (var j = 1; j <= s.DimensionX; j++)
            {
                s[1, j] = s[1, j].Pow(new ScalarValue(1.0 / M.DimensionY));
            }

            return s;
        }

        public static Value HarmonicMean(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return ScalarValue.Zero;
            }

            if (M.IsVector)
            {
                var q = ScalarValue.Zero;

                for (var i = 1; i <= M.Length; i++)
                {
                    q += (1.0 / M[i]);
                }

                return M.Length / q;
            }

            var s = new MatrixValue(1, M.DimensionX);

            for (var i = 1; i < M.DimensionX; i++)
            {
                s[1, i] = ScalarValue.Zero;
            }

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    s[1, j] += (1.0 / M[i, j]);
                }
            }

            for (var j = 1; j <= s.DimensionX; j++)
            {
                s[1, j] = (M.DimensionY / s[1, j]);
            }

            return s;
        }

        public static MatrixValue Covariance(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return new MatrixValue();
            }

            if (M.IsVector)
            {
                return new MatrixValue(1, 1, Variance(M) as ScalarValue);
            }

            var avg = Average(M) as MatrixValue;
            var scale = 1.0;
            var s = new MatrixValue(M.DimensionX, M.DimensionX);

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    for (var k = 1; k <= M.DimensionX; k++)
                    {
                        s[k, j] += (M[i, j] - avg[j]) * (M[i, k] - avg[k]);
                    }
                }
            }

            scale /= M.DimensionY;

            for (var i = 1; i <= s.DimensionY; i++)
            {
                for (var j = 1; j <= s.DimensionX; j++)
                {
                    s[i, j] *= scale;
                }
            }

            return s;
        }

        public static MatrixValue Correlation(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return new MatrixValue();
            }

            var result = Covariance(M);

            for (var i = 1; i <= M.DimensionX; i++)
            {
                var temp = 1 / result[i, i].Sqrt();

                for (var j = 1; j <= M.DimensionX; j++)
                {
                    result[i, j] *= temp;
                    result[j, i] *= temp;
                }
            }

            return result;
        }

        public static MatrixValue CrossCorrelation(MatrixValue M, MatrixValue N, int n)
        {
            var result = new MatrixValue(1, n + 1);
            var avgM = (ScalarValue)Average(M);
            var avgN = (ScalarValue)Average(N);
            var errM = ((ScalarValue)Variance(M)).Sqrt();
            var errN = ((ScalarValue)Variance(N)).Sqrt();
            var length = M.Length;

            for (var i = 0; i <= n; i++)
            {
                var scale = 1.0 / ((length - i) * errM * errN);

                for (var j = 1; j <= length - i; j++)
                {
                    result[i + 1] += (M[j] - avgM) * (N[j + i] - avgN);
                }

                result[i + 1] *= scale;
            }

            return result;
        }

        public static Value Variance(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return ScalarValue.Zero;
            }

            if (M.IsVector)
            {
                var variance = ScalarValue.Zero;
                var mean = M.Sum() / M.Length;

                for (var i = 1; i <= M.Length; i++)
                {
                    variance += (M[i] - mean).Square();
                }

                return variance / M.Length;
            }

            var avg = (MatrixValue)YMath.Average(M);
            var scale = 1.0;
            var s = new MatrixValue(1, M.DimensionX);

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    s[1, j] += (M[i, j] - avg[j]).Square();
                }
            }

            scale /= M.DimensionY;

            for (var i = 1; i <= s.DimensionY; i++)
            {
                for (var j = 1; j <= s.DimensionX; j++)
                {
                    s[i, j] *= scale;
                }
            }

            return s;
        }

        public static ScalarValue Median(MatrixValue M)
        {
            if (M.Length == 0)
            {
                return ScalarValue.Zero;
            }
            else if (M.Length == 1)
            {
                return M[1];
            }

            M = M.VectorSort();
            var midPoint = 0;
            var sum = ScalarValue.Zero;

            if (M.Length % 2 == 1)
            {
                midPoint = M.Length / 2;
                sum = M[midPoint + 1];
            }
            else
            {
                midPoint = (M.Length / 2);
                sum = M[midPoint] + M[midPoint + 1];
                sum /= 2.0;
            }

            return sum;
        }
    }
}
