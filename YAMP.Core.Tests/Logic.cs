namespace YAMP.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Logic : Base
    {
        [Test]
        public void EqualsFits()
        {
            Test("1 == 1", 1.0);
        }
        [Test]
        public void EqualsFail()
        {
            Test("1 == 0", 0.0);
        }
        [Test]
        public void NotEqualsFits()
        {
            Test("1 ~= 0", 1.0);
        }
        [Test]
        public void NotEqualsFail()
        {
            Test("1 ~= 1", 0.0);
        }
        [Test]
        public void AndFitsOne()
        {
            Test("1 && 1", 1.0);
        }
        [Test]
        public void AndFailLeft()
        {
            Test("1 && 0", 0.0);
        }
        [Test]
        public void AndFailRight()
        {
            Test("0 && 1", 0.0);
        }
        [Test]
        public void AndFailsZero()
        {
            Test("0 && 0", 0.0);
        }
        [Test]
        public void OrFitsOne()
        {
            Test("1 || 1", 1.0);
        }
        [Test]
        public void OrFitsRight()
        {
            Test("0 || 1", 1.0);
        }
        [Test]
        public void OrFailsZero()
        {
            Test("0 || 0", 0.0);
        }
        [Test]
        public void OrFitsLeft()
        {
            Test("1 || 0", 1.0);
        }
        [Test]
        public void AbsoluteOfMatrixSmaller()
        {
            Test("|[1,2,3,4,5,6,7] < 5|", 2.0);
        }
        [Test]
        public void Greater()
        {
            Test("17>12", 1.0);
        }
        [Test]
        public void Smaller()
        {
            Test("7<-1.5", 0.0);
        }
        [Test]
        public void NegativeNumberEquals()
        {
            Test("2+i==2-i", 0.0);
        }
        [Test]
        public void NegativeNumberEqualsNot()
        {
            Test("3-i~=4", 1.0);
        }
    }
}
