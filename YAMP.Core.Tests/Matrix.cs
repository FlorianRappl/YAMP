namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class Matrix : Base
    {
        [Test]
        public void AbsoluteValueOfSubtractedMatrices()
        {
            Test("|[2,3,1]-[1,3,1]|", 1.0);
        }
        [Test]
        public void ScalarArithmeticsInMatrixLiteral()
        {
            Test("|[2^2,2+3,-2,-2]|", 7.0);
        }
        [Test]
        public void MatrixMultiplication()
        {
            Test("|[3,2,1]*[1;2;3]|", 10.0);
        }
        [Test]
        public void DifferentDimensionSameValue()
        {
            Test("|[1;2;3]|-|[1,2,3]|", 0.0);
        }
        [Test]
        public void ScalarTimesMatrix()
        {
            Test("|2*[1,2;1,2]|", 0.0);
        }
        [Test]
        public void SubtractMatrices()
        {
            Test("|[2,1;3,5]-[2,1;3,5]'|", 4.0);
        }
        [Test]
        public void MultiplyVectors()
        {
            Test("[2,1,0]*[5;2;1]", 12.0);
        }
        [Test]
        public void Det()
        {
            Test("det([1;2]*[2,1])", 0.0);
        }
        [Test]
        public void RangeWithExplicitDelta()
        {
            Test("|1:1:3|", Math.Sqrt(1 + 4 + 9));
        }
        [Test]
        public void CommaIsNewColumn()
        {
            Test("|[1,2,3]|", Math.Sqrt(1 + 4 + 9));
        }
        [Test]
        public void SemicolonIsNewLine()
        {
            Test("|[1;2;3]|", Math.Sqrt(1 + 4 + 9));
        }
        [Test]
        public void SubtractionOfMatrices()
        {
            Test("([3 2 1] - [1 2 3])(3)", -2.0);
        }
        [Test]
        public void AdditionOfMatrices()
        {
            Test("([3 2 1] + [1 2 3])(2)", 4.0);
        }
        [Test]
        public void Cholesky()
        {
            Test("det(chol([1, 1i; -1i, pi]))", Math.Sqrt(Math.PI - 1.0), 1e-8);
        }
        [Test]
        public void AbsoluteValue()
        {
            Test("|[2 3 4]|", Math.Sqrt(29.0));
        }
        [Test]
        public void NewLineIsSignificant()
        {
            Test("[2 3 4\n1 2 3](2, 2)", 2.0);
        }
        [Test]
        public void SpaceIsSignificant()
        {
            Test(" [2 2 * 2 - 2^3 4](1, 2) ", -4.0);
        }
        [Test]
        public void TwoDimensionalIndex()
        {
            Test("[1,2,3;4,5,6;7,8,9](2,3)", 6.0);
        }
        [Test]
        public void AssignToVariableAndAbsolute()
        {
            Test("|(x=[1,2,3;4,5,6;7,8,9])(:,1)|", Math.Sqrt(66.0));
        }
        [Test]
        public void AbsoluteOfEigenvalue()
        {
            Test("abs(eigval([1,2;4,5])(1:2))", 6.48074069840786, 1e-8);
        }
        [Test]
        public void AbsoluteOfEigenvector()
        {
            Test("abs(eigvec([1,2;4,5])(1:2))", 1.0);
        }
        [Test]
        public void SineOfMatrix()
        {
            Test("-sin([1,2,3])(2)", -Math.Sin(2));
        }
        [Test]
        public void SumOfPowers()
        {
            Test("sum([0:10, 2^(0:2:20), 2^(1:2:21)](:,1))", 55.0);
        }
        [Test]
        public void LengthOfRange()
        {
            Test("length(-pi/4:0.1:pi/4)", 16.0);
        }
        [Test]
        public void AssignMultiple()
        {
            Test("[a, b, c] = 12.0;b", 12.0);
        }
        [Test]
        public void Sort()
        {
            Test("sort([25, 1, 0, 29, 105, 0, -5])(4)", 1.0);
        }
        [Test]
        public void LuAndSum()
        {
            Test("A=[4, 3; 6, 3]; [L, U, p] = lu(A); sum(sum(A - p * L * U))", 0.0, 1e-8);
        }
        [Test]
        public void QrAndDet()
        {
            Test("A=[12,-51,4;6,167,-68;-4,24,-41]; [q, r] = qr(A); det(q)", -1.0, 1e-8);
        }
    }
}
