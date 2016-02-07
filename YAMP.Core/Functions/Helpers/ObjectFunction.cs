namespace YAMP.Functions.Helpers
{
    [Description("Creates an empty object.")]
    [Kind(PopularKinds.Function)]
    sealed class ObjectFunction : ArgumentFunction
    {
        [Description("Creates an empty object without any keys.")]
        [Example("object()", "Returns an empty object that can represents a key-value store.")]
        public ObjectValue Function()
        {
            return new ObjectValue();
        }
    }
}
