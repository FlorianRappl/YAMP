using System;

namespace YAMP
{
	[Description("Gets information about the type of variables.")]
	[Kind(PopularKinds.System)]
    sealed class TypeFunction : StandardFunction
    {
        [Description("Requests information about the type for the specified variable.")]
        [Example("type(x)", "Gets the type information of the variable x.")]
        public override Value Perform(Value argument)
        {
            return new StringValue(argument.Header);
        }
    }
}
