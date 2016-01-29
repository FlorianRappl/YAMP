/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
	/// <summary>
	/// Declares common functionality for all random number generators.
	/// </summary>
	public abstract class Generator
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the random number generator can be reset, so that it produces the same 
        ///   random number sequence again.
        /// </summary>
        public abstract bool CanReset
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the random number generator, so that it produces the same random number sequence again.
        /// </summary>
        /// <returns>
        /// True, if the random number generator was reset; otherwise, false.
        /// </returns>
        public abstract bool Reset();
		
		/// <summary>
		/// Returns a nonnegative random number less than MaxValue.
		/// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to 0, and less than that is, 
        /// the range of return values includes 0 but not MaxValue.
        /// </returns>
        public abstract int Next();

		/// <summary>
		/// Returns a nonnegative random number less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// <paramref name="maxValue"/> must be greater than or equal to 0. 
		/// </param>
		/// <returns>
        /// A 32-bit signed integer greater than or equal to 0, and less than MaxValue; that is, 
        /// the range of return values includes 0 but not MaxValue. 
		/// </returns>
        public abstract int Next(int maxValue);

        /// <summary>
        /// Returns a random number within the specified range. 
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number to be generated. 
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// MaxValue must be greater than or equal to MinValue. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to MinValue, and less than 
        /// MaxValue; that is, the range of return values includes MinValue but not MaxValue.
        /// </returns>
        public abstract int Next(int minValue, int maxValue);

        /// <summary>
		/// Returns a nonnegative floating point random number less than 1.0.
		/// </summary>
		/// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0; that is, 
        ///   the range of return values includes 0.0 but not 1.0. 
		/// </returns>
        public abstract double NextDouble();

        /// <summary>
        /// Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// MaxValue must be greater than or equal to 0.0. 
        /// </param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than MaxValue; 
        ///   that is, the range of return values includes 0 but not MaxValue. 
        /// </returns>
        public abstract double NextDouble(double maxValue);

        /// <summary>
        /// Returns a floating point random number within the specified range. 
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number to be generated. 
        /// The range between <paramref name="minValue"/> and MaxValue must be less than or equal to
        /// MaxValue.
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// MaxValue must be greater than or equal to <paramref name="minValue"/>.
        /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> must be less than or equal to
        ///   <see cref="Double.MaxValue"/>.
        /// </param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to <paramref name="minValue"/>, and less than 
        ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but 
        ///   not <paramref name="maxValue"/>. 
        /// </returns>
        public abstract double NextDouble(double minValue, double maxValue);

        /// <summary>
        /// Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        /// Buffers 31 random bits for future calls, so the random number generator is only invoked once in every 31 calls.
        /// </remarks>
        /// <returns>A <see cref="Boolean"/> value.</returns>
        public abstract bool NextBoolean();
        
        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers. 
        /// </summary>
        /// <remarks>
        /// Each element of the array of bytes is set to a random number greater than or equal to 0, and less than or 
        ///   equal to <see cref="Byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public abstract void NextBytes(byte[] buffer);

        #endregion
    }
}