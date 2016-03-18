namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

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

        protected void Test(String query, String[] unknownVariables)
        {
            var parser = new Parser();
            var context = parser.Parse(query);
            var variables = context.Variables.Where(m => !m.IsAssigned).Select(m => m.Name).ToList();
            CollectionAssert.AreEquivalent(unknownVariables, variables);
        }
    }
}
