namespace YAMP
{
    using System.Threading;

    [Description("PauseFunctionDescription")]
    [Kind(PopularKinds.System)]
    sealed class PauseFunction : SystemFunction
    {
        public PauseFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("PauseFunctionDescriptionForVoid")]
        [Example("pause()", "PauseFunctionExampleForVoid1")]
        public void Function()
        {
            var handle = new ManualResetEvent(false);
            Context.RaisePause(new PauseEventArgs(handle));
            handle.WaitOne();
        }
    }
}
