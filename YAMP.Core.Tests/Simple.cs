namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Simple : Base
    {
        [Test]
        public void Subtraction()
        {
            Test("2-3-4", -5.0);
        }
        [Test]
        public void PowerAdditionAndDivides()
        {
            Test("(10^6*27)/8/1024", (27000000.0 / 8.0) / 1024.0);
        }
        [Test]
        public void NegateAndAdd()
        {
            Test("-2+3", 1.0);
        }
        [Test]
        public void FloatingPoint()
        {
            Test("2.2", 2.2);
        }
        [Test]
        public void AbbreviatedFloatingPointAddAndSubtract()
        {
            Test(".2+4-4+.2", 0.4);
        }
        [Test]
        public void DivideAndAddAndMultiply()
        {
            Test("3/3+1*4-5", 0.0);
        }
        [Test]
        public void IntegerArithmetic()
        {
            Test("7*3+5-13+2", 15.0);
        }
        [Test]
        public void BracketsAndDivide()
        {
            Test("(7*3+5-13+2)/3", 5.0);
        }
        [Test]
        public void ExactDivideByPrimeThree()
        {
            Test("15/3-5", 0.0);
        }
        [Test]
        public void BracketAndSubtractFollowRules()
        {
            Test("(7*3+5-13+2)/3-5", 0.0);
        }
        [Test]
        public void PowerOperationAndAdd()
        {
            Test("2^3+3", 11.0);
        }
        [Test]
        public void AddAndPowerOperation()
        {
            Test("3+2^3", 11.0);
        }
        [Test]
        public void MultiplyAndPowerOperation()
        {
            Test("2*3^3+7/2+1-4/2", 56.5);
        }
        [Test]
        public void PowerOperationWithBracket()
        {
            Test("2*3^(3-1)+7/2+1-4/2", 20.5);
        }
        [Test]
        public void BracketAndPowerOperation()
        {
            Test("(2-1)*3^(3-1)+7/2+1-4/2", 11.5);
        }
        [Test]
        public void MultiplyWithAbsoluteValue()
        {
            Test("3*|-2|*5", 30.0);
        }
        [Test]
        public void FactorialAndSubtract()
        {
            Test("5!-1000/5+4*20", 0.0);
        }
        [Test]
        public void ChainedPower()
        {
            Test("2^2^2^2", 65536.0);
        }
        [Test]
        public void DivideByBracket()
        {
            Test("2-(3*5)^2+7/(2-8)*2", -225.0 - 1.0 / 3.0);
        }
        [Test]
        public void NegativePowerWithoutBracket()
        {
            Test("-2^2", -4.0);
        }
        [Test]
        public void NegateAndSubtract()
        {
            Test("-2-2", -4.0);
        }
        [Test]
        public void NegativeMultiplyWithoutBracket()
        {
            Test("-2*2", -4.0);
        }
        [Test]
        public void NegatePowerAndAdd()
        {
            Test("-2^2+4", 0.0);
        }
        [Test]
        public void UnitMultiplication()
        {
            Test("3-4*1-1", -2.0);
        }
        [Test]
        public void UnitPower()
        {
            Test("3-4^1-1", -2.0);
        }
        [Test]
        public void DivideChained()
        {
            Test("2/2/2", 0.5);
        }
        [Test]
        public void PowerAndPower()
        {
            Test("2^2^2", 16.0);
        }
        [Test]
        public void StrangeNumberCombination()
        {
            Test("0.212410080106903 * 500-0.00654415361812242 * 500-0.0337905933677912 * 500-0.182007882231707 * 500+131.208072980527", 126.2417984251682);
        }
        [Test]
        public void NegativeSquaredInBracket()
        {
            Test("(1 - (-1)^2)^0.5", 0.0);
        }
        [Test]
        public void NegativeSquaredAlright()
        {
            Test("(-25)^2", 625.0);
        }
        [Test]
        public void RaisedToNegativePower()
        {
            Test("2^-2", 0.25);
        }
        [Test]
        public void RaisedInverseChained()
        {
            Test("2^-1^-1", 0.5);
        }
        [Test]
        public void RaisedInverseWithBrackets()
        {
            Test("(2^-1)^-1", 2.0);
        }
        [Test]
        public void NegativeSquaredWithBracket()
        {
            Test("(-5)^2", 25.0);
        }
        [Test]
        public void PositiveSquaredWithBracket()
        {
            Test("(5)^2", 25.0);
        }
        [Test]
        public void NegativeSquaredBracketInteger()
        {
            Test("(-75)^2", Math.Pow(75.0, 2.0));
        }
    }
}
