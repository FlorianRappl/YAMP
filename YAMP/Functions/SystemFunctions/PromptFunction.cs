using System;
using System.Threading;

namespace YAMP
{
    [Description("Prompts the user to input something.")]
    [Kind(PopularKinds.System)]
    class PromptFunction : SystemFunction
    {
        [Description("Waits until the user made some input.")]
        [Example("prompt(\"Please enter something\")", "Shows the user a prompt with the message to Please enter something. Returns the user input as String.")]
        public StringValue Function(StringValue message)
        {
            var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
            var e = new UserInputEventArgs(handle, message.Value);
            Parser.RaiseInputPrompt("prompt", e);
            handle.WaitOne();
            return new StringValue(e.Input);
        }
    }
}
