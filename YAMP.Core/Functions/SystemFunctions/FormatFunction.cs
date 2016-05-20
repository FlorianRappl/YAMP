namespace YAMP
{
    using YAMP.Converter;

    [Description("FormatFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class FormatFunction : SystemFunction
    {
        public FormatFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("FormatFunctionDescriptionForVoid")]
        public StringValue Function()
        {
            return new StringValue(Context.DefaultDisplayStyle.ToString());
        }

        [Description("FormatFunctionDescriptionForString")]
        [Example("format(\"default\")", "FormatFunctionExampleForString1")]
        [Example("format(\"scientific\")", "FormatFunctionExampleForString2")]
        [Example("format(\"engineering\")", "FormatFunctionExampleForString3")]
        public void Function(StringValue type)
        {
            var stec = new StringToEnumConverter(typeof(DisplayStyle));
            var value = stec.Convert(type);
            Context.DefaultDisplayStyle = (DisplayStyle)value;
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Information, "Display format changed to " + value + "."));
        }
    }
}
