namespace YAMP.Sensors
{
    using NAudio.CoreAudioApi;
    using NAudio.Wave;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using YAMP.Converter;

    [Description("Provides access to the default audio input, which is usually the installed microphone.")]
    [Kind("Sensor")]
    sealed class AudioFunction : ArgumentFunction
	{
		#region Fields

        static readonly Dictionary<AudioProperty, Func<WaveFormat, ScalarValue>> NamedProperties = new Dictionary<AudioProperty, Func<WaveFormat, ScalarValue>>
        {
            { AudioProperty.Rate, format => new ScalarValue(format.SampleRate) },
            { AudioProperty.Channels, format => new ScalarValue(format.Channels) },
            { AudioProperty.Bits, format => new ScalarValue(format.BitsPerSample) },
        };

		readonly WasapiCapture _wasapiCapture;
		readonly List<Byte[]> _measurements;
		Int32 _measured;
		Boolean _measuring;

		#endregion

		#region ctor

		public AudioFunction()
        {
            _measurements = new List<Byte[]>();

            try
            {
                _wasapiCapture = new WasapiCapture();
                _wasapiCapture.DataAvailable += OnDataAvailable;
            }
            catch
            { }

            _measuring = false;
        }

		#endregion

		#region Measuring

		void OnDataAvailable(Object sender, WaveInEventArgs e)
        {
            if (_measuring)
            {
                if (_measured >= 0)
                {
                    _measurements.Add(e.Buffer.Take(e.BytesRecorded).ToArray());
                }

                _measured++;
            }
		}

		async Task<MatrixValue> Audio(Int32 n)
		{
			if (_wasapiCapture != null)
            {
                _wasapiCapture.StartRecording();

                _measured = -1;
                _measuring = true;

                while (_measured < n)
                {
                    await Task.Delay(50);
                }

                _measuring = false;
                _wasapiCapture.StopRecording();
                var lengths = _measurements.Take(n).Select(a => a.Length).ToArray();
                var result = new MatrixValue(1, lengths.Sum() / 4);
                var k = 1;

                for (var j = 0; j < n; j++)
                {
                    for (var i = 0; i < lengths[j]; i += 4, k++)
                    {
                        var value = BitConverter.ToSingle(_measurements[j], i);
                        result[k] = new ScalarValue(value);
                    }
                }

                return result;
            }

			return new MatrixValue();
		}

		#endregion

		#region Methods

		/// <summary>
        /// retrieves a vector of the current reading of the audio input
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves a vector of the current reading of the audio input with n consecutive measurements.")]
        [Example("audio(3)", "Returns a matrix of audio readings consisting of 3 successive sound buffer fills.")]
        public MatrixValue Function(ScalarValue n)
        {
            var t = Audio(n.IntValue);
            t.Wait();
            return t.Result;
        }

        [Description("Retrieves a vector of the current reading of the audio input.")]
        [Example("audio()", "Returns a matrix of audio readings consisting of 1 buffer fill.")]
        public MatrixValue Function()
        {
            var t = Audio(1);
            t.Wait();
            return t.Result;
		}

		[Description("Retrieves named properties of the audio input. Possible properties are \"SampleRate\" and \"Channels\".")]
		[Example("audio(\"rate\")", "Returns a scalar value which is the sample rate in Hz.")]
		[Example("audio(\"channels\")", "Returns a scalar value which is the number of channels.")]
		[Example("audio(\"bits\")", "Returns a scalar value which represents the bits per sample (8, 16, 24, 32, ...).")]
		public ScalarValue Function(StringValue p)
		{
			var conv = new StringToEnumConverter(typeof(AudioProperty));
			var property = (AudioProperty)conv.Convert(p);
            var callback = default(Func<WaveFormat, ScalarValue>);

            if (_wasapiCapture != null && _wasapiCapture.WaveFormat != null && NamedProperties.TryGetValue(property, out callback))
            {
                return callback(_wasapiCapture.WaveFormat);
            }

            return new ScalarValue();
		}

		#endregion

        #region Enumeration

        enum AudioProperty
		{
			Rate,
			Channels,
			Bits
        }

        #endregion
    }
}
