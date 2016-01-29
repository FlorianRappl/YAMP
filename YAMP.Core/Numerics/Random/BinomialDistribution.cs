/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
    /// Provides generation of binomial distributed random numbers.
	/// </summary>
	/// <remarks>
    /// The binomial distribution generates only discrete numbers.<br />
    /// The implementation of the <see cref="BinomialDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/binomial_distribution">Wikipedia - Binomial distribution</a>.
    /// </remarks>
	public class BinomialDistribution : Distribution
	{
		#region instance fields

		/// <summary>
        /// Gets or sets the parameter alpha which is used for generation of binomial distributed random numbers.
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
                }
        	}
		}

		/// <summary>
        /// Stores the parameter alpha which is used for generation of binomial distributed random numbers.
		/// </summary>
        double alpha;

        /// <summary>
        /// Gets or sets the parameter beta which is used for generation of binomial distributed random numbers.
        /// </summary>
        /// <remarks>Call <see cref="IsValidBeta"/> to determine whether a value is valid and therefor assignable.</remarks>
        public int Beta
        {
            get
            {
                return this.beta;
            }
            set
            {
                if (this.IsValidBeta(value))
                {
                    this.beta = value;
                }
            }
        }

        /// <summary>
        /// Stores the parameter beta which is used for generation of binomial distributed random numbers.
        /// </summary>
        int beta;

        #endregion

		#region construction

		/// <summary>
        /// Initializes a new instance of the class, using a as underlying random number generator.
		/// </summary>
        public BinomialDistribution()
        {
            this.alpha = 0.5;
            this.beta = 1;
		}
		
		/// <summary>
        /// Initializes a new instance of the <see cref="BinomialDistribution"/> class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public BinomialDistribution(Generator generator)
            : base(generator)
        {
            this.alpha = 0.5;
            this.beta = 1;
        }

		#endregion
	
		#region instance methods

		/// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is greater than or equal to 0.0, and less than or equal to 1.0; otherwise, <see langword="false"/>.
		/// </returns>
        public bool IsValidAlpha(double value)
		{
			return (value >= 0.0 && value <= 1.0);
		}

        /// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// <see langword="true"/> if value is greater than or equal to 0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidBeta(int value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Returns a binomial distributed random number.
        /// </summary>
        /// <returns>A binomial distributed 32-bit signed integer.</returns>
        public int Next()
        {
            int successes = 0;

            for (int i = 0; i < this.beta; i++)
            {
                if (this.Generator.NextDouble() < this.alpha)
                    successes++;
            }

            return successes;
        }

        #endregion

		#region overridden Distribution members

        /// <summary>
        /// Gets the minimum possible value of binomial distributed random numbers.
		/// </summary>
        public override double Minimum
		{
			get
			{
				return 0.0;
			}
		}

		/// <summary>
        /// Gets the maximum possible value of binomial distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return this.beta;
			}
		}

		/// <summary>
        /// Gets the mean value of binomial distributed random numbers.
		/// </summary>
        public override double Mean
		{
			get
			{
                return this.alpha * this.beta;
			}
		}
		
		/// <summary>
        /// Gets the median of binomial distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
				return double.NaN;
			}
		}
		
		/// <summary>
        /// Gets the variance of binomial distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
                return this.alpha * (1.0 - this.alpha) * this.beta;
			}
		}
		
		/// <summary>
        /// Gets the mode of binomial distributed random numbers.
		/// </summary>
        public override double[] Mode
		{
            get
            {
                return new double[] { Math.Floor(this.alpha * (this.beta + 1.0)) };
            }
		}
		
		/// <summary>
        /// Returns a binomial distributed floating point random number.
		/// </summary>
        /// <returns>A binomial distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            double successes = 0.0;

            for (int i = 0; i < this.beta; i++)
            {
                if (this.Generator.NextDouble() < this.alpha)
                    successes++;
            }
            
            return successes;
        }

		#endregion
	}
}