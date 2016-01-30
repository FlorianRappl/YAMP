namespace YAMP
{
    using System.Threading;

    [Description("Waits for the user to press some key.")]
    [Kind(PopularKinds.System)]
    sealed class PauseFunction : SystemFunction
    {
        public PauseFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("Waits until the user presses some key.")]
        [Example("pause()", "Shows the user a prompt with a default message.")]
        public void Function()
        {
            var handle = new ManualResetEvent(false);
            Context.RaisePause(new PauseEventArgs(handle));
            handle.WaitOne();
        }
    }
}
