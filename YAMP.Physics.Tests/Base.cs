namespace YAMP.Physics.Tests
{
    using NUnit.Framework;
    using System;

    public abstract class Base
    {
        static Base()
        {
            Parser.Load();
            Parser.LoadPlugin(typeof(UnitValue).Assembly);
        }

        protected void Test(String query, Double result, Double prec = 0.0)
        {
            var parser = Parser.Parse(query);
            var value = parser.Execute();
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real, prec);
        }
    }
}
