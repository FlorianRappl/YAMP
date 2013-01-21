/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
    /// Declares common functionality for all random number distributions.
	/// </summary>
	public abstract class Distribution
    {
        #region instance fields

        /// <summary>
        /// Gets or sets a Generator object that can be used as underlying random number generator.
        /// </summary>
        protected Generator Generator
        {
            get
            {
                return this.generator;
            }
            set
            {
                this.generator = value;
            }
        }

        /// <summary>
        /// Stores a Generator object that can be used as underlying random number generator.
        /// </summary>
        Generator generator;

        /// <summary>
        /// Gets the standard genertor to use (MT19937).
        /// </summary>
        protected static readonly Generator standardGenerator = new MT19937Generator();

        /// <summary>
        /// Gets a value indicating whether the random number distribution can be reset, so that it produces the same 
        /// random number sequence again.
        /// </summary>
        public bool CanReset
        {
            get
            {
                return this.generator.CanReset;
            }
        }

        #endregion

        #region construction

        /// <summary>
        /// Initializes a new instance of the Distribution class, using a 
        /// StandardGenerator as underlying random number generator.
        /// </summary>
        protected Distribution() : this(standardGenerator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Distribution class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A Generator object.</param>
        /// <exception cref="ArgumentNullException">
        /// Generator is NULL (Nothing in Visual Basic).
        /// </exception>
        protected Distribution(Generator generator)
        {
            this.generator = generator;
        }

        #endregion

        #region virtual instance methods
        /// <summary>
        /// Resets the random number distribution, so that it produces the same random number sequence again.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, if the random number distribution was reset; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Reset()
        {
            return this.generator.Reset();
        }
        #endregion

        #region abstract members
        /// <summary>
		/// Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public abstract double Minimum
		{
			get;
		}

        /// <summary>
		/// Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public abstract double Maximum
		{
			get;
		}

        /// <summary>
		/// Gets the mean of distributed random numbers.
        /// </summary>
        public abstract double Mean
		{
			get;
		}
		
		/// <summary>
		/// Gets the median of distributed random numbers.
		/// </summary>
        public abstract double Median
		{
			get;
		}

        /// <summary>
		/// Gets the variance of distributed random numbers.
        /// </summary>
        public abstract double Variance
		{
			get;
		}
		
		/// <summary>
		/// Gets the mode of distributed random numbers.
		/// </summary>
        public abstract double[] Mode
		{
			get;
		}
		
		/// <summary>
		/// Returns a distributed floating point random number.
		/// </summary>
		/// <returns>A distributed double-precision floating point number.</returns>
        public abstract double NextDouble();
        #endregion
    }
}