using System;

namespace YAMP
{
	[Description("Gives access to a stopwatch with milliseconds precision.")]
	[Kind(PopularKinds.System)]
	class TimerFunction : SystemFunction
	{
	    int _startMilliSecs;
	    int _elapsedMilliSecs;
	    bool _isRunning;

        public TimerFunction()
        {
            Reset();
        }

		[Description("Outputs the current value of the stopwatch in milliseconds.")]
		public ScalarValue Function()
		{
			if (_isRunning)
				return new ScalarValue(Environment.TickCount - _startMilliSecs);

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

		void Start()
		{
			if (_isRunning)
				return;

			_startMilliSecs = Environment.TickCount;
			_isRunning = true;
		}

        void Stop()
        {
			if (_isRunning)
			{
				_elapsedMilliSecs += Environment.TickCount - _startMilliSecs;
				_isRunning = false;
			}
        }

		void Reset()
		{
			_elapsedMilliSecs = 0;
			_isRunning = false;
			_startMilliSecs = 0;
		}
	}
}
