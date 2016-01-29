namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public abstract class Base
    {
        static Base()
        {
            Parser.Load();
        }

        protected void Test(String query, Double result, Double prec = 0.0)
        {
            var parser = Parser.Parse(query);
            var value = parser.Execute();
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real, prec);
        }

        protected void Test(String query, Boolean hasErrors)
        {
            var parser = Parser.Parse(query);
            var value = parser.Context.Parser.HasErrors;
            Assert.AreEqual(hasErrors, value);
        }
    }
}
