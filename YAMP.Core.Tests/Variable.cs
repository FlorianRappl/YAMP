namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class Variable : Base
    {
        [Test]
        public void SubtractFourOf2xFor2()
        {
            Test("2*x-4", "x", 2.0, 0.0);
        }
        [Test]
        public void SubtractFourOf2xInterval()
        {
            Test("2*x-4", -2.0, 2.0, x => 2 * x - 4.0);
        }
        [Test]
        public void XFactorial()
        {
            Test("x!", "x", 5.0, 120.0);
        }
        [Test]
        public void XSquareMinusThreeX()
        {
            Test("x^2-3*x/4", "x", 0.75, 0.0);
        }
        [Test]
        public void XIsTheValueOfYPlus2()
        {
            Test("X=(Y=5)+2", 7.0);
        }
        [Test]
        public void YIsThePreviouslySetValue()
        {
            Test("X=(Y=5)+2;Y", 5.0);
        }

        void Test(String query, String name, Double val, Double result, Double prec = 0.0)
        {
            var parser = new Parser();
            var argument = new Dictionary<String, Value>
            {
                { name, new ScalarValue(val) }
            };
            var value = parser.Evaluate(query, argument);
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real, prec);
        }

        void Test(String query, Double xmin, Double xmax, Func<Double, Double> compare)
        {
            var parser = new Parser();
            var x = xmin;
            var success = 0;
            var total = 0;

            while (x < xmax)
            {
                var value = parser.Evaluate(query, new { x });

                if (value.Equals(compare(x)))
                {
                    ++success;
                }

                ++total;
                x += 0.1;
            }

            Assert.AreEqual(success, total);
        }
    }
}
