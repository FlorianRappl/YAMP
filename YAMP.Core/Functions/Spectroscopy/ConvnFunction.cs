namespace YAMP
{
    [Description("ConvnFunctionDescription")]
    [Link("ConvnFunctionLink")]
    [Kind(PopularKinds.Function)]
    sealed class ConvnFunction : ArgumentFunction
    {
        [Description("ConvnFunctionDescriptionForMatrixMatrix")]
        [Example("convn([0,0,5,5,0,0], [0,0,3,3,0,0])", "ConvnFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue A, MatrixValue B)
        {
            //TODO: Additional options like ( http://www.mathworks.de/de/help/matlab/ref/convn.html )
            //i.e. "full" (default - it is this), "same" (only same length), "valid" (see formula on the website)
            var result = new MatrixValue(A.Length + B.Length - 1, 1);

            for (var i = 1; i <= A.Length; i++)
            {
                var k = i;

                for (var j = 1; j <= B.Length; j++)
                {
                    result[k++] += A[i] * B[j];
                }
            }

            return result;
        }
    }
}
