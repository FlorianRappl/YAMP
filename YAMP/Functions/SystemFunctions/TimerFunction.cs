using System;
using System.Diagnostics;

namespace YAMP
{
	[Description("Gives access to a stopwatch with milliseconds precision.")]
	[Kind(PopularKinds.System)]
	class TimerFunction : SystemFunction
	{
		static Stopwatch timer = new Stopwatch();

		[Description("Outputs the current value of the stopwatch in milliseconds.")]
		public ScalarValue Function()
		{
			return new ScalarValue(timer.ElapsedMilliseconds);
		}

		[Description("Controls the stopwatch and outputs the current value of the stopwatch in milliseconds.")]
		[Example("timer(0)", "Stops the stopwatch")]
		[Example("timer(1)", "Starts the stopwatch")]
		[Example("timer(-1)", "Resets the stopwatch")]
		public ScalarValue Function(ScalarValue a)
		{
			switch (a.IntValue)
			{
				case -1:
					timer.Reset();
					goto case 0;
				case 0:
					timer.Stop();
					break;
				case 1:
					timer.Start();
					break;
			}

			return Function();
		}

		[Description("Controls the stopwatch and outputs the current value of the stopwatch in milliseconds.")]
		[Example("timer(\"stop\")", "Stops the stopwatch")]
		[Example("timer(\"start\")", "Starts the stopwatch")]
		[Example("timer(\"reset\")", "Resets the stopwatch")]
		public ScalarValue Function(StringValue a)
		{
			switch (a.Value.ToLower())
			{
				case "reset":
					timer.Reset();
					goto case "stop";
				case "stop":
					timer.Stop();
					break;
				case "start":
					timer.Start();
					break;
			}

			return Function();
		}
	}
}
