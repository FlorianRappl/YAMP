namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    public abstract class Base
    {
        protected void Test(String query, Double result, Double prec = 0.0)
        {
            var parser = new Parser();
            parser.UseScripting = true;
            var value = parser.Evaluate(query);
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real, prec);
        }

        protected void Test(String query, Boolean hasErrors)
        {
            var parser = new Parser();
            parser.UseScripting = true;
            var context = parser.Parse(query);
            var value = context.Parser.HasErrors;
            Assert.AreEqual(hasErrors, value);
        }
    }
}
