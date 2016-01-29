namespace YAMP.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Converter : Base
    {
        [Test]
        public void CastStringToNumber()
        {
            Test("cast(\"3.5\")", 3.5);
        }
        [Test]
        public void ConvertBinaryToDecimal11()
        {
            Test("bin2dec(\"1011\")", 11.0);
        }
        [Test]
        public void ConvertBinaryToDecimal3()
        {
            Test("bin2dec(\"0011\")", 3.0);
        }
        [Test]
        public void ConvertHexadecimalToDecimal255()
        {
            Test("hex2dec(\"Ff\")", 255.0);
        }
        [Test]
        public void ConvertHexadecimalToDecimal291()
        {
            Test("hex2dec(\"123\")", 291.0);
        }
        [Test]
        public void ConvertOctetToDecimal919()
        {
            Test("oct2dec(\"1627\")", 919.0);
        }
        [Test]
        public void ConvertOctetToDecimal63()
        {
            Test("oct2dec(\"77\")", 63.0);
        }
    }
}
