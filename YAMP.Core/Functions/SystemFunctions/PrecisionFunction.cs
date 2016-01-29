using System;

namespace YAMP
{
	[Description("Gets or sets the precision set for display purposes.")]
	[Kind(PopularKinds.System)]
    sealed class PrecisionFunction : SystemFunction
    {
        [Description("Gets the currently set precision in digits.")]
        [Example("precision()")]
        public ScalarValue Function()
        {
            return new ScalarValue(Context.Precision);
        }

        [Description("Sets the output precision to x digits.")]
        [Example("precision(5)", "Sets the precision to 5 digits.")]
        public void Function(ScalarValue digits)
        {
            var n = digits.GetIntegerOrThrowException("digits", Name);
            Context.Precision = n;
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Information, "Output precision changed to " + Context.Precision + " digits."));
        }
    }
}
