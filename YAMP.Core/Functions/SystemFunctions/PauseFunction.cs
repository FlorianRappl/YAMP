using System;
using System.Threading;

namespace YAMP
{
    [Description("Waits for the user to press some key.")]
    [Kind(PopularKinds.System)]
    sealed class PauseFunction : SystemFunction
    {
        [Description("Waits until the user presses some key.")]
        [Example("pause()", "Shows the user a prompt with a default message.")]
        public void Function()
        {
            var handle = new ManualResetEvent(false);
            var e = new PauseEventArgs(handle);
            Parser.RaisePause(Context, e);
            handle.WaitOne();
        }
    }
}
