namespace YAMP.Sensors.Devices
{
    using NAudio.CoreAudioApi;
    using NAudio.Wave;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Microphone : BaseDevice
    {
        readonly WasapiCapture _wasapiCapture;
        readonly List<Byte[]> _measurements;
        Int32 _measured;
        Boolean _measuring;

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

        public Int32 SampleRate
        {
            get { return _wasapiCapture != null ? _wasapiCapture.WaveFormat.SampleRate : 0; }
        }

        public Int32 Channels
        {
            get { return _wasapiCapture != null ? _wasapiCapture.WaveFormat.Channels : 0; }
        }

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

        protected override void InstallReadingChangedHandler()
        {
        }

        protected override void UninstallReadingChangedHandler()
        {
        }
    }
}
