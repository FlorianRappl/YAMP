using System;

namespace YAMP
{
    [Description("In mathematics and, in particular, functional analysis, convolution is a mathematical operation on two functions f and g, producing a third function that is typically viewed as a modified version of one of the original functions, giving the area overlap between the two functions as a function of the amount that one of the original functions is translated. Convolution is similar to cross-correlation.")]
    [Link("http://en.wikipedia.org/wiki/Convolution")]
    [Kind(PopularKinds.Function)]
    sealed class ConvnFunction : ArgumentFunction
    {
        [Description("Performs the convolution of two vextors of data, A and B. The result has the length length(A) + length(B) - 1.")]
        [Example("convn([0,0,5,5,0,0], [0,0,3,3,0,0])", "The convolution of two rectangle signals is a triangle signal.")]
        public MatrixValue Function(MatrixValue A, MatrixValue B)
        {
            //TODO: Additional options like ( http://www.mathworks.de/de/help/matlab/ref/convn.html )
            //i.e. "full" (default - it is this), "same" (only same length), "valid" (see formula on the website)
            var result = new MatrixValue(A.Length + B.Length - 1, 1);

            for (var i = 1; i <= A.Length; i++)
            {
                var k = i;

                for (var j = 1; j <= B.Length; j++)
                    result[k++] += A[i] * B[j];
            }

            return result;
        }
    }
}
