namespace YAMP.Sets.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    public abstract class Base
    {
        protected void TestNoException(String query)
        {
            Assert.DoesNotThrow(() =>
            {
                var parser = new Parser();
                parser.LoadPlugin(typeof(SetsPlugin).Assembly);
                parser.UseScripting = true;
                var value = parser.Evaluate(query);
            });
        }

        protected void TestValue(String query, SetValue result)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(SetsPlugin).Assembly);
            parser.UseScripting = true;
            var value = parser.Evaluate(query);
            Assert.AreEqual(result, value);
        }

        protected void TestValue(String query, Value result)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(SetsPlugin).Assembly);
            parser.UseScripting = true;
            var value = parser.Evaluate(query);
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real);
        }

        protected void TestValue(String query, double result)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(SetsPlugin).Assembly);
            parser.UseScripting = true;
            var value = parser.Evaluate(query);
            var real = ((ScalarValue)value).Re;
            Assert.AreEqual(result, real);
        }

        protected void TestValue(String query, MatrixValue result)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(SetsPlugin).Assembly);
            parser.UseScripting = true;
            var value = parser.Evaluate(query);
            var mat = ((MatrixValue)value);
            Assert.AreEqual(result, mat);
        }


        protected void TestExpression(String query, Boolean hasErrors)
        {
            var parser = new Parser();
            parser.LoadPlugin(typeof(SetsPlugin).Assembly);
            parser.UseScripting = true;
            var context = parser.Parse(query);
            var value = context.Parser.HasErrors;
            Assert.AreEqual(hasErrors, value);
        }


        //protected void Test(String query, String[] unknownVariables)
        //{
        //    var parser = new Parser();
        //    parser.LoadPlugin(typeof(SetsPlugin).Assembly);
        //    var context = parser.Parse(query);
        //    var variables = context.Variables.Where(m => !m.IsAssigned).Select(m => m.Name).ToList();
        //    CollectionAssert.AreEquivalent(unknownVariables, variables);
        //}
    }
}
