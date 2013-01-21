/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of laplace distributed random numbers.
	/// </summary>
	/// <remarks>
    /// The implementation of the <see cref="LaplaceDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Laplace distribution</a>.
    /// </remarks>
	public class LaplaceDistribution : Distribution
	{
		#region instance fields

		/// <summary>
		/// Gets or sets the parameter alpha which is used for generation of laplace distributed random numbers.
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
		/// Stores the parameter alpha which is used for generation of laplace distributed random numbers.
		/// </summary>
		double alpha;
		
		/// <summary>
		/// Gets or sets the parameter mu which is used for generation of laplace distributed random numbers.
		/// </summary>
		/// <remarks>Call <see cref="IsValidMu"/> to determine whether a value is valid and therefor assignable.</remarks>
		public double Mu
		{
			get
			{
				return this.mu;
			}
			set
			{
                if (this.IsValidMu(value))
                    this.mu = value;
        	}
		}

		/// <summary>
		/// Stores the parameter mu which is used for generation of laplace distributed random numbers.
		/// </summary>
		double mu;

        #endregion

		#region construction, destruction

		/// <summary>
        /// Initializes a new instance of the <see cref="LaplaceDistribution"/> class, using a 
        /// StandardGenerator as underlying random number generator.
		/// </summary>
        public LaplaceDistribution()
        {
            this.alpha = 1.0;
            this.mu = 0.0;
		}

		/// <summary>
        /// Initializes a new instance of the LaplaceDistribution class, using the specified 
        /// Generator as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// Generator is NULL (Nothing in Visual Basic).
        /// </exception>
        public LaplaceDistribution(Generator generator)
            : base(generator)
        {
            this.alpha = 1.0;
            this.mu = 0.0;
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
			return value > 0;
		}
		
		/// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Mu"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
		public bool IsValidMu(double value)
		{
			return true;
		}

        #endregion

		#region overridden Distribution members

        /// <summary>
		/// Gets the minimum possible value of laplace distributed random numbers.
		/// </summary>
		public override double Minimum
		{
			get
			{
				return double.MinValue;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of laplace distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return double.MaxValue;
			}
		}

		/// <summary>
		/// Gets the mean value of laplace distributed random numbers.
		/// </summary>
        public override double Mean
		{
			get
			{
				return this.mu;
			}
		}
		
		/// <summary>
		/// Gets the median of laplace distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
                return this.mu;
			}
		}
		
		/// <summary>
		/// Gets the variance of laplace distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
                return 2.0 * Math.Pow(this.alpha, 2.0);
			}
		}
		
		/// <summary>
		/// Gets the mode of laplace distributed random numbers.
		/// </summary>
        public override double[] Mode
		{
            get
            {
                return new double[] { this.mu };
            }
		}
		
		/// <summary>
		/// Returns a laplace distributed floating point random number.
		/// </summary>
        /// <returns>A laplace distributed double-precision floating point number.</returns>
        public override double NextDouble()
		{
            double rand = 0.5 - this.Generator.NextDouble();
            return this.mu - this.alpha * Math.Sign(rand) * Math.Log(2.0 * Math.Abs(rand));
		}

		#endregion
	}
}