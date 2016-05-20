namespace YAMP
{
    [Description("CastFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class CastFunction : SystemFunction
    {
        public CastFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("CastFunctionDescriptionForString")]
        [Example("cast(\"5\")", "CastFunctionExampleForString1")]
        [Example("cast(\"0.1\")", "CastFunctionExampleForString2")]
        [Example("cast(\"3e-5\")", "CastFunctionExampleForString3")]
        [Example("cast(prompt(\"Enter something\"))", "CastFunctionExampleForString4")]
        public ScalarValue Function(StringValue original)
        {
            return new ScalarValue(Value.CastStringToDouble(original.Value));
        }
    }
}
