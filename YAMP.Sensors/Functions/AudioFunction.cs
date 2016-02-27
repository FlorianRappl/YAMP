namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using YAMP.Converter;
    using YAMP.Sensors.Devices;

    [Description("Provides access to the default audio input, which is usually the installed microphone.")]
    [Kind("Sensor")]
    sealed class AudioFunction : ArgumentFunction
	{
        static readonly Dictionary<AudioProperty, Func<Microphone, ScalarValue>> NamedProperties = new Dictionary<AudioProperty, Func<Microphone, ScalarValue>>
        {
            { AudioProperty.Rate, mic => new ScalarValue(mic.SampleRate) },
            { AudioProperty.Channels, mic => new ScalarValue(mic.Channels) },
            { AudioProperty.Bits, mic => new ScalarValue(mic.BitsPerSample) },
        };

        readonly Microphone _sensor = new Microphone();

		/// <summary>
        /// retrieves a vector of the current reading of the audio input
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves a vector of the current reading of the audio input with n consecutive measurements.")]
        [Example("audio(3)", "Returns a matrix of audio readings consisting of 3 successive sound buffer fills.")]
        public MatrixValue Function(ScalarValue n)
        {
            var data = _sensor.Record(n.IntValue).Result;
            return new MatrixValue(data);
        }

        [Description("Retrieves a vector of the current reading of the audio input.")]
        [Example("audio()", "Returns a matrix of audio readings consisting of 1 buffer fill.")]
        public MatrixValue Function()
        {
            return Function(ScalarValue.One);
		}

		[Description("Retrieves named properties of the audio input. Possible properties are \"SampleRate\" and \"Channels\".")]
		[Example("audio(\"rate\")", "Returns a scalar value which is the sample rate in Hz.")]
		[Example("audio(\"channels\")", "Returns a scalar value which is the number of channels.")]
		[Example("audio(\"bits\")", "Returns a scalar value which represents the bits per sample (8, 16, 24, 32, ...).")]
		public ScalarValue Function(StringValue p)
		{
			var conv = new StringToEnumConverter(typeof(AudioProperty));
			var property = (AudioProperty)conv.Convert(p);
            var callback = default(Func<Microphone, ScalarValue>);

            if (NamedProperties.TryGetValue(property, out callback))
            {
                return callback(_sensor);
            }

            return new ScalarValue();
		}

        enum AudioProperty
		{
			Rate,
			Channels,
			Bits
        }
    }
}
