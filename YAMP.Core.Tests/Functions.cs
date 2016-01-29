namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Functions : Base
    {
        [Test]
        public void Ceiling()
        {
            Test("ceil(2.5)", 3.0);
        }
        [Test]
        public void Floor()
        {
            Test("floor(2.5)", 2.0);
        }
        [Test]
        public void ExponentialZero()
        {
            Test("exp(0)*10-5", 5.0);
        }
        [Test]
        public void ExponentialThree()
        {
            Test("exp(3)*10", 10.0 * Math.Exp(3));
        }
        [Test]
        public void Cosine()
        {
            Test("cos(pi)", -1.0);
        }
        [Test]
        public void Sine()
        {
            Test("sin(pi/2)", 1.0);
        }
        [Test]
        public void Trigonometric()
        {
            Test("cos(0)*exp(0)*sin(3*pi/2)", -1.0);
        }
        [Test]
        public void Maximum()
        {
            Test("|max([1,5,7,9,8+5i])|", Math.Sqrt(89.0));
        }
        [Test]
        public void Trace()
        {
            Test("trace(inv([1, 2, 3; 4, 5, 6; 7, 8, 10]))", 4.0, 1e-8);
        }
        [Test]
        public void Polyval()
        {
            Test("polyval([1 2 3], 5)", 86.0, 1e-8);
        }
        [Test]
        public void Polyfit()
        {
            Test("sum(polyfit([0, 1, 2, 3, 4, 5], [3, 3, 5, 9, 15, 23], 2))", 3.0, 1e-8);
        }
        [Test]
        public void ZetaOfHalf()
        {
            Test("zeta(0.5)", -1.4603545088095868, 1e-8);
        }
        [Test]
        public void ZetaOfZero()
        {
            Test("zeta(0)", -0.5, 1e-8);
        }
        [Test]
        public void EvaluateAddition()
        {
            Test("eval(\"2+3\")", 5.0);
        }
        [Test]
        public void EvaluateAbsolute()
        {
            Test("eval(\"|[1,2,3]|\")", Math.Sqrt(14.0));
        }
        [Test]
        public void AbsoluteWithRangeIndex()
        {
            Test("abs([1,2,3;4,5,6;7,8,9](1,:))", Math.Sqrt(14.0));
        }
        [Test]
        public void LengthOfHelp()
        {
            Test("length(help()) > 0", 1.0);
        }
        [Test]
        public void Absolute()
        {
            Test("abs(3+4i)", 5.0);
        }
        [Test]
        public void Eigen()
        {
            Test("eig([1,2;4,5])(1)(1)", 3.0 - Math.Sqrt(12));
        }
        [Test]
        public void EigenValue()
        {
            Test("eigval([1,2;4,5])(1)", 3.0 - Math.Sqrt(12));
        }
        [Test]
        public void ComplexCosine()
        {
            Test("|cos(1+i)|", 1.2934544550420957, 1e-8);
        }
        [Test]
        public void ArcCos()
        {
            Test("|arccos(0.83373002513-0.988897705i)|", 1.4142135618741407, 1e-8);
        }
        [Test]
        public void Ode()
        {
            Test("ode((t, x) => -x, 0:0.01:1, 1.0)(101, 2)", 0.36818409421192455, 1e-8);
        }
        [Test]
        public void Root()
        {
            Test("root(x => x.^2-4, 3)", 2.0, 1e-8);
        }
        [Test]
        public void AnyOne()
        {
            Test("any([1, 0; 0, 0])", 1.0);
        }
        [Test]
        public void AllOne()
        {
            Test("all([1, 0; 0, 0])", 0.0);
        }
        [Test]
        public void AnyZero()
        {
            Test("any([0, 0; 0, 0])", 0.0);
        }
        [Test]
        public void AllNonZero()
        {
            Test("all([1, 1; 5, 3])", 1.0);
        }
        [Test]
        public void Fft()
        {
            Test("length(fft(ones(31,1)))", 31.0);
        }
        [Test]
        public void Bessel()
        {
            Test("bessel(2, 4.5)", 0.21784898358785076, 1e-8);
        }
        [Test]
        public void Erf()
        {
            Test("erf(1.4)", 0.9522851197626383, 1e-8);
        }
    }
}
