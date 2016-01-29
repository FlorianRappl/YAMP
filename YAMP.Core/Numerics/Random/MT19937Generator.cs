/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * 
 */

using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents a Mersenne Twister pseudo-random number generator with period 2^19937-1.
    /// </summary>
    /// <remarks>
    /// The <see cref="MT19937Generator"/> type bases upon information and the implementation presented on the
    ///   <a href="http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html">Mersenne Twister Home Page</a>.
    /// </remarks>
    public class MT19937Generator : Generator
    {
        #region class fields
        /// <summary>
        /// Represents the number of unsigned random numbers generated at one time. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 624.</remarks>
        private const int N = 624;

        /// <summary>
        /// Represents a constant used for generation of unsigned random numbers. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 397.</remarks>
        private const int M = 397;
        
        /// <summary>
        /// Represents the constant vector a. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x9908b0dfU.</remarks>
        private const uint VectorA = 0x9908b0dfU; 

        /// <summary>
        /// Represents the most significant w-r bits. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x80000000.</remarks>
        private const uint UpperMask = 0x80000000U;
        
        /// <summary>
        /// Represents the least significant r bits. This field is constant.
        /// </summary>
        /// <remarks>The value of this constant is 0x7fffffff.</remarks>
        private const uint LowerMask = 0x7fffffffU;

        /// <summary>
        /// Represents the multiplier that computes a double-precision floating point number greater than or equal to 0.0 
        ///   and less than 1.0 when it gets applied to a nonnegative 32-bit signed integer.
        /// </summary>
        private const double IntToDoubleMultiplier = 1.0 / ((double)int.MaxValue + 1.0);

        /// <summary>
        /// Represents the multiplier that computes a double-precision floating point number greater than or equal to 0.0 
        ///   and less than 1.0  when it gets applied to a 32-bit unsigned integer.
        /// </summary>
        private const double UIntToDoubleMultiplier = 1.0 / ((double)uint.MaxValue + 1.0);
        #endregion

        #region instance fields
        /// <summary>
        /// Stores the state vector array.
        /// </summary>
        private uint[] mt;

        /// <summary>
        /// Stores an index for the state vector array element that will be accessed next.
        /// </summary>
        private uint mti;

        /// <summary>
        /// Stores the used seed value.
        /// </summary>
        private uint seed;

        /// <summary>
        /// Stores the used seed array.
        /// </summary>
        private uint[] seedArray;

        /// <summary>
        /// Stores an <see cref="uint"/> used to generate up to 32 random <see cref="Boolean"/> values.
        /// </summary>
        private uint bitBuffer;

        /// <summary>
        /// Stores how many random <see cref="Boolean"/> values still can be generated from <see cref="bitBuffer"/>.
        /// </summary>
        private int bitCount;
        #endregion

        #region construction
        /// <summary>
        /// Initializes a new instance of the <see cref="MT19937Generator"/> class, using a time-dependent default 
        ///   seed value.
        /// </summary>
        public MT19937Generator() : this((uint)Math.Abs(Environment.TickCount))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MT19937Generator"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// If a negative number is specified, the absolute value of the number is used. 
        /// </param>
        public MT19937Generator(int seed) : this((uint)Math.Abs(seed))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MT19937Generator"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">
        /// An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public MT19937Generator(uint seed)
        {
            this.mt = new uint[MT19937Generator.N];
            this.seed = seed;
            this.seedArray = null;
            this.ResetGenerator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MT19937Generator"/> class, using the specified seed array.
        /// </summary>
        /// <param name="seedArray">
        /// An array of numbers used to calculate a starting values for the pseudo-random number sequence.
        /// If negative numbers are specified, the absolute values of them are used. 
        /// </param>
        public MT19937Generator(int[] seedArray)
        {
            this.mt = new uint[MT19937Generator.N];
            this.seed = 19650218U;
            this.seedArray = new uint[seedArray.Length];

            for (int index = 0; index < seedArray.Length; index++)
                this.seedArray[index] = (uint)Math.Abs(seedArray[index]);

            this.ResetGenerator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MT19937Generator"/> class, using the specified seed array.
        /// </summary>
        /// <param name="seedArray">
        /// An array of unsigned numbers used to calculate a starting values for the pseudo-random number sequence.
        /// </param>
        public MT19937Generator(uint[] seedArray)
        {
            this.mt = new uint[MT19937Generator.N];
            this.seed = 19650218U;
            this.seedArray = seedArray;
            this.ResetGenerator();
        }

        #endregion

        #region instance methods
        /// <summary>
        /// Resets the <see cref="MT19937Generator"/>, so that it produces the same pseudo-random number sequence again.
        /// </summary>
        private void ResetGenerator()
        {
            this.mt[0] = this.seed & 0xffffffffU;
            for (this.mti = 1; this.mti < MT19937Generator.N; this.mti++)
            {
                this.mt[mti] = (1812433253U * (this.mt[this.mti - 1] ^ (this.mt[this.mti - 1] >> 30)) + this.mti);
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier.
                // In the previous versions, MSBs of the seed affect only MSBs of the array mt[].
                // 2002/01/09 modified by Makoto Matsumoto
            }

            // If the object was instanciated with a seed array do some further (re)initialisation.
            if (this.seedArray != null)
            {
                this.ResetBySeedArray();
            }

            // Reset helper variables used for generation of random bools.
            this.bitBuffer = 0;
            this.bitCount = 32;
        }

        /// <summary>
        /// Extends resetting of the <see cref="MT19937Generator"/> using the <see cref="seedArray"/>.
        /// </summary>
        private void ResetBySeedArray()
        {
            uint i = 1;
            uint j = 0;
            int k = (MT19937Generator.N > this.seedArray.Length) ? MT19937Generator.N : this.seedArray.Length;
            for (; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525U)) + this.seedArray[j] + j; // non linear
                i++;
                j++;
                if (i >= MT19937Generator.N)
                {
                    mt[0] = mt[MT19937Generator.N - 1];
                    i = 1;
                }
                if (j >= this.seedArray.Length)
                {
                    j = 0;
                }
            }
            for (k = MT19937Generator.N - 1; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941U)) - i; // non linear
                i++;
                if (i >= MT19937Generator.N)
                {
                    mt[0] = mt[MT19937Generator.N - 1];
                    i = 1;
                }
            }

            mt[0] = 0x80000000U; // MSB is 1; assuring non-0 initial array
        }

        /// <summary>
        /// Generates <see cref="MT19937Generator.N"/> unsigned random numbers.
        /// </summary>
        /// <remarks>
        /// Generated random numbers are 32-bit unsigned integers greater than or equal to <see cref="UInt32.MinValue"/> 
        ///   and less than or equal to <see cref="UInt32.MaxValue"/>.
        /// </remarks>
        private void GenerateNUInts()
        {
            int kk;
            uint y;
            uint[] mag01 = new uint[2] { 0x0U, MT19937Generator.VectorA };
            
            for (kk = 0; kk < MT19937Generator.N - MT19937Generator.M; kk++)
            {
                y = (this.mt[kk] & MT19937Generator.UpperMask) | (this.mt[kk + 1] & MT19937Generator.LowerMask);
                this.mt[kk] = this.mt[kk + MT19937Generator.M] ^ (y >> 1) ^ mag01[y & 0x1U];
            }
            for (; kk < MT19937Generator.N - 1; kk++)
            {
                y = (this.mt[kk] & MT19937Generator.UpperMask) | (this.mt[kk + 1] & MT19937Generator.LowerMask);
                this.mt[kk] = this.mt[kk + (MT19937Generator.M - MT19937Generator.N)] ^ (y >> 1) ^ mag01[y & 0x1U];
            }
            y = (this.mt[MT19937Generator.N - 1] & MT19937Generator.UpperMask) | (this.mt[0] & MT19937Generator.LowerMask);
            this.mt[MT19937Generator.N - 1] = this.mt[MT19937Generator.M - 1] ^ (y >> 1) ^ mag01[y & 0x1U];

            this.mti = 0;
        }

        /// <summary>
        /// Returns an unsigned random number.
        /// </summary>
        /// <returns>
        /// A 32-bit unsigned integer greater than or equal to <see cref="UInt32.MinValue"/> and 
        ///   less than or equal to <see cref="UInt32.MaxValue"/>.
        /// </returns>
        public uint NextUInt()
        {
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }

            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            return (y ^ (y >> 18));
        }

        /// <summary>
        /// Returns a nonnegative random number less than or equal to MaxValue.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to 0, and less than or equal to MaxValue; 
        ///   that is, the range of return values includes 0 and MaxValue.
        /// </returns>
        public int NextInclusiveMaxValue()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            return (int)(y >> 1);
        }
        #endregion

        #region overridden Generator members
        /// <summary>
        /// Gets a value indicating whether the <see cref="MT19937Generator"/> can be reset, so that it produces the 
        ///   same pseudo-random number sequence again.
        /// </summary>
        public override bool CanReset
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Resets the <see cref="MT19937Generator"/>, so that it produces the same pseudo-random number sequence again.
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        public override bool Reset()
        {
            this.ResetGenerator();
            return true;
        }

        /// <summary>
        /// Returns a nonnegative random number less than MaxValue.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to 0, and less than MaxValue; that is, 
        ///   the range of return values includes 0 but not MaxValue.
        /// </returns>
        public override int Next()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            int result = (int)(y >> 1);
            // Exclude Int32.MaxValue from the range of return values.
            if (result == Int32.MaxValue)
            {
                return this.Next();
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// <paramref name="maxValue"/> must be greater than or equal to 0. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to 0, and less than <paramref name="maxValue"/>; that is, 
        ///   the range of return values includes 0 but not <paramref name="maxValue"/>. 
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxValue"/> is less than 0. 
        /// </exception>
        public override int Next(int maxValue)
        {
            if (maxValue < 0)
            {
				maxValue = -maxValue;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (int)((double)(int)(y >> 1) * MT19937Generator.IntToDoubleMultiplier * (double)maxValue);
        }

        /// <summary>
        /// Returns a random number within the specified range. 
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number to be generated. 
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less than 
        ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but 
        ///   not <paramref name="maxValue"/>. 
        /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.  
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
				maxValue = minValue;

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            int range = maxValue - minValue;
            if (range < 0)
            {
                // The range is greater than Int32.MaxValue, so we have to use slower floating point arithmetic.
                // Also all 32 random bits (uint) have to be used which again is slower (See comment in NextDouble()).
                return minValue + (int)
                    ((double)y * MT19937Generator.UIntToDoubleMultiplier * ((double)maxValue - (double)minValue));
            }
            else
            {
                // 31 random bits (int) will suffice which allows us to shift and cast to an int before the first multiplication and gain better performance.
                // See comment in NextDouble().
                return minValue + (int)((double)(int)(y >> 1) * MT19937Generator.IntToDoubleMultiplier * (double)range);
            }
        }

        /// <summary>
        /// Returns a nonnegative floating point random number less than 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0; that is, 
        ///   the range of return values includes 0.0 but not 1.0.
        /// </returns>
        public override double NextDouble()
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            // Here a ~2x speed improvement is gained by computing a value that can be cast to an int 
            //   before casting to a double to perform the multiplication.
            // Casting a double from an int is a lot faster than from an uint and the extra shift operation 
            //   and cast to an int are very fast (the allocated bits remain the same), so overall there's 
            //   a significant performance improvement.
            return (double)(int)(y >> 1) * MT19937Generator.IntToDoubleMultiplier;
        }

        /// <summary>
        /// Returns a nonnegative floating point random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// <paramref name="maxValue"/> must be greater than or equal to 0.0. 
        /// </param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than <paramref name="maxValue"/>; 
        ///   that is, the range of return values includes 0 but not <paramref name="maxValue"/>. 
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxValue"/> is less than 0. 
        /// </exception>
        public override double NextDouble(double maxValue)
        {
            if (maxValue < 0.0)
            {
				maxValue = -maxValue;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return (double)(int)(y >> 1) * MT19937Generator.IntToDoubleMultiplier * maxValue;
        }

        /// <summary>
        /// Returns a floating point random number within the specified range. 
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number to be generated. 
        /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> must be less than or equal to
        ///   <see cref="Double.MaxValue"/>
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. 
        /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> must be less than or equal to
        ///   <see cref="Double.MaxValue"/>.
        /// </param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to <paramref name="minValue"/>, and less than 
        ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but 
        ///   not <paramref name="maxValue"/>. 
        /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.  
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> is greater than
        ///   <see cref="Double.MaxValue"/>.
        /// </exception>
        public override double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
				maxValue = minValue;
            }

            double range = maxValue - minValue;

            if (range == double.PositiveInfinity)
            {
				return 0.0;
            }

            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            if (this.mti >= MT19937Generator.N)
            {// generate N words at one time
                this.GenerateNUInts();
            }
            uint y = this.mt[this.mti++];
            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            // The shift operation and extra int cast before the first multiplication give better performance.
            // See comment in NextDouble().
            return minValue + (double)(int)(y >> 1) * MT19937Generator.IntToDoubleMultiplier * range;
        }
        
        /// <summary>
        /// Returns a random Boolean value.
        /// </summary>
        /// <remarks>
        /// Buffers 32 random bits (1 uint) for future calls, so a new random number is only generated every 32 calls.
        /// </remarks>
        /// <returns>A <see cref="Boolean"/> value.</returns>
        public override bool NextBoolean()
        {
            if (this.bitCount == 32)
            {
                // Generate 32 more bits (1 uint) and store it for future calls.
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                if (this.mti >= MT19937Generator.N)
                {// generate N words at one time
                    this.GenerateNUInts();
                }
                uint y = this.mt[this.mti++];
                // Tempering
                y ^= (y >> 11);
                y ^= (y << 7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                this.bitBuffer = (y ^ (y >> 18));

                // Reset the bitCount and use rightmost bit of buffer to generate random bool.
                this.bitCount = 1;
                return (this.bitBuffer & 0x1) == 1;
            }

            // Increase the bitCount and use rightmost bit of shifted buffer to generate random bool.
            this.bitCount++;
            return ((this.bitBuffer >>= 1) & 0x1) == 1;
        }
        
        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers. 
        /// </summary>
        /// <remarks>
        /// Each element of the array of bytes is set to a random number greater than or equal to 0, and less than or 
        ///   equal to <see cref="Byte.MaxValue"/>.
        /// </remarks>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is a null reference (<see langword="Nothing"/> in Visual Basic). 
        /// </exception>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
            {
				buffer = new byte[0];
            }

            // Fill the buffer with 4 bytes (1 uint) at a time.
            int i = 0;
            uint y;
            while (i < buffer.Length - 3)
            {
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                if (this.mti >= MT19937Generator.N)
                {// generate N words at one time
                    this.GenerateNUInts();
                }
                y = this.mt[this.mti++];
                // Tempering
                y ^= (y >> 11);
                y ^= (y << 7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                y ^= (y >> 18);

                buffer[i++] = (byte)y;
                buffer[i++] = (byte)(y >> 8);
                buffer[i++] = (byte)(y >> 16);
                buffer[i++] = (byte)(y >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i < buffer.Length)
            {
                // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
                if (this.mti >= MT19937Generator.N)
                {// generate N words at one time
                    this.GenerateNUInts();
                }
                y = this.mt[this.mti++];
                // Tempering
                y ^= (y >> 11);
                y ^= (y << 7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                y ^= (y >> 18);

                buffer[i++] = (byte)y;
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte)(y >> 8);
                    if (i < buffer.Length)
                    {
                        buffer[i++] = (byte)(y >> 16);
                        if (i < buffer.Length)
                        {
                            buffer[i] = (byte)(y >> 24);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
