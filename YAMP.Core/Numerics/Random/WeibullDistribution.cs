/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of weibull distributed random numbers.
	/// </summary>
	/// <remarks>
    /// The implementation of the <see cref="WeibullDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Weibull_distribution">Wikipedia - Weibull distribution</a>.
    /// </remarks>
	public class WeibullDistribution : Distribution
	{
        #region class fields

        /// <summary>
        /// Represents coefficients for the Lanczos approximation of the Gamma function.
        /// </summary>
        static readonly double[] LanczosCoefficients = new double[] { 
            1.000000000190015, 
            76.18009172947146, 
            -86.50532032941677, 
            24.01409824083091, 
            -1.231739572450155, 
            1.208650973866179e-3, 
            -5.395239384953e-6 
        };

        #endregion

        #region instance fields

		/// <summary>
		/// Gets or sets the parameter alpha which is used for generation of weibull distributed random numbers.
		/// </summary>
		/// <remarks>Call <see cref="IsValidAlpha"/> to determine whether a value is valid and therefor assignable.</remarks>
		public double Alpha
		{
			get
			{
				return this.alpha;
			}
			set
			{
                if (this.IsValidAlpha(value))
                {
                    this.alpha = value;
                    this.UpdateHelpers();
                }
        	}
		}

		/// <summary>
		/// Stores the parameter alpha which is used for generation of weibull distributed random numbers.
		/// </summary>
		double alpha;

        /// <summary>
        /// Gets or sets the parameter lambda which is used for generation of erlang distributed random numbers.
        /// </summary>
        /// <remarks>Call <see cref="IsValidLambda"/> to determine whether a value is valid and therefor assignable.</remarks>
        public double Lambda
        {
            get
            {
                return this.lambda;
            }
            set
            {
                if (this.IsValidLambda(value))
                {
                    this.lambda = value;
                }
            }
        }

        /// <summary>
        /// Stores the parameter lambda which is used for generation of erlang distributed random numbers.
        /// </summary>
        double lambda;

        /// <summary>
        /// Stores an intermediate result for generation of weibull distributed random numbers.
        /// </summary>
        /// <remarks>
        /// Speeds up random number generation cause this value only depends on distribution parameters 
        ///   and therefor doesn't need to be recalculated in successive executions of <see cref="NextDouble"/>.
        /// </remarks>
        double helper1;

        #endregion

		#region construction

		/// <summary>
        /// Initializes a new instance of the WeibullDistribution class, using a 
        /// StandardGenerator as underlying random number generator.
		/// </summary>
        public WeibullDistribution()
        {
            this.alpha = 1.0;
            this.lambda = 1.0;
            this.UpdateHelpers();
		}

		/// <summary>
        /// Initializes a new instance of the WeibullDistribution class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public WeibullDistribution(Generator generator)
            : base(generator)
        {
            this.alpha = 1.0;
            this.lambda = 1.0;
            this.UpdateHelpers();
        }

		#endregion
	
		#region instance methods

		/// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsValidAlpha(double value)
		{
			return value > 0.0;
		}
		
		/// <summary>
		/// Determines whether the specified value is valid for parameter <see cref="Lambda"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
		/// </returns>
        public bool IsValidLambda(double value)
		{
			return value > 0.0;
		}

        /// <summary>
        /// Updates the helper variables that store intermediate results for generation of weibull distributed random 
        ///   numbers.
        /// </summary>
        void UpdateHelpers()
        {
            this.helper1 = 1.0 / this.alpha;
        }

        /// <summary>
        /// Represents a Lanczos approximation of the Gamma function.
        /// </summary>
        /// <param name="x">A double-precision floating point number.</param>
        /// <returns>
        /// A double-precision floating point number representing an approximation of Gamma(<paramref name="x"/>).
        /// </returns>
        double Gamma(double x)
        {
            double sum = WeibullDistribution.LanczosCoefficients[0];

            for (int index = 1; index <= 6; index++)
                sum += WeibullDistribution.LanczosCoefficients[index] / (x + index);

            return Math.Sqrt(2.0 * Math.PI) / x * Math.Pow(x + 5.5, x + 0.5) / Math.Exp(x + 5.5) * sum;
        }

        #endregion

		#region overridden Distribution members

        /// <summary>
		/// Gets the minimum possible value of weibull distributed random numbers.
		/// </summary>
		public override double Minimum
		{
			get
			{
				return 0.0;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of weibull distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return double.MaxValue;
			}
		}

		/// <summary>
		/// Gets the mean value of weibull distributed random numbers.
		/// </summary>
        public override double Mean
		{
			get
			{
                return this.lambda * this.Gamma(1.0 + 1.0 / this.alpha);
			}
		}

        /// <summary>
		/// Gets the median of weibull distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
                return this.lambda * Math.Pow(Math.Log(2.0), 1.0 / this.alpha);
			}
		}
		
		/// <summary>
		/// Gets the variance of weibull distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
                return Math.Pow(this.lambda, 2.0) * this.Gamma(1.0 + 2.0 / this.alpha) - Math.Pow(this.Mean, 2.0);
			}
		}
		
		/// <summary>
		/// Gets the mode of weibull distributed random numbers.
		/// </summary>
        public override double[] Mode
		{
			get
			{
                if (this.alpha >= 1.0)
                    return new double[] { this.lambda * Math.Pow(1.0 - 1.0 / this.alpha, 1.0 / this.alpha) };
                
                return new double[] { 0.0 };
            }
		}
		
		/// <summary>
		/// Returns a weibull distributed floating point random number.
		/// </summary>
		/// <returns>A weibull distributed double-precision floating point number.</returns>
        public override double NextDouble()
		{
            // Subtract random number from 1.0 to avoid Math.Log(0.0)
            return this.lambda * Math.Pow(-Math.Log(1.0 - this.Generator.NextDouble()), this.helper1);
		}

		#endregion
	}
}