namespace YAMP.Sensors
{
    using NAudio.CoreAudioApi;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Description("Provides access to the default audio input, which is usually the installed microphone.")]
    [Kind("Sensor")]
    sealed class AudioFunction : ArgumentFunction
	{
		#region Fields

		WasapiCapture wasapiCapture;
		List<Byte[]> measurements;
		Int32 measured;
		Boolean measuring;

		#endregion

		#region ctor

		public AudioFunction()
        {
            try
            {
                wasapiCapture = new WasapiCapture();
                wasapiCapture.DataAvailable += OnDataAvailable;
            }
            catch
            { }

            measuring = false;
        }

		#endregion

		#region Measuring

		void OnDataAvailable(Object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (measuring)
            {
                if (measured >= 0)
                {
                    measurements.Add(e.Buffer.Take(e.BytesRecorded).ToArray());
                }

                measured++;
            }
		}

		async Task<MatrixValue> Audio(Int32 n)
		{
			if (wasapiCapture != null)
            {
                wasapiCapture.StartRecording();

                measurements = new List<Byte[]>();
                measured = -1;
                measuring = true;

                while (measured < n)
                {
                    await Task.Delay(50);
                }

                measuring = false;
                wasapiCapture.StopRecording();
                var lengths = measurements.Take(n).Select(a => a.Length).ToArray();
                var result = new MatrixValue(1, lengths.Sum() / 4);
                var k = 1;

                for (var j = 0; j < n; j++)
                {
                    for (var i = 0; i < lengths[j]; i += 4, k++)
                    {
                        var value = BitConverter.ToSingle(measurements[j], i);
                        result[k] = new ScalarValue(value);
                    }
                }

                return result;
            }

			return new MatrixValue();
		}

		#endregion

		#region Functions

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
			var conv = new YAMP.Converter.StringToEnumConverter(typeof(AudioProperty));
			var property = (AudioProperty)conv.Convert(p);

            if (wasapiCapture != null && wasapiCapture.WaveFormat != null)
            {
                switch (property)
                {
                    case AudioProperty.Rate:
                        return new ScalarValue(wasapiCapture.WaveFormat.SampleRate);
                    case AudioProperty.Channels:
                        return new ScalarValue(wasapiCapture.WaveFormat.Channels);
                    case AudioProperty.Bits:
                        return new ScalarValue(wasapiCapture.WaveFormat.BitsPerSample);
                }

                return null;
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
