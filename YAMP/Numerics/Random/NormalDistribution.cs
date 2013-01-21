/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of normal distributed random numbers.
	/// </summary>
	/// <remarks>
    /// The implementation of the <see cref="NormalDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Normal_distribution">Wikipedia - Normal distribution</a>
    ///   and the implementation in the <a href="http://www.lkn.ei.tum.de/lehre/scn/cncl/doc/html/cncl_toc.html">
    ///   Communication Networks Class Library</a>.
    /// </remarks>
	public class NormalDistribution : Distribution
	{
		#region instance fields
        /// <summary>
        /// Gets or sets the parameter mu which is used for generation of normal distributed random numbers.
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
                {
                    this.mu = value;
					this.UpdateHelpers();
                }
            }
        }

        /// <summary>
        /// Stores the parameter mu which is used for generation of normal distributed random numbers.
        /// </summary>
        private double mu;

        /// <summary>
		/// Gets or sets the parameter sigma which is used for generation of normal distributed random numbers.
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
		/// Stores the parameter sigma which is used for generation of normal distributed random numbers.
		/// </summary>
		private double sigma;

        /// <summary>
        /// Stores a precomputed normal distributed random number that will be returned the next time 
        ///   <see cref="NextDouble"/> gets called.
        /// </summary>
        /// <remarks>
        /// Two new normal distributed random numbers are generated every other call to <see cref="NextDouble"/>.
        /// </remarks>
        private double helper1;

        /// <summary>
        /// Stores a value indicating whether <see cref="NextDouble"/> was called twice since last generation of 
        ///   normal distributed random numbers.
        /// </summary>
        /// <remarks>
        /// Two new normal distributed random numbers are generated every other call to <see cref="NextDouble"/>.
        /// </remarks>
        private bool helper2;
		#endregion

		#region construction

		/// <summary>
        /// Initializes a new instance of the NormalDistribution class, using a 
        /// StandardGenerator as underlying random number generator.
		/// </summary>
        public NormalDistribution()
        {
            this.mu = 1.0;
            this.sigma = 1.0;
            this.UpdateHelpers();
		}

		/// <summary>
        /// Initializes a new instance of the NormalDistribution class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public NormalDistribution(Generator generator)
            : base(generator)
        {
            this.mu = 1.0;
            this.sigma = 1.0;
			this.UpdateHelpers();
        }

		#endregion
	
		#region instance methods
        /// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Mu"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidMu(double value)
        {
            return true;
        }

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
		/// Updates the helper variables that store intermediate results for generation of normal distributed random 
		///   numbers.
		/// </summary>
		private void UpdateHelpers()
		{
			this.helper1 = 0.0;
			this.helper2 = false;
		}
		#endregion

		#region overridden Distribution members
		/// <summary>
		/// Resets the normal distribution, so that it produces the same random number sequence again.
		/// </summary>
		/// <returns>
		/// <see langword="true"/>, if the normal distribution was reset; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Reset()
		{
			bool result = base.Reset();
			if (result)
			{
				this.UpdateHelpers();
			}

			return result;
		}
		
		/// <summary>
		/// Gets the minimum possible value of normal distributed random numbers.
		/// </summary>
        public override double Minimum
		{
			get
			{
				return double.MinValue;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of normal distributed random numbers.
		/// </summary>
        public override double Maximum
		{
			get
			{
				return double.MaxValue;
			}
		}

		/// <summary>
		/// Gets the mean value of normal distributed random numbers.
		/// </summary>
        public override double Mean
		{
			get
			{
				return this.mu;
			}
		}
		
		/// <summary>
		/// Gets the median of normal distributed random numbers.
		/// </summary>
        public override double Median
		{
			get
			{
                return this.mu;
			}
		}
		
		/// <summary>
		/// Gets the variance of normal distributed random numbers.
		/// </summary>
        public override double Variance
		{
			get
			{
				return Math.Pow(this.sigma, 2.0);
			}
		}
		
		/// <summary>
		/// Gets the mode of normal distributed random numbers.
		/// </summary>
        public override double[] Mode
		{
			get
			{
                return new double[] { this.mu };
			}
		}
		
		/// <summary>
		/// Returns a normal distributed floating point random number.
		/// </summary>
		/// <returns>A normal distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            if (this.helper2)
            {
                this.helper2 = false;

                return this.helper1;
            }
            else
            {
				this.helper2 = true;
				
				while (true)
                {
                    double v1 = 2.0 * this.Generator.NextDouble() - 1.0;
                    double v2 = 2.0 * this.Generator.NextDouble() - 1.0;
                    double w = v1 * v1 + v2 * v2;

                    if (w <= 1)
                    {
                        double y = Math.Sqrt(-2.0 * Math.Log(w) / w) * this.sigma;
                        this.helper1 = v2 * y + this.mu;
                        return v1 * y + this.mu;
                    }
                }
            }
        }
		#endregion
	}
}