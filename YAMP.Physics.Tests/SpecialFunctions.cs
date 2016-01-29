namespace YAMP.Physics.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SpecialFunctions : Base
    {
        [Test]
        public void Ylm0000()
        {
            Test("ylm(0, 0, 0, 0)", 0.5 / Math.Sqrt(Math.PI));
        }
        [Test]
        public void Ylm00100()
        {
            Test("ylm(0, 0, 10, 0)", 0.5 / Math.Sqrt(Math.PI));
        }
        [Test]
        public void Ylm0005()
        {
            Test("ylm(0, 0, 0, 5)", 0.5 / Math.Sqrt(Math.PI));
        }
        [Test]
        public void Ylm10Pi30()
        {
            Test("ylm(1, 0, pi / 3, 0)", 0.5 * Math.Sqrt(3.0 / Math.PI) * Math.Cos(Math.PI / 3.0));
        }
        [Test]
        public void Ylm22Pi30()
        {
            Test("ylm(2, 2, pi / 3, 0)", 0.28970565151739147, 1e-8);
        }
        [Test]
        public void ImaginaryPartOfYlm21Pi305()
        {
            Test("imag(ylm(2, 1, pi / 3, 0.5))", -0.16037899974811717, 1e-8);
        }
        [Test]
        public void Clebsch15()
        {
            Test("clebsch(0.5, 0.5)(1, 5)", 1.0);
        }
        [Test]
        public void Clebsch55()
        {
            Test("clebsch(0.5, 0.5)(5, 5)", 1.0 / Math.Sqrt(2.0));
        }
        [Test]
        public void Legendre()
        {
            Test("legendre(3, 1)", 1.0);
        }
        [Test]
        public void Hermite()
        {
            Test("hermite(3, 2)", 40.0);
        }
        [Test]
        public void Laguerre()
        {
            Test("laguerre(2, 2)", -1.0, 1e-8);
        }
        [Test]
        public void Zernike1105()
        {
            Test("zernike(1, 1, 0.5)", 0.5);
        }
        [Test]
        public void Zernike2005()
        {
            Test("zernike(2, 0, 0.5)", 2.0 * 0.25 - 1.0);
        }
        [Test]
        public void Gegenbauer()
        {
            Test("gegenbauer(1, 0.5, 0.25)", 2.0 * 0.5 * 0.25);
        }
        [Test]
        public void Polylog03()
        {
            Test("polylog(0, 3)", -1.5);
        }
        [Test]
        public void Polylog10()
        {
            Test("polylog(1, 0)", 0.0);
        }
        [Test]
        public void Polylog21()
        {
            Test("polylog(2, 1)", Math.PI * Math.PI / 6.0);
        }
        [Test]
        public void Polylog31()
        {
            Test("polylog(3, 1)", 1.2020569031595945, 1e-8);
        }
        [Test]
        public void Polylog32()
        {
            Test("polylog(-3, 2)", 26.0);
        }
        [Test]
        public void Polylog52()
        {
            Test("polylog(-5, 2)", 1082.0000000231821, 1e-8);
        }
        [Test]
        public void Polylog901()
        {
            Test("polylog(-9, 0.1)", 86.621357524537643, 1e-8);
        }
        [Test]
        public void Hzeta()
        {
            Test("hzeta(3, 1)", 1.20205690315959428, 1e-8);
        }
    }
}
