namespace YAMP
{
    using System;

    [Description("TimeFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class TimeFunction : SystemFunction
    {
        public TimeFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("TimeFunctionDescriptionForVoid")]
        [Example("time()", "TimeFunctionExampleForVoid1")]
        public StringValue Function()
        {
            var dt = DateTime.Now;
            return new StringValue(dt.ToString("t"));
        }

        [Description("TimeFunctionDescriptionForScalar")]
        [Example("time(100)", "TimeFunctionExampleForScalar1")]
        public StringValue Function(ScalarValue offset)
        {
            var dt = DateTime.Now.AddMinutes(offset.Re);
            return new StringValue(dt.ToString("t"));
        }
    }
}
