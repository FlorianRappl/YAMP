namespace YAMP
{
    using System;
    using System.Threading;

	[Description("SleepFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class SleepFunction : SystemFunction
	{
        public SleepFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("SleepFunctionDescriptionForScalar")]
		[Example("sleep(150)", "SleepFunctionExampleForScalar1")]
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
