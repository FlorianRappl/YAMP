namespace YAMP.Sensors.Devices
{
    using NAudio.CoreAudioApi;
    using NAudio.Wave;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The microphone devide using NAudio.
    /// </summary>
    public class Microphone : BaseDevice
    {
        readonly WasapiCapture _wasapiCapture;
        readonly List<Byte[]> _measurements;
        Int32 _measured;
        Boolean _measuring;

        /// <summary>
        /// Creates a new microphone.
        /// </summary>
        public Microphone()
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

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        public Int32 SampleRate
        {
            get { return _wasapiCapture != null ? _wasapiCapture.WaveFormat.SampleRate : 0; }
        }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        public Int32 Channels
        {
            get { return _wasapiCapture != null ? _wasapiCapture.WaveFormat.Channels : 0; }
        }

        /// <summary>
        /// Gets the bits per sample.
        /// </summary>
        public Int32 BitsPerSample
        {
            get { return _wasapiCapture != null ? _wasapiCapture.WaveFormat.BitsPerSample : 0; }
        }

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

        /// <summary>
        /// Records the audio for the specified number of buffer fills.
        /// </summary>
        public async Task<Double[]> Record(Int32 bufferFills)
        {
            if (_wasapiCapture != null)
            {
                _wasapiCapture.StartRecording();

                _measured = -1;
                _measuring = true;

                while (_measured < bufferFills)
                {
                    await Task.Delay(50);
                }

                _measuring = false;
                _wasapiCapture.StopRecording();
                var lengths = _measurements.Take(bufferFills).Select(a => a.Length).ToArray();
                var result = new Double[lengths.Sum() / 4];
                var k = 1;

                for (var j = 0; j < bufferFills; j++)
                {
                    for (var i = 0; i < lengths[j]; i += 4, k++)
                    {
                        var value = BitConverter.ToSingle(_measurements[j], i);
                        result[k] = value;
                    }
                }

                return result;
            }

            return new Double[0];
        }

        /// <summary>
        /// Installs the reading handler.
        /// </summary>
        protected override void InstallReadingChangedHandler()
        {
        }

        /// <summary>
        /// Uninstalls the reading handler.
        /// </summary>
        protected override void UninstallReadingChangedHandler()
        {
        }
    }
}
