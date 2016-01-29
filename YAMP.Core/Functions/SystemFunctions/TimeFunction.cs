using System;

namespace YAMP
{
    [Description("The time() function allows you to get times with or without offset.")]
    [Kind(PopularKinds.System)]
    sealed class TimeFunction : SystemFunction
    {
        [Description("Gets the current time, taken at the moment of the query request.")]
        [Example("time()", "Prints the current time.")]
        public StringValue Function()
        {
            var dt = DateTime.Now;
            return new StringValue(dt.ToString("t"));
        }

        [Description("Gets the current time with the specified offset in minutes.")]
        [Example("time(100)", "Prints the time in 100 minutes.")]
        public StringValue Function(ScalarValue offset)
        {
            var dt = DateTime.Now.AddMinutes(offset.Re);
            return new StringValue(dt.ToString("t"));
        }
    }
}
