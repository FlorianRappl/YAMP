using System;
using System.Threading;

namespace YAMP
{
    [Description("Prompts the user to input something.")]
    [Kind(PopularKinds.System)]
    sealed class PromptFunction : SystemFunction
    {
        [Description("Waits until the user made some input with a default message.")]
        [Example("prompt()", "Shows the user a prompt with a default message. Returns the user input as String.")]
        public StringValue Function()
        {
            var handle = new ManualResetEvent(false);
            var e = new UserInputEventArgs(handle, "Your input is required");
            Parser.RaiseInputPrompt(Context, e);
            handle.WaitOne();
            return new StringValue(e.Input);
        }

        [Description("Waits until the user made some input.")]
        [Example("prompt(\"Please enter something\")", "Shows the user a prompt with the message to Please enter something. Returns the user input as String.")]
        public StringValue Function(StringValue message)
        {
            var handle = new ManualResetEvent(false);
            var e = new UserInputEventArgs(handle, message.Value);
            Parser.RaiseInputPrompt(Context, e);
            handle.WaitOne();
            return new StringValue(e.Input);
        }
    }
}
