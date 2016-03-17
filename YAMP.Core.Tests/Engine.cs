namespace YAMP.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Engine : Base
    {
        [Test]
        public void ParseMandelbrotFunctionCallMissingArgumentShouldFail()
        {
            Test("mandelbrot(-2,,-2,2,100,100)", true);
        }

        [Test]
        public void ParseMatrixLiteralAndIndex()
        {
            Test("[1,2,3](2)", false);
        }

        [Test]
        public void ParseNegativePower()
        {
            Test("2^-2", false);
        }

        [Test]
        public void ParsePowerPowerShouldFail()
        {
            Test("2^^2", true);
        }

        [Test]
        public void ParseOpenBracketMissingRestShouldFail()
        {
            Test("function f(", true);
        }

        [Test]
        public void ParseStandaloneMemberOperatorShouldFail()
        {
            Test(".", true);
        }

        [Test]
        public void ParseStandalonePlusOperatorShouldFail()
        {
            Test("+", true);
        }

        [Test]
        public void ParseStandaloneNegateOperatorShouldFail()
        {
            Test("~", true);
        }

        [Test]
        public void ParsedForStatementMissingOperandsShouldFail()
        {
            Test("for() { }", true);
        }

        [Test]
        public void ParseForStatementMissingBlockShouldFail()
        {
            Test("for() { ", true);
        }

        [Test]
        public void ParseForStatementMissingRestShouldFail()
        {
            Test("for(k=0 { }", true);
        }

        [Test]
        public void ParseForStatementCommasShouldFail()
        {
            Test("for(k=0, k != 2, k++) { }", true);
        }

        [Test]
        public void ParseForStatement()
        {
            Test("for(k = 0; k != 2; k++) { }", false);
        }

        [Test]
        public void ParseFunctionCallWithMissingArgumentsShouldFail()
        {
            Test("newton(", true);
        }

        [Test]
        public void ParseDoLoopMissingBlockShouldFail()
        {
            Test("do{ ", true);
        }

        [Test]
        public void ParseDoBlock()
        {
            Test("do{ }", false);
        }

        [Test]
        public void ParseWhileStatementConditionMissingShouldFail()
        {
            Test("while() {}", true);
        }

        [Test]
        public void ParseWhileStatement()
        {
            Test("while(true) {}", false);
        }

        [Test]
        public void ParseDoWhileStatement()
        {
            Test("do {} while(0 > 2)", false);
        }

        [Test]
        public void ParseIndexOperatorMissingShouldFail()
        {
            Test("a([4)", true);
        }

        [Test]
        public void ParseCallOperatorMissingShouldFail()
        {
            Test("f(", true);
        }

        [Test]
        public void LoopToStoreDataInMatrix()
        {
            Test("x = []; y = 0; for(k = 1; k <= 10; k+=1) { y+=k; x(k) = y; } x(10) - x(9)", 10.0);
        }

        [Test]
        public void ConditionToPlaceValueInVariable()
        {
            Test("x = 9; y = 5; if(x > y) { t = x; x = y; y = t; } y - x", 4.0);
        }
    }
}
