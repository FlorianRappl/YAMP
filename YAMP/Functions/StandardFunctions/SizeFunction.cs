using System;

namespace YAMP
{
    [Description("Outputs the dimensions (size) of the given object.")]
    [Kind(PopularKinds.Function)]
    sealed class SizeFunction : ArgumentFunction
    {
        [Description("Returns a row vector containing the number of rows (1, 1) and the number of columns (1, 2).")]
        [Example("size([1,2,3,4,5])", "Results in a vector with the elements 1 and 5, since we have 5 columns and 1 row.")]
        [Example("size(rand(2))", "Results in a vector with the elements 2 and 2, since we have 2 columns and 2 rows.")]
        public MatrixValue Function(MatrixValue M)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(M.DimensionY);
            m[1, 2] = new ScalarValue(M.DimensionX);
            return m;
        }

        [Description("Returns a row vector containing the number of rows (1, 1) and the number of columns (1, 2), which is 1 and 1 for any scalar.")]
        public MatrixValue Function(ScalarValue x)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(1);
            m[1, 2] = new ScalarValue(1);
            return m;
        }

        [Description("Returns a row vector containing the number of rows (1, 1) and the number of columns (1, 2), which is 1 and the number of characters for any string.")]
        [Example("size(\"hello\")", "Results in a vector with the elements 1 and 5, since we have 5 charaters.")]
        public MatrixValue Function(StringValue str)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(1);
            m[1, 2] = new ScalarValue(str.Value.Length);
            return m;
        }
    }
}
