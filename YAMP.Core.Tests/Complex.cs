namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Complex : Base
    {
        [Test]
        public void MultiplicationSubtractionDivisionOfComplexNumbers()
        {
            Test("|4*(2i-5)/3i|", 4.0 * Math.Sqrt(29.0) / 3.0);
        }
        [Test]
        public void RealPowerOfComplexNumber()
        {
            Test("|(2+3i)^2|", 13.0, 1e-8);
        }
        [Test]
        public void AbsoluteValueOfImaginaryPower()
        {
            Test("|1^(i+5)|", 1.0);
        }
        [Test]
        public void OneImaginaryPOwer()
        {
            Test("|1^(i+5)|", 1.0);
        }
        [Test]
        public void PowerOfComplexNumbers()
        {
            Test("|(5+8i)^(i+1)|", 3.4284942595728127, 1e-8);
        }
        [Test]
        public void DivisionOfComplexNumbers()
        {
            Test("|(2+3i)/(1+8i)|", 0.447213595499958, 1e-8);
        }
        [Test]
        public void RealPart()
        {
            Test("real(2 + 5i)", 2.0);
        }
        [Test]
        public void ImaginaryPart()
        {
            Test("imag(2 + 5i)", 5.0);
        }
    }
}
