using System.Threading;

namespace YAMP
{
	[Description("Enables the possibility to make a short break - computationally. Sets the computation thread on idle for a number of ms.")]
	[Kind(PopularKinds.System)]
	class SleepFunction : SystemFunction
	{
	    private static readonly StringValue Start = new StringValue("start");
        private static readonly StringValue Stop = new StringValue("stop");

		[Description("Sets the computation thread on idle for the proposed time in milliseconds (ms).")]
		[Example("sleep(150)", "Sleeps for 150ms and outputs the real waiting time in ms.")]
		public ScalarValue Function(ScalarValue a)
		{
			var sw = new TimerFunction();
			sw.Function(Start);
		    using (var blocking = new ManualResetEvent(false)) blocking.WaitOne(a.IntValue);
			sw.Function(Stop);
			return new ScalarValue(sw.Function());
		}
	}
}
