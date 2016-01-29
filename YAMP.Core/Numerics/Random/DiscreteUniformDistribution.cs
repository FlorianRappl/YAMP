/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */


using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Provides generation of discrete uniformly distributed random numbers.
	/// </summary>
	/// <remarks>
	/// The discrete uniform distribution generates only discrete numbers.<br />
	/// The implementation of the <see cref="DiscreteUniformDistribution"/> type bases upon information presented on
	///   <a href="http://en.wikipedia.org/wiki/Uniform_distribution_%28discrete%29">
	///   Wikipedia - Uniform distribution (discrete)</a>.
	/// </remarks>
	public class DiscreteUniformDistribution : Distribution
	{
		#region instance fields
		/// <summary>
		/// Gets or sets the parameter alpha which is used for generation of uniformly distributed random numbers.
		/// </summary>
		/// <remarks>Call <see cref="IsValidAlpha"/> to determine whether a value is valid and therefor assignable.</remarks>
		public int Alpha
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
		/// Stores the parameter alpha which is used for generation of uniformly distributed random numbers.
		/// </summary>
		private int alpha;

		/// <summary>
		/// Gets or sets the parameter beta which is used for generation of uniformly distributed random numbers.
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
					this.UpdateHelpers();
				}
			}
		}

		/// <summary>
		/// Stores the parameter beta which is used for generation of uniformly distributed random numbers.
		/// </summary>
		private int beta;

		/// <summary>
		/// Stores an intermediate result for generation of uniformly distributed random numbers.
		/// </summary>
		/// <remarks>
		/// Speeds up random number generation cause this value only depends on distribution parameters 
		///   and therefor doesn't need to be recalculated in successive executions of <see cref="NextDouble"/>.
		/// </remarks>
		private int helper1;
		#endregion

		#region construction
		/// <summary>
        /// Initializes a new instance of the DiscreteUniformDistribution class, using a 
        /// StandardGenerator as underlying random number generator. 
		/// </summary>
		public DiscreteUniformDistribution()
        {
            this.alpha = 0;
            this.beta = 1;
            this.UpdateHelpers();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscreteUniformDistribution"/> class, using the specified 
		///   <see cref="Generator"/> as underlying random number generator.
		/// </summary>
		/// <param name="generator">A <see cref="Generator"/> object.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
		/// </exception>
		public DiscreteUniformDistribution(Generator generator)
			: base(generator)
		{
			this.alpha = 0;
			this.beta = 1;
			this.UpdateHelpers();
		}
		#endregion

		#region instance methods
		/// <summary>
		/// Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is less than or equal to <see cref="Beta"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsValidAlpha(int value)
		{
			return (value <= this.beta);
		}

		/// <summary>
		/// Determines whether the specified value is valid for parameter <see cref="Beta"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// <see langword="true"/> if value is greater than or equal to <see cref="Alpha"/>, and less than 
		///   <see cref="int.MaxValue"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsValidBeta(int value)
		{
			return (value >= this.alpha && value < int.MaxValue);
		}

		/// <summary>
		/// Updates the helper variables that store intermediate results for generation of uniformly distributed random 
		///   numbers.
		/// </summary>
		private void UpdateHelpers()
		{
			this.helper1 = this.beta + 1;
		}

		/// <summary>
		/// Returns a uniformly distributed random number.
		/// </summary>
		/// <returns>A geometric distributed 32-bit signed integer.</returns>
		public int Next()
		{
			return this.Generator.Next(this.alpha, this.helper1);
		}
		#endregion

		#region overridden Distribution members
		/// <summary>
		/// Gets the minimum possible value of uniformly distributed random numbers.
		/// </summary>
		public override double Minimum
		{
			get
			{
				return this.alpha;
			}
		}

		/// <summary>
		/// Gets the maximum possible value of uniformly distributed random numbers.
		/// </summary>
		public override double Maximum
		{
			get
			{
				return this.beta;
			}
		}

		/// <summary>
		/// Gets the mean value of the uniformly distributed random numbers.
		/// </summary>
		public override double Mean
		{
			get
			{
				return this.alpha / 2.0 + this.beta / 2.0;
			}
		}

		/// <summary>
		/// Gets the median of uniformly distributed random numbers.
		/// </summary>
		public override double Median
		{
			get
			{
				return this.alpha / 2.0 + this.beta / 2.0;
			}
		}

		/// <summary>
		/// Gets the variance of uniformly distributed random numbers.
		/// </summary>
		public override double Variance
		{
			get
			{
				return (Math.Pow(this.beta - this.alpha + 1.0, 2.0) - 1.0) / 12.0;
			}
		}

		/// <summary>
		/// Gets the mode of the uniformly distributed random numbers.
		/// </summary>
		public override double[] Mode
		{
			get
			{
				return new double[] { };
			}
		}

		/// <summary>
		/// Returns a uniformly distributed floating point random number.
		/// </summary>
		/// <returns>A uniformly distributed double-precision floating point number.</returns>
		public override double NextDouble()
		{
			return this.Generator.Next(this.alpha, this.helper1);
		}
		#endregion
	}
}