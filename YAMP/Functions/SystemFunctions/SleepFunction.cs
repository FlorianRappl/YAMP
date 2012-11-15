using System.Threading;

namespace YAMP
{
	[Description("Enables the possibility to make a short break - computationally. Sets the computation thread on idle for a number of ms.")]
	[Kind(PopularKinds.System)]
	class SleepFunction : SystemFunction
	{
	    private readonly StringValue _start = new StringValue("start");
        private readonly StringValue _stop = new StringValue("stop");

		[Description("Sets the computation thread on idle for the proposed time in milliseconds (ms).")]
		[Example("sleep(150)", "Sleeps for 150ms and outputs the real waiting time in ms.")]
		public ScalarValue Function(ScalarValue a)
		{
			var sw = new TimerFunction();
			sw.Function(_start);
		    using (var blocking = new ManualResetEvent(false)) blocking.WaitOne(a.IntValue);
			sw.Function(_stop);
			return new ScalarValue(sw.Function());
		}
	}
}
