namespace YAMP.Sensors
{
    using System;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Media.Capture;
    using Windows.Media.MediaProperties;
    using Windows.Storage.Streams;
    using YAMP.Exceptions;

    [Description("Provides access to the default video input, which is usually the installed webcam.")]
    [Kind("Sensor")]
    sealed class VideoFunction : ArgumentFunction
	{
		#region Constants

		const Double rfactor = 256 * 256;
		const Double gfactor = 256;
		const Double bfactor = 1;

		#endregion

		#region Video Device

		static MediaCapture mc;

		#endregion

		#region ctor

		static VideoFunction()
        {
            Init();
        }

        [STAThreadAttribute]
        async static void Init()
        {
            try
            {
                mc = new MediaCapture();
                await mc.InitializeAsync();
            }
            catch { }
        }

		#endregion

		#region Functions

		/// <summary>
        /// retrieves a matrix of the current reading of the video input
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves a matrix representing an image capture of the video input (usually the webcam).")]
        [Example("video()", "Returns three matrices with the rgb values of the image with default coarsening.")]
        [Returns(typeof(MatrixValue), "The matrix of red values.", 0)]
        [Returns(typeof(MatrixValue), "The matrix of green values.", 1)]
        [Returns(typeof(MatrixValue), "The matrix of blue values.", 2)]
        public static ArgumentsValue Function()
        {
            var t = Video();
            t.Wait();
            return t.Result as ArgumentsValue;
        }

        [Description("Retrieves a matrix representing a corarsened version of the image capture of the video input (usually the webcam).")]
        [Example("video(2)", "Returns three matrices with the rgb values of the image, averaged in both directions over 2 adjacent pixels. The default value for the coarsening is 10.0.")]
        [Returns(typeof(MatrixValue), "The matrix of red values.", 0)]
        [Returns(typeof(MatrixValue), "The matrix of green values.", 1)]
        [Returns(typeof(MatrixValue), "The matrix of blue values.", 2)]
        public static ArgumentsValue Function(ScalarValue Coarsening)
        {
            var t = Video(Coarsening.Value);
            t.Wait();
            return t.Result as ArgumentsValue;
        }

        [Description("Retrieves a matrix representing a corarsened version of the image capture of the video input (usually the webcam).")]
        [Example("video(4, 1)", "Returns one matrices with the rgb values of the image, averaged in both directions over 4 adjacent pixels. The default value for the coarsening is 10.0. The RGB values are stored in one number as r * 256 * 256 + g * 256 + b.")]
        public static MatrixValue Function(ScalarValue Coarsening, ScalarValue Fused)
        {
            var t = Video(Coarsening.Value, Fused.Value != 0.0);
            t.Wait();
            return t.Result as MatrixValue;
		}

		[Description("Gets named properties of the video input. Possible properties are \"brightness\" and \"contrast\".")]
		[Example("video(\"brightness\")", "Returns the value for brightness.")]
		public static ScalarValue Function(StringValue p)
		{
			var conv = new YAMP.Converter.StringToEnumConverter(typeof(VideoProperty));
			var property = (VideoProperty)conv.Convert(p);

            if (mc == null || mc.VideoDeviceController == null)
                return new ScalarValue();

			switch (property)
			{
				case VideoProperty.Brightness:
					double brightness;
					mc.VideoDeviceController.Brightness.TryGetValue(out brightness);
					return new ScalarValue(brightness);

				case VideoProperty.Contrast:
					double contrast;
					mc.VideoDeviceController.Contrast.TryGetValue(out contrast);
					return new ScalarValue(contrast);
			}

			return null;
		}

		[Description("Gets and tries to set named properties of the video input. Possible properties are \"brightness\" and \"contrast\".")]
		[Example("video(\"brightness\", 1)", "Tries to set the value for brightness to 1 and returns it.")]
		public static ScalarValue Function(StringValue p, ScalarValue Value)
		{
			var conv = new YAMP.Converter.StringToEnumConverter(typeof(VideoProperty));
			var property = (VideoProperty)conv.Convert(p);

			switch (property)
			{
				case VideoProperty.Brightness:
					mc.VideoDeviceController.Brightness.TrySetValue(Value.Value);
					double brightness;
					mc.VideoDeviceController.Brightness.TryGetValue(out brightness);
					return new ScalarValue(brightness);
				case VideoProperty.Contrast:
					mc.VideoDeviceController.Contrast.TrySetValue(Value.Value);
					double contrast;
					mc.VideoDeviceController.Contrast.TryGetValue(out contrast);
					return new ScalarValue(contrast);
			}

			return null;
		}

		#endregion

		#region Measuring

		async static Task<Value> Video(Double coarsening = 10.0, Boolean fused = false)
        {
            if (mc == null)
                return new ArgumentsValue();

            if (coarsening < 1.0)
                throw new YAMPRuntimeException("Video: coarsening must be larger than or equal to 1.0.");

            var imageProperties = ImageEncodingProperties.CreatePng();
            var IMRAS = new InMemoryRandomAccessStream();

            await mc.CapturePhotoToStreamAsync(imageProperties, IMRAS);

            var BMPD = await BitmapDecoder.CreateAsync(IMRAS);
            var PD = await BMPD.GetPixelDataAsync();
			var rgbValues = PD.DetachPixelData();
            var width = (Int32)BMPD.PixelWidth;
            var height = (Int32)BMPD.PixelHeight;

            var cI = 1.0 / coarsening;
			var finalWidth = (Int32)(width * cI);
            var finalHeight = (Int32)(height * cI);

            var count = new Byte[finalHeight, finalWidth];
            var rvalues = new Double[finalHeight, finalWidth];
            var gvalues = new Double[finalHeight, finalWidth];
            var bvalues = new Double[finalHeight, finalWidth];

            for (var i = 0; i < width; i++)
            {
                var idx = (Int32)(i * cI);

                if (idx >= finalWidth)
                {
                    idx = finalWidth - 1;
                }

                for (int j = 0; j < height; j++)
                {
                    var jdx = (Int32)(j * cI);

                    if (jdx >= finalHeight)
                    {
                        jdx = finalHeight - 1;
                    }

                    rvalues[jdx, idx] += rgbValues[(j * width + i) * 4 + 2];
                    gvalues[jdx, idx] += rgbValues[(j * width + i) * 4 + 1];
                    bvalues[jdx, idx] += rgbValues[(j * width + i) * 4 + 0];
                    count[jdx, idx]++;
                }
            }

            for (var i = 0; i < finalHeight; i++)
            {
                for (var j = 0; j < finalWidth; j++)
                {
                    var cinv = 1.0 / count[i, j];
                    rvalues[i, j] *= cinv;
                    gvalues[i, j] *= cinv;
                    bvalues[i, j] *= cinv;
                }
            }

            if (fused)
            {
                for (var i = 0; i < finalHeight; i++)
                {
                    for (var j = 0; j < finalWidth; j++)
                    {
                        rvalues[i, j] = (Int32)rvalues[i, j];
                        gvalues[i, j] = (Int32)gvalues[i, j];
                        bvalues[i, j] = (Int32)bvalues[i, j];

                        rvalues[i, j] *= rfactor;
                        gvalues[i, j] *= gfactor;
                        bvalues[i, j] *= bfactor;

                        rvalues[i, j] += gvalues[i, j] + bvalues[i, j];
                    }
                }

                return new MatrixValue(rvalues);
            }
            
			return new ArgumentsValue(new MatrixValue(rvalues), new MatrixValue(gvalues), new MatrixValue(bvalues));
        }

		#endregion

        #region Enumeration

        enum VideoProperty
		{
			Brightness,
			Contrast
        }

        #endregion
    }
}
