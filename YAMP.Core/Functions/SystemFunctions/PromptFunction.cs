namespace YAMP
{
    using System.Threading;

    [Description("PromptFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class PromptFunction : SystemFunction
    {
        public PromptFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("PromptFunctionDescriptionForVoid")]
        [Example("prompt()", "PromptFunctionExampleForVoid1")]
        public StringValue Function()
        {
            var handle = new ManualResetEvent(false);
            var e = new UserInputEventArgs(handle, "Your input is required");
            Context.RaiseInputPrompt(e);
            handle.WaitOne();
            return new StringValue(e.Input);
        }

        [Description("PromptFunctionDescriptionForString")]
        [Example("prompt(\"Please enter something\")", "PromptFunctionExampleForString1")]
        public StringValue Function(StringValue message)
        {
            var handle = new ManualResetEvent(false);
            var e = new UserInputEventArgs(handle, message.Value);
            Context.RaiseInputPrompt(e);
            handle.WaitOne();
            return new StringValue(e.Input);
        }
    }
}
