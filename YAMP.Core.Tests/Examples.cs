namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using YAMP.Help;

    [TestFixture]
    public class Examples : Base
    {
        [Datapoints]
        public String[] examples = ObtainExamples();

        [Theory]
        public void ExampleShouldBeValidYampCode(String example)
        {
            var parser = new Parser();
            var query = parser.Parse(example);
            Assert.IsFalse(query.Parser.HasErrors);
        }

        [Test]
        public void DefaultLocalizationIsWellConnected()
        {
            var types = typeof(StandardFunction).Assembly.GetTypes();
            var localization = Localization.Default;

            foreach (var type in types.Where(m => m.GetCustomAttribute<YAMP.DescriptionAttribute>() != null))
            {
                var key = type.GetCustomAttribute<YAMP.DescriptionAttribute>().DescriptionKey;
                Assert.IsTrue(localization.ContainsKey(key),
                            "Missing key {0} in {1}", key, type.Name);

                var methods = type.GetMethods().Where(m => m.GetCustomAttribute<YAMP.DescriptionAttribute>() != null);

                foreach (var method in methods)
                {
                    var description = method.GetCustomAttribute<YAMP.DescriptionAttribute>().DescriptionKey;

                    Assert.IsTrue(localization.ContainsKey(description),
                            "Missing description {0} in method {1} of {2}", description, method.Name, type.Name);

                    var examples = method.GetCustomAttributes<ExampleAttribute>();
                    var returns = method.GetCustomAttributes<ReturnsAttribute>();

                    foreach (var example in examples)
                    {
                        Assert.IsTrue(localization.ContainsKey(example.DescriptionKey), 
                            "Missing example {0} in method {1} of {2}", example.DescriptionKey, method.Name, type.Name);
                    }

                    foreach (var ret in returns)
                    {
                        Assert.IsTrue(localization.ContainsKey(ret.ExplanationKey), 
                            "Missing return {0} in method {1} of {2}", ret.ExplanationKey, method.Name, type.Name);
                    }
                }
            }
        }

        static String[] ObtainExamples()
        {
            var parser = new Parser();
            var doc = Documentation.Create(parser.Context);
            var queries = new List<String>();

            foreach (var section in doc.Sections)
            {
                if (section is HelpFunctionSection)
                {
                    var f = (HelpFunctionSection)section;

                    foreach (var usage in f.Usages)
                    {
                        foreach (var example in usage.Examples)
                        {
                            if (!example.IsFile)
                            {
                                queries.Add(example.Example);
                            }
                        }
                    }
                }
            }

            return queries.ToArray();
        }
    }
}
