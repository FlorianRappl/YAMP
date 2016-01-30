namespace YAMP
{
    using System;
    using System.Threading;

	[Description("Enables the possibility to make a short break - computationally. Sets the computation thread on idle for a number of ms.")]
	[Kind(PopularKinds.System)]
    sealed class SleepFunction : SystemFunction
	{
        public SleepFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("Sets the computation thread on idle for the proposed time in milliseconds (ms).")]
		[Example("sleep(150)", "Sleeps for 150ms and outputs the real waiting time in ms.")]
		public void Function(ScalarValue timeout)
        {
            var n = timeout.GetIntegerOrThrowException("timeout", Name);
			var start = Environment.TickCount;

			using (var blocking = new ManualResetEvent(false))
			{
				blocking.WaitOne(n);
			}

			var time = Environment.TickCount - start;
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Information, "Slept " + time + "ms."));
		}
	}
}
