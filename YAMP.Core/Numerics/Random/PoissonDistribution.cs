/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of poisson distributed random numbers.
	/// </summary>
	/// <remarks>
	/// The poisson distribution generates only discrete numbers.<br />
    /// The implementation of the <see cref="PoissonDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Poisson_distribution">Wikipedia - Poisson distribution</a>
    ///   and the implementation in the <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">
    ///   Communication Networks Class Library</a>.
    /// </remarks>
	public class PoissonDistribution : Distribution
	{
		#region instance fields
		/// <summary>
        /// Gets or sets the parameter lambda which is used for generation of poisson distributed random numbers.
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
                    this.UpdateHelpers();
                }
            }
        }

		/// <summary>
        /// Stores the the parameter lambda which is used for generation of poisson distributed random numbers.
		/// </summary>
		private double lambda;

        /// <summary>
        /// Stores an intermediate result for generation of poisson distributed random numbers.
        /// </summary>
        /// <remarks>
        /// Speeds up random number generation cause this value only depends on distribution parameters 
        ///   and therefor doesn't need to be recalculated in successive executions of <see cref="NextDouble"/>.
        /// </remarks>
        private double helper1;
		#endregion

		#region construction

		/// <summary>
        /// Initializes a new instance of the PoissonDistribution class, using a 
        /// StandardGenerator as underlying random number generator.
		/// </summary>
        public PoissonDistribution()
        {
            this.lambda = 1.0;
            this.UpdateHelpers();
		}
		
		/// <summary>
        /// Initializes a new instance of the PoissonDistribution class, using the specified 
        /// Generator as underlying random number generator.
        /// </summary>
        /// <param name="generator">A Generator object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public PoissonDistribution(Generator generator)
            : base(generator)
        {
            this.lambda = 1.0;
            this.UpdateHelpers();
        }

		#endregion
		
		#region instance methods
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
        /// Updates the helper variables that store intermediate results for generation of beta distributed random 
        ///   numbers.
        /// </summary>
        private void UpdateHelpers()
        {
            this.helper1 = Math.Exp(-1.0 * this.lambda);
        }

        /// <summary>
        /// Returns a poisson distributed random number.
        /// </summary>
        /// <returns>A poisson distributed 32-bit signed integer.</returns>
        public int Next()
        {
            int count = 0;
            for (double product = this.Generator.NextDouble(); product >= this.helper1; product *= this.Generator.NextDouble())
            {
                count++;
            }

            return count;
        }
        #endregion

		#region overridden Distribution members
        /// <summary>
		/// Gets the minimum possible value of poisson distributed random numbers.
		/// </summary>
        public override double Minimum
		{
			get
			{
				return 0.0;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of poisson distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return double.MaxValue;
			}
		}

		/// <summary>
		/// Gets the mean value of poisson distributed random numbers. 
		/// </summary>
        public override double Mean
		{
			get
			{
				return this.lambda;
			}
		}
		
		/// <summary>
		/// Gets the median of poisson distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
				return double.NaN;
			}
		}
		
		/// <summary>
		/// Gets the variance of poisson distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
				return this.lambda;
			}
		}
		
		/// <summary>
		/// Gets the mode of poisson distributed random numbers. 
		/// </summary>
        public override double[] Mode
		{
			get
			{
				// Check if the value of lambda is a whole number.
                if (this.lambda == Math.Floor(this.lambda))
                {
                    return new double[] { this.lambda - 1.0, this.lambda };
                }
                else
                {
                    return new double[] { Math.Floor(this.lambda) };
                }
			}
		}
		
		/// <summary>
		/// Returns a poisson distributed floating point random number.
		/// </summary>
		/// <returns>A poisson distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            double count = 0.0;
            for (double product = this.Generator.NextDouble(); product >= this.helper1; product *= this.Generator.NextDouble())
            {
                count++;
            }

            return count;
        }
		#endregion
	}
}