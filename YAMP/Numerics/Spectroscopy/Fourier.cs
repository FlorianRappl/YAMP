using System;
using YAMP;
using System.Collections.Generic;

namespace YAMP.Numerics
{
    /// <summary>
    /// A more advanced FFT that is a lot more general.
    /// </summary>
    public sealed class Fourier
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the Fourier transformer.
        /// </summary>
        /// <param name="size">The series length of the transformer, which must be positive.</param>
        public Fourier(int size)
            : this(size, FourierSign.Negative, FourierNormalization.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Fourier transformer with the given sign and normalization conventions.
        /// </summary>
        /// <param name="size">The series length of the transformer, which must be positive.</param>
        /// <param name="signConvention">The sign convention of the transformer.</param>
        /// <param name="normalizationConvention">The normalization convention of the transformer.</param>
        public Fourier(int size, FourierSign signConvention, FourierNormalization normalizationConvention)
        {
            if (size < 1)
                throw new YAMPArgumentRangeException("size", 0);

            this.size = size;
            this.signConvention = signConvention;
            this.normalizationConvention = normalizationConvention;

            // pre-compute the Nth complex roots of unity
            this.roots = Helpers.ComputeRoots(size, +1);

            // decompose the size into prime factors
            this.factors = Factors(size);

            // store a plan for the transform based on the prime factorization
            plan = new List<Transformlet>();

            foreach (Factor factor in factors)
            {
                Transformlet t;
                switch (factor.Value)
                {
                    // use a radix-specialized transformlet when available
                    case 2:
                        t = new RadixTwoTransformlet(size, roots);
                        break;
                    case 3:
                        t = new RadixThreeTransformlet(size, roots);
                        break;
                    // eventually, we should make an optimized radix-4 transform
                    case 5:
                        t = new RadixFiveTransformlet(size, roots);
                        break;
                    case 7:
                        t = new RadixSevenTransformlet(size, roots);
                        break;
                    case 11:
                    case 13:
                        // the base transformlet is R^2, but when R is small, this can still be faster than the Bluestein algorithm
                        // timing measurements appear to indicate that this is the case for radix 11 and 13
                        // eventually, we should make optimized Winograd transformlets for these factors
                        t = new Transformlet(factor.Value, size, roots);
                        break;
                    default:
                        // for large factors with no available specialized transformlet, use the Bluestein algorithm
                        t = new BluesteinTransformlet(factor.Value, size, roots);
                        break;
                }

                t.Multiplicity = factor.Multiplicity;
                plan.Add(t);
            }
        }

        #endregion

        #region Members

        int size;
        List<Factor> factors;
        List<Transformlet> plan;
        FourierNormalization normalizationConvention;
        FourierSign signConvention;
        ScalarValue[] roots;

        #endregion

        #region Properties

        /// <summary>
        /// The series length for which the transformer is specialized.
        /// </summary>
        public int Length
        {
            get
            {
                return (size);
            }
        }

        /// <summary>
        /// Gets the normalization convention used by the transformer.
        /// </summary>
        public FourierNormalization NormalizationConvention
        {
            get
            {
                return (normalizationConvention);
            }
        }

        /// <summary>
        /// Gets the normalization convention used by the transformer.
        /// </summary>
        public FourierSign SignConvention
        {
            get
            {
                return (signConvention);
            }
        }

        #endregion

        #region Methods

        int GetSign()
        {
            return signConvention == FourierSign.Positive ? 1 : -1;
        }

        static void Normalize(ScalarValue[] x, double f)
        {
            for (int i = 0; i < x.Length; i++)
                x[i] = new ScalarValue(f * x[i].Re, f * x[i].Im);
        }

        internal void Transform(ref ScalarValue[] x, ref ScalarValue[] y, int sign)
        {
            int Ns = 1;

            foreach (Transformlet t in plan)
            {
                for (int k = 0; k < t.Multiplicity; k++)
                {
                    t.FftPass(x, y, Ns, sign);
                    var temp = x;
                    x = y;
                    y = temp;
                    Ns *= t.Radix;
                }
            }
        }

        /// <summary>
        /// Computes the Fourier transform of the given series.
        /// </summary>
        /// <param name="values">The series to transform.</param>
        /// <returns>The discrete Fourier transform of the series.</returns>
        public ScalarValue[] Transform(IList<ScalarValue> values)
        {
            if (values == null) 
                throw new ArgumentNullException("values");

            if (values.Count != size)
                throw new YAMPDifferentLengthsException(values.Count, size);

            // copy the original values into a new array
            var x = new ScalarValue[size];
            values.CopyTo(x, 0);

            // normalize the copy appropriately
            if (normalizationConvention == FourierNormalization.Unitary)
                Normalize(x, 1.0 / Math.Sqrt(size));
            else if (normalizationConvention == FourierNormalization.Inverse)
                Normalize(x, 1.0 / size);

            // create a scratch array
            var y = new ScalarValue[size];

            // do the FFT
            Transform(ref x, ref y, GetSign());
            return x;
        }

        /// <summary>
        /// Computes the inverse Fourier transform of the given series.
        /// </summary>
        /// <param name="values">The series to invert.</param>
        /// <returns>The inverse discrete Fourier transform of the series.</returns>
        public ScalarValue[] InverseTransform(IList<ScalarValue> values)
        {
            if (values == null) 
                throw new ArgumentNullException("values");

            if (values.Count != size)
                throw new YAMPDifferentLengthsException(values.Count, size);

            // copy the original values into a new array
            var x = new ScalarValue[size];
            values.CopyTo(x, 0);

            // normalize the copy appropriately
            if (normalizationConvention == FourierNormalization.None)
                Normalize(x, 1.0 / size);
            else if (normalizationConvention == FourierNormalization.Unitary)
                Normalize(x, 1.0 / Math.Sqrt(size));

            // create a scratch array
            var y = new ScalarValue[size];

            // do the FFT
            Transform(ref x, ref y, -GetSign());
            return x;
        }

        #endregion

        #region Helpers

        static List<Factor> Factors(int n)
        {
            if (n < 1) 
                throw new YAMPArgumentRangeException("n", 0);

            List<Factor> factors = new List<Factor>();

            if (n > 1) 
                FactorByTrialDivision(factors, ref n);

            if (n > 1) 
                FactorByPollardsRhoMethod(factors, ref n);

            if (n > 1) 
                factors.Add(new Factor(n, 1));

            return (factors);
        }

        static void FactorByTrialDivision(List<Factor> factors, ref int n)
        {
            var smallPrimes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };

            foreach (int p in smallPrimes)
            {

                int m = 0;

                while (n % p == 0)
                {
                    n = n / p;
                    m++;
                }

                if (m > 0) 
                    factors.Add(new Factor(p, m));

                if (n == 1)
                    return;
            }
        }

        static void FactorByPollardsRhoMethod(List<Factor> factors, ref int n)
        {
            int x = 5; 
            int y = 2; 
            int k = 1; 
            int l = 1;

            for (int c = 0; c < 250; c++)
            {
                int g = Helpers.GCD(Math.Abs(y - x), n);

                if (g == n)
                    return;
                else if (g == 1)
                {
                    k--;

                    if (k == 0)
                    {
                        y = x;
                        l = 2 * l;
                        k = l;
                    }

                    x = Helpers.PowMod(x, 2, n) + 1;
                    if (x == n) x = 0;
                }
                else
                {
                    int m = 0;

                    while (n % g == 0)
                    {
                        n = n / g;
                        x = x % n;
                        y = y % n;
                        m++;
                    }

                    factors.Add(new Factor(g, m));
                }
            }
        }

        #endregion

        #region Factor

        struct Factor 
        {
            public Factor (int value, int multiplicity)
            {
                this.value = value;
                this.multiplicity = multiplicity;
            }

            int value, multiplicity;

            public int Value 
            {
                get { return (value); }
            }

            public int Multiplicity 
            {
                get { return (multiplicity); }
            }
        }

        #endregion
    }
}
