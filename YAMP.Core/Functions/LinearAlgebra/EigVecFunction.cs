namespace YAMP
{
    using YAMP.Numerics;

    [Description("EigVecFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class EigVecFunction : ArgumentFunction
    {
        [Description("EigVecFunctionDescriptionForMatrix")]
        [Example("eigvec([1,2,3;4,5,6;7,8,9])", "EigVecFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            var ev = new Eigenvalues(M as MatrixValue);
            return ev.GetV();
        }
    }
}
