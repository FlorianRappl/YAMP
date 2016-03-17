namespace YAMP.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Parsing : Base
    {
        [Test]
        public void LineComment()
        {
            Test("2+3//This is a line-comment!\n-4", 1.0);
        }

        [Test]
        public void BlockComment()
        {
            Test("1-8* /* this is another comment */ 0.25", -1.0);
        }

        [Test]
        public void BlockCommentWithNewLines()
        {
            Test("1-8* /* this is \nanother comment\nwith new lines */ 0.5+4", 1.0);
        }

        [Test]
        public void LambdaExpressionWithInteger()
        {
            Test("f = x => x.^2; f(2)", 4.0);
        }

        [Test]
        public void LambdaExpressionWithMatrices()
        {
            Test("f = (x, y) => x*y'; f([1,2,3],[1,2,3])", 14.0);
        }

        [Test]
        public void MatrixWithNewLines()
        {
            Test("sum(size([\n1 2 3 4\n5 6 7 8]))", 6.0);
        }
    }
}
