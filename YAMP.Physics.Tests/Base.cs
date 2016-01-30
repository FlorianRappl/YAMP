namespace YAMP.Physics.Tests
{
    using NUnit.Framework;
    using System;

    public abstract class Base
    {
        protected void Test(String query, Double result, Double prec = 0.0)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(UnitValue).Assembly);
            var value = parser.Evaluate(query);
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real, prec);
        }
    }
}
