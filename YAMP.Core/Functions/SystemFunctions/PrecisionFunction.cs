namespace YAMP
{
	[Description("PrecisionFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class PrecisionFunction : SystemFunction
    {
        public PrecisionFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("PrecisionFunctionDescriptionForVoid")]
        [Example("precision()")]
        public ScalarValue Function()
        {
            return new ScalarValue(Context.Precision);
        }

        [Description("PrecisionFunctionDescriptionForScalar")]
        [Example("precision(5)", "PrecisionFunctionExampleForScalar1")]
        public void Function(ScalarValue digits)
        {
            var n = digits.GetIntegerOrThrowException("digits", Name);
            Context.Precision = n;
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Information, "Output precision changed to " + Context.Precision + " digits."));
        }
    }
}
