using System;

namespace YAMP
{
	class Constants
	{
        static readonly ScalarValue pi = new ScalarValue(Math.PI);

        public ScalarValue Pi
		{
			get { return pi; }
		}

        static readonly ScalarValue e = new ScalarValue(Math.E);

        public ScalarValue E
		{
			get { return e; }
		}

        static readonly ScalarValue gamma = new ScalarValue(0.57721566490153286060651209008240243);

        public ScalarValue Gamma
		{
			get { return gamma; }
		}

        static readonly ScalarValue phi = new ScalarValue(1.61803398874989484820458683436563811);

        public ScalarValue Phi
		{
			get { return phi; }
		}

        static readonly ScalarValue delta = new ScalarValue(4.66920160910299067185320382046620161);

        public ScalarValue Delta
		{
			get { return delta; }
		}

        static readonly ScalarValue alpha = new ScalarValue(2.50290787509589282228390287321821578);

        public ScalarValue Alpha
		{
			get { return alpha; }
		}

        static readonly ScalarValue i = ScalarValue.I;

        public ScalarValue I
        {
            get { return i; }
        }
	}
}

