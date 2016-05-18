namespace YAMP.Functions.Helpers
{
    [Description("ObjectFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class ObjectFunction : ArgumentFunction
    {
        [Description("ObjectFunctionDescriptionForVoid")]
        [Example("object()", "ObjectFunctionExampleForVoid1")]
        public ObjectValue Function()
        {
            return new ObjectValue();
        }
    }
}
