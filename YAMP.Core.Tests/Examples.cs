namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
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
