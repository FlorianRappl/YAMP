using System;

namespace YAMP
{
	[Description("Gives access to a stopwatch with milliseconds precision.")]
	[Kind(PopularKinds.System)]
	class TimerFunction : SystemFunction
	{
	    private int _startMilliSecs;
	    private int _elapsedMilliSecs;
	    private bool _isRunning;

        public TimerFunction()
        {
            Reset();
        }

		[Description("Outputs the current value of the stopwatch in milliseconds.")]
		public ScalarValue Function()
		{
			return new ScalarValue(_elapsedMilliSecs);
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
					Reset();
					break;
				case 0:
					Stop();
					break;
				case 1:
					Start();
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
					Reset();
					break;
				case "stop":
					Stop();
					break;
				case "start":
					Start();
					break;
			}

			return Function();
		}

        private void Reset()
        {
            _elapsedMilliSecs = 0;
            _isRunning = false;
            _startMilliSecs = 0;
        }

        private void Stop()
        {
            if (!_isRunning) return;
            _elapsedMilliSecs += Environment.TickCount - _startMilliSecs;
            _isRunning = false;
        }

        private void Start()
        {
            if (_isRunning) return;
            _startMilliSecs = Environment.TickCount;
            _isRunning = true;
        }
	}
}
