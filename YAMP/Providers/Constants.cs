using System;

namespace YAMP
{
    /// <summary>
    /// Represents a list of ready to use constants for the parser
    /// </summary>
	class Constants
	{
        static readonly ScalarValue pi = new ScalarValue(Math.PI);

        /// <summary>
        /// Gets the value of Pi.
        /// </summary>
        public ScalarValue Pi
		{
			get { return pi; }
		}

        static readonly ScalarValue e = new ScalarValue(Math.E);

        /// <summary>
        /// Gets the value of euler's number.
        /// </summary>
        public ScalarValue E
		{
			get { return e; }
		}

        static readonly ScalarValue gamma = new ScalarValue(0.57721566490153286060651209008240243);

        /// <summary>
        /// Gets the value of gamma.
        /// </summary>
        public ScalarValue Gamma
		{
			get { return gamma; }
		}

        static readonly ScalarValue phi = new ScalarValue(1.61803398874989484820458683436563811);

        /// <summary>
        /// Gets the value of the golden ratio.
        /// </summary>
        public ScalarValue Phi
		{
			get { return phi; }
		}

        static readonly ScalarValue delta = new ScalarValue(4.66920160910299067185320382046620161);

        /// <summary>
        /// Gets the value of delta.
        /// </summary>
        public ScalarValue Delta
		{
			get { return delta; }
		}

        static readonly ScalarValue alpha = new ScalarValue(2.50290787509589282228390287321821578);

        /// <summary>
        /// Gets the value of alpha.
        /// </summary>
        public ScalarValue Alpha
		{
			get { return alpha; }
		}

        static readonly ScalarValue i = ScalarValue.I;

        /// <summary>
        /// Gets the value of the imaginary constant.
        /// </summary>
        public ScalarValue I
        {
            get { return i; }
        }
	}
}

