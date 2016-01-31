namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    [Description("Provides the possibility to perform iterative sensor measurements. This function should be used in experiments, to get several measurements done with a specified (constant) time interval.")]
    [Kind(PopularKinds.System)]
    sealed class TimeSeriesFunction : ArgumentFunction
    {
        static IDictionary<String, SensorFunction> kids;

        /// <summary>
        /// Static constructor to fill the static dictionary, which contains instances of all available sensor functions.
        /// </summary>
        static TimeSeriesFunction()
        {
            kids = new Dictionary<String, SensorFunction>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var sensorBase = typeof(SensorFunction);

            foreach (var type in types)
            {
                if (type.IsSubclassOf(sensorBase) && !type.IsAbstract)
                {
                    var instance = type.GetConstructor(Type.EmptyTypes).Invoke(null) as SensorFunction;
                    kids.Add(instance.Name, instance);
                }
            }
        }

        public static SensorFunction GetSensorFunction(String name)
        {
            var fn = name.ToLower();

            if (kids.ContainsKey(fn))
            {
                return kids[fn];
            }

            return null;
        }

        public override Value Perform(ParseContext context, Value argument)
        {
            this.context = context;
            return base.Perform(context, argument);
        }

        ParseContext context;

        [Description("Performs a series of function evaluations. Useful with functions with timedependent values like the measurement functions acc, comp, gps, gyro, inc, light, or orient or Lambda expressions containing the measurement functions. The first argument specifies the function to be evaluated, the second argument is the total number of measurements, the third argument the time between measurements in seconds, and the optional last argument give the arguments of the measurement function call.")]
        [ExampleAttribute("timeseries(\"gps\", 10, 3.14, \"Longitude\")", "Returns a list of 10 measurements of the gps function with the argument list \"Longitude\" separated by 3.14 seconds.")]
        [ExampleAttribute("timeseries(\"gps\", 10, 3.14)", "Returns a list of 10 measurements of the gps function without any argument.")]
        [Arguments(3, 0)]
		public MatrixValue Function(StringValue f, ScalarValue n, ScalarValue dt, ArgumentsValue args)
		{
			var function = GetSensorFunction(f.Value);

            if (function == null)
				throw new YAMPRuntimeException("The given function {0} could not be found.", f.Value);

            return Function(new FunctionValue(function), n, dt, args);
		}

		[Description("Performs a series of function evaluations. Useful with functions with timedependent values like the measurement functions acc, comp, gps, gyro, inc, light, or orient or Lambda expressions containing the measurement functions. The first argument specifies the function to be evaluated, the second argument is the total number of measurements, the third argument the time between measurements in seconds, and the optional last argument give the arguments of the measurement function call.")]
		[ExampleAttribute("timeseries(gps, 10, 3.14, \"Longitude\")", "Returns a list of 10 measurements of the gps function with the argument list \"Longitude\" separated by 3.14 seconds.")]
		[ExampleAttribute("timeseries(gps, 10, 3.14)", "Returns a list of 10 measurements of the gps function without any argument.")]
		[ExampleAttribute("timeseries(() => acc() .* (1 + 0.1 * rand(3,1)), 10, 3.14)", "Returns a list of 10 measurements of the acc function with an artificial noise.")]
		[Arguments(3, 0)]
        public MatrixValue Function(FunctionValue f, ScalarValue n, ScalarValue dt, ArgumentsValue args)
        {
            var numberOfMeasurements = (Int32)n.Value;
            var timeBetweenMeasurements = (Int32)Math.Floor(dt.Value * 1000);
            var results = new MatrixValue(numberOfMeasurements, 2);
            var time = 0.0;

            for (var i = 1; i <= numberOfMeasurements; i++)
            {
                Thread.Sleep(timeBetweenMeasurements);

                var result = f.Perform(context, args);
                results[i, 1] = new ScalarValue(time);

                if (result is ScalarValue)
                {
                    results[i, 2] = result as ScalarValue;
                }
                else if (result is MatrixValue)
                {
                    var m = result as MatrixValue;

                    for (var j = 1; j <= m.Length; j++)
                    {
                        results[i, 1 + j] = m[j];
                    }
                }

                time += dt.Value;
            }

            return results;
        }
    }
}
