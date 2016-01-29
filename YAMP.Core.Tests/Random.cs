namespace YAMP.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Random : Base
    {
        [Test]
        public void JackknifeStatisticsOfNormalRandomAverage()
        {
            Test("x = round(sum(sum([1, 0; 0, 100] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, avg)))); sum([x < 29, x > 18]) / 2", 1.0);
        }
        [Test]
        public void JackknifeStatisticsOfNormalRandomVariance()
        {
            Test("x = round(sum(sum([1, 0; 0, 10] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, var)*[10,0;0,1]))); sum([x < 24, x > 16]) / 2", 1.0);
        }
        [Test]
        public void NormalRandWithDoubleSum()
        {
            Test("sum(sum(round(cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)]))))", 2.0);
        }
        [Test]
        public void NormalRandUsedWithSumRounded()
        {
            Test("sum(round(acor(3 + randn(100, 1))))", 1.0);
        }
        [Test]
        public void NormalRandUsedInStatistics()
        {
            Test("x = [3 + randn(100, 1), 10 + 2 * randn(100, 1)]; sum(sum(Bootstrap(x, 200, avg) - Jackknife(x, 20, avg))) < 0.1", 1.0);
        }
        [Test]
        public void RoundNormalRand()
        {
            Test("round(sum(randn(10000, 1)) / 1000)", 0.0);
        }
        [Test]
        public void RoundUniformRand()
        {
            Test("round(sum(rand(10000, 1)) / 1000)", 5.0);
        }
        [Test]
        public void RoundIntegerRand()
        {
            Test("round(sum(randi(10000, 1, 1, 9)) / 10000)", 5.0);
        }
        [Test]
        public void SolveRandomSystem()
        {
            Test("A = rand(3); A(2, 2) = 0.5i; x = solve(A, eye(3,1)); abs(A * x - eye(3,1))", 0.0, 1e-8);
        }
    }
}
