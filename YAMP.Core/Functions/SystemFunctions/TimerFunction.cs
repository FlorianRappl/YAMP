namespace YAMP
{
    using System;

	[Description("TimerFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class TimerFunction : SystemFunction
	{
	    static Int32 _startMilliSecs = 0;
	    static Int32 _elapsedMilliSecs = 0;
	    static Boolean _isRunning = false;

        public TimerFunction(ParseContext context)
            : base(context)
        {
        }
        
		[Description("TimerFunctionDescriptionForVoid")]
		public ScalarValue Function()
		{
            if (_isRunning)
            {
                return new ScalarValue(_elapsedMilliSecs + Environment.TickCount - _startMilliSecs);
            }

			return new ScalarValue(_elapsedMilliSecs);
		}

		[Description("TimerFunctionDescriptionForScalar")]
		[Example("timer(0)", "TimerFunctionExampleForScalar1")]
		[Example("timer(1)", "TimerFunctionExampleForScalar2")]
		[Example("timer(-1)", "TimerFunctionExampleForString3")]
		public ScalarValue Function(ScalarValue action)
        {
            var n = action.GetIntegerOrThrowException("action", Name);

			switch (n)
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

		[Description("TimerFunctionDescriptionForString")]
		[Example("timer(\"stop\")", "TimerFunctionExampleForString1")]
		[Example("timer(\"start\")", "TimerFunctionExampleForString2")]
		[Example("timer(\"reset\")", "TimerFunctionExampleForString3")]
		public ScalarValue Function(StringValue action)
		{
			switch (action.Value.ToLower())
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
			if (!_isRunning)
			{
				_startMilliSecs = Environment.TickCount;
				_isRunning = true;
			}
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
			_startMilliSecs = Environment.TickCount;
		}
	}
}
