/*
 * Copyright � 2006 Stefan Trosch�tz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of rayleigh distributed random numbers.
	/// </summary>
	/// <remarks>
    /// The implementation of the <see cref="RayleighDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Rayleigh_distribution">Wikipedia - Rayleigh Distribution</a>.
    /// </remarks>
	public class RayleighDistribution : Distribution
	{
		#region instance fields

		/// <summary>
		/// Gets or sets the parameter sigma which is used for generation of rayleigh distributed random numbers.
		/// </summary>
		/// <remarks>Call <see cref="IsValidSigma"/> to determine whether a value is valid and therefor assignable.</remarks>
		public double Sigma
		{
			get
			{
				return this.sigma;
			}
			set
			{
                if (this.IsValidSigma(value))
                {
                    this.sigma = value;
                    this.UpdateHelpers();
                }
        	}
		}

		/// <summary>
		/// Stores the parameter sigma which is used for generation of rayleigh distributed random numbers.
		/// </summary>
		double sigma;

        /// <summary>
        /// Stores first <see cref="NormalDistribution"/> object used for generation of rayleigh distributed random numbers.
        /// </summary>
        NormalDistribution normalDistribution1;

        /// <summary>
        /// Stores second <see cref="NormalDistribution"/> object used for generation of rayleigh distributed random numbers.
        /// </summary>
        NormalDistribution normalDistribution2;

        #endregion

		#region construction

		/// <summary>
        /// Initializes a new instance of the RayleighDistribution class, using a 
        /// StandardGenerator as underlying random number generator.
		/// </summary>
        public RayleighDistribution()
            : this(standardGenerator)
		{
		}

		/// <summary>
        /// Initializes a new instance of the RayleighDistribution class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public RayleighDistribution(Generator generator)
            : base(generator)
        {
            this.sigma = 1.0;
            this.normalDistribution1 = new NormalDistribution(generator);
            this.normalDistribution1.Mu = 0.0;
            this.normalDistribution2 = new NormalDistribution(generator);
            this.normalDistribution2.Mu = 0.0;
            this.UpdateHelpers();
        }

		#endregion
	
		#region instance methods

        /// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Sigma"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsValidSigma(double value)
		{
			return value > 0.0;
		}

        /// <summary>
        /// Updates the helper variables that store intermediate results for generation of rayleigh distributed random 
        ///   numbers.
        /// </summary>
        void UpdateHelpers()
        {
            this.normalDistribution1.Sigma = this.sigma;
            this.normalDistribution2.Sigma = this.sigma;
        }

        #endregion

		#region overridden Distribution members

		/// <summary>
		/// Resets the chi distribution, so that it produces the same random number sequence again.
		/// </summary>
		/// <returns>
		/// <see langword="true"/>, if the chi distribution was reset; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Reset()
		{
			bool result = base.Reset();

			if (result)
			{
				result = this.normalDistribution1.Reset();

				if (result)
					result = this.normalDistribution2.Reset();
			}

			return result;
		}

		/// <summary>
		/// Gets the minimum possible value of rayleigh distributed random numbers.
		/// </summary>
		public override double Minimum
		{
			get
			{
				return 0.0;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of rayleigh distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return double.MaxValue;
			}
		}

		/// <summary>
		/// Gets the mean value of rayleigh distributed random numbers.
		/// </summary>
        public override double Mean
		{
			get
			{
                return this.sigma * Math.Sqrt(Math.PI / 2.0);
			}
		}
		
		/// <summary>
		/// Gets the median of rayleigh distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
				return this.sigma * Math.Sqrt(Math.Log(4));
			}
		}
		
		/// <summary>
		/// Gets the variance of rayleigh distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
                return Math.Pow(this.sigma, 2.0) * (4.0 - Math.PI) / 2.0;
			}
		}
		
		/// <summary>
		/// Gets the mode of rayleigh distributed random numbers.
		/// </summary>
        public override double[] Mode
		{
            get
            {
                return new double[] { this.sigma };
            }
		}
		
		/// <summary>
		/// Returns a rayleigh distributed floating point random number.
		/// </summary>
		/// <returns>A rayleigh distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            return Math.Sqrt(Math.Pow(this.normalDistribution1.NextDouble(), 2) + Math.Pow(this.normalDistribution2.NextDouble(), 2));
        }

		#endregion
	}
}