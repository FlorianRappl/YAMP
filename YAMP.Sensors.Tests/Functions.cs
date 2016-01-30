namespace YAMP.Sensors.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Functions : Base
    {
        [Test]
        public void AccelerationWorksAndDoesNotChangeConsecutively()
        {
            Test("|acc() - acc()|", 0.0);
        }
        [Test]
        public void GpsWorksAndDoesNotChangeConsecutively()
        {
            Test("|gps() - gps()|", 0.0);
        }
        [Test]
        public void CompassWorksAndDoesNotChangeConsecutively()
        {
            Test("|comp() - comp()|", 0.0);
        }
        [Test]
        public void LightnessWorksAndDoesNotChangeConsecutively()
        {
            Test("|light() - light()|", 0.0);
        }
        [Test]
        public void InclineWorksAndDoesNotChangeConsecutively()
        {
            Test("|inc() - inc()|", 0.0);
        }
        [Test]
        public void GyroWorksAndDoesNotChangeConsecutively()
        {
            Test("|gyro() - gyro()|", 0.0);
        }
        [Test]
        public void OrientationWorksAndDoesNotChangeConsecutively()
        {
            Test("|orient() - orient()|", 0.0);
        }
        [Test]
        public void OrientIsExpected3x3Matrix()
        {
            Test("A=size(orient());sum(A)", 6.0);
        }
        [Test]
        public void GyroIsExpected3x1Matrix()
        {
            Test("A=size(gyro());sum(A)", 4.0);
        }
    }
}
