namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using YAMP.Converter;
    using YAMP.Sensors.Devices;

    [Description("Provides access to the default video input, which is usually the installed webcam.")]
    [Kind("Sensor")]
    sealed class VideoFunction : ArgumentFunction
	{
        static readonly Dictionary<VideoProperty, VideoDeviceReader> NamedProperties = new Dictionary<VideoProperty, VideoDeviceReader>()
        {
            { VideoProperty.Brightness, new VideoDeviceReader(camera => new ScalarValue(camera.Brightness), (camera, value) => camera.Brightness = value.Value)},
            { VideoProperty.Contrast, new VideoDeviceReader(camera => new ScalarValue(camera.Contrast), (camera, value) => camera.Contrast = value.Value)},
        };

        readonly Camera _sensor = new Camera();

		/// <summary>
        /// retrieves a matrix of the current reading of the video input
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves a matrix representing an image capture of the video input (usually the webcam).")]
        [Example("video()", "Returns three matrices with the rgb values of the image with default coarsening.")]
        [Returns(typeof(MatrixValue), "The matrix of red values.", 0)]
        [Returns(typeof(MatrixValue), "The matrix of green values.", 1)]
        [Returns(typeof(MatrixValue), "The matrix of blue values.", 2)]
        public ArgumentsValue Function()
        {
            var result = _sensor.Video().Result;
            var red = new MatrixValue(result.Red);
            var green  = new MatrixValue(result.Green);
            var blue = new MatrixValue(result.Blue);
            return new ArgumentsValue(red, green, blue);
        }

        [Description("Retrieves a matrix representing a corarsened version of the image capture of the video input (usually the webcam).")]
        [Example("video(2)", "Returns three matrices with the rgb values of the image, averaged in both directions over 2 adjacent pixels. The default value for the coarsening is 10.0.")]
        [Returns(typeof(MatrixValue), "The matrix of red values.", 0)]
        [Returns(typeof(MatrixValue), "The matrix of green values.", 1)]
        [Returns(typeof(MatrixValue), "The matrix of blue values.", 2)]
        public ArgumentsValue Function(ScalarValue Coarsening)
        {
            var result = _sensor.Video(Coarsening.Value).Result;
            var red = new MatrixValue(result.Red);
            var green = new MatrixValue(result.Green);
            var blue = new MatrixValue(result.Blue);
            return new ArgumentsValue(red, green, blue);
        }

        [Description("Retrieves a matrix representing a corarsened version of the image capture of the video input (usually the webcam).")]
        [Example("video(4, 1)", "Returns one matrices with the rgb values of the image, averaged in both directions over 4 adjacent pixels. The default value for the coarsening is 10.0. The RGB values are stored in one number as r * 256 * 256 + g * 256 + b.")]
        public MatrixValue Function(ScalarValue Coarsening, ScalarValue Fused)
        {
            var result = _sensor.Video(Coarsening.Value, true).Result;
            return new MatrixValue(result.Red);
		}

		[Description("Gets named properties of the video input. Possible properties are \"brightness\" and \"contrast\".")]
		[Example("video(\"brightness\")", "Returns the value for brightness.")]
		public ScalarValue Function(StringValue p)
		{
			var conv = new StringToEnumConverter(typeof(VideoProperty));
            var property = (VideoProperty)conv.Convert(p);
            var deviceReader = default(VideoDeviceReader);

            if (NamedProperties.TryGetValue(property, out deviceReader))
            {
                return deviceReader.GetValue(_sensor);
            }

            return new ScalarValue();
		}

		[Description("Gets and tries to set named properties of the video input. Possible properties are \"brightness\" and \"contrast\".")]
		[Example("video(\"brightness\", 1)", "Tries to set the value for brightness to 1 and returns it.")]
		public ScalarValue Function(StringValue p, ScalarValue value)
        {
            var conv = new StringToEnumConverter(typeof(VideoProperty));
            var property = (VideoProperty)conv.Convert(p);
            var deviceReader = default(VideoDeviceReader);

            if (NamedProperties.TryGetValue(property, out deviceReader))
            {
                deviceReader.SetValue(_sensor, value);
            }

            return value;
		}

        enum VideoProperty
		{
			Brightness,
			Contrast
        }

        sealed class VideoDeviceReader
        {
            readonly Func<Camera, ScalarValue> _get;
            readonly Action<Camera, ScalarValue> _set;

            public VideoDeviceReader(Func<Camera, ScalarValue> get, Action<Camera, ScalarValue> set)
            {
                _get = get;
                _set = set;
            }

            public ScalarValue GetValue(Camera camera)
            {
                return _get(camera);
            }

            public void SetValue(Camera camera, ScalarValue value)
            {
                _set(camera, value);
            }
        }
    }
}
