namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Analyzer : Base
    {
        [Test]
        public void ReadUnknownVariable()
        {
            var source = "a";
            Test(source, new[] { "a" });
        }

        [Test]
        public void ReadKnownVariable()
        {
            var source = "a = 1";
            Test(source, new String[] { });
        }

        [Test]
        public void ReadKnownAndUnknownVariablesInGlobalScope()
        {
            var source = "d = 5; a = b + c * d";
            Test(source, new String[] { "b", "c" });
        }

        [Test]
        public void ReadUnknownVariableInLocalScope()
        {
            var source = "f = (x) => x * y; g = f(z)";
            Test(source, new String[] { "y", "z" });
        }
    }
}
