namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Constants : Base
    {
        [Test]
        public void ExponentialAndPiCube()
        {
            Test("e^3-pi^3", Math.Pow(Math.E, 3.0) - Math.Pow(Math.PI, 3.0));
        }
        [Test]
        public void NegatePi()
        {
            Test("-pi", -Math.PI);
        }
    }
}
