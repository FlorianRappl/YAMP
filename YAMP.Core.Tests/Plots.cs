namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Plots : Base
    {
        [Test]
        public void PolarPlotForTrigFunctions()
        {
            Test("polar(-pi/4:0.1:pi/4, [sin(-pi/4:0.1:pi/4), cos(-pi/4:0.1:pi/4), tan(-pi/4:0.1:pi/4)])", 3);
        }
        [Test]
        public void LinearPlotForPowers()
        {
            Test("plot([0:10, 2^(0:2:20), 2^(1:2:21)])", 2);
        }

        void Test(String query, Int32 number)
        {
            var parser = new Parser();
            var value = parser.Evaluate(query);
            var plot = (PlotValue)value;
            Assert.AreEqual(number, plot.Count);
        }
    }
}
