using System;
using YAMP.Converter;

namespace YAMP
{
    [Description("Sets the output formatting of numeric values to a certain pre-defined style.")]
    [Kind(PopularKinds.System)]
    sealed class FormatFunction : SystemFunction
    {
        [Description("Returns the currently used output format.")]
        public StringValue Function()
        {
            return new StringValue(Context.DefaultDisplayStyle.ToString());
        }

        [Description("Changes the current output format to the given style.")]
        [Example("format(\"default\")", "The default style outputs as many digits as possible, however, only with the given precision.")]
        [Example("format(\"scientific\")", "The scientific style outputs as many digits up to the given precision with an exponential notation beyond.")]
        [Example("format(\"engineering\")", "The engineering style outputs as many digits up to the given precision with an exponential notation used in the orders of 3, i.e. coming from exactly zero with ..., e-6, e-3, e0, e3, e6 and so on.")]
        public void Function(StringValue type)
        {
            var stec = new StringToEnumConverter(typeof(DisplayStyle));
            var value = stec.Convert(type);
            Context.DefaultDisplayStyle = (DisplayStyle)value;
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Information, "Display format changed to " + value + "."));
        }
    }
}
