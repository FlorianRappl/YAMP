using System;
using System.Diagnostics;

namespace YAMP
{
    class TimerFunction : ArgumentFunction
    {
        static Stopwatch timer = new Stopwatch();

        public Value Function()
        {
            return new ScalarValue(timer.ElapsedMilliseconds);
        }

        public Value Function(Value argument)
        {
            if (argument is ScalarValue)
            {
                var a = argument as ScalarValue;

                switch (a.IntValue)
                {
                    case 0:
                        timer.Stop();
                        break;
                    case 1:
                        timer.Start();
                        break;
                }
            }
            else if (argument is StringValue)
            {
                var a = argument as StringValue;

                switch (a.Value.ToLower())
                {
                    case "stop":
                        timer.Stop();
                        break;
                    case "start":
                        timer.Start();
                        break;
                }
            }

            return Function();
        }
    }
}
