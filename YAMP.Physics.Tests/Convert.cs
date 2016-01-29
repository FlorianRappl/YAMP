namespace YAMP.Physics.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Convert : Base
    {
        [Test]
        public void ConvertUnitMeterPerSecondToKmPerHour()
        {
            Test("convert(1, \"m / s\", \"km / h\")", 3.6);
        }
        [Test]
        public void ConvertUnitElectronVoltToJoule()
        {
            Test("convert(1, \"eV\", \"J\") - Q * unit(1, \"V\")", 0.0);
        }
        [Test]
        public void ConvertUnitMeterToFeet()
        {
            Test("convert(1, \"m\", \"ft\")", 3.2808);
        }
    }
}
