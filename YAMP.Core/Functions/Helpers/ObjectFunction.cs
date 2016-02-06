namespace YAMP.Functions.Helpers
{
    [Description("Creates an empty object.")]
    [Kind(PopularKinds.Function)]
    sealed class ObjectFunction : BaseFunction
    {
        public override Value Perform(Value argument)
        {
            return new ObjectValue();
        }
    }
}
