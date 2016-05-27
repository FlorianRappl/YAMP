using YAMP.Exceptions;

namespace YAMP.Sets
{
    [Description("The NewSet (unordered) function.")]
    [Kind(PopularKinds.Function)]
    sealed class NewSetFunction : ArgumentFunction
	{
        [Description("Creates a new Unordered Set")]
        [Arguments(1, 0)]
        public SetValue Function(StringValue name, ArgumentsValue args)
        {
            var set = new SetValue(name.Value, null, false);

            int iArgs = 1; //First is "name"
            foreach (var arg in args)
            {
                iArgs++;
                if (arg is StringValue || arg is NumericValue)
                    set.Set.Add(arg);
                else
                    throw new YAMPArgumentInvalidException("Element is not ScalarValue neither StringValue", iArgs);
            }
            return set;
        }
	}
}

