namespace YAMP.Sensors.Devices
{
    using System;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Media.Capture;
    using Windows.Media.MediaProperties;
    using Windows.Storage.Streams;
    using YAMP.Exceptions;

    /// <summary>
    /// The camera device using the MediaCapture.
    /// </summary>
    public class Camera : BaseDevice, IDisposable
    {
        const Double rfactor = 256 * 256;
        const Double gfactor = 256;
        const Double bfactor = 1;

        readonly MediaCapture _mediaCapture = InitDevice();

        static MediaCapture InitDevice()
        {
            try
            {
                var mediaCapture = new MediaCapture();
                var status = mediaCapture.InitializeAsync().Status;
                return mediaCapture;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        public Double Brightness
        {
            get 
            {
                var result = default(Double);
                _mediaCapture.VideoDeviceController.Brightness.TryGetValue(out result);
                return result; 
            }
            set 
            {
                _mediaCapture.VideoDeviceController.Brightness.TrySetValue(value); 
            }
        }

        /// <summary>
        /// Gets or sets the contrast.
        /// </summary>
        public Double Contrast
        {
            get 
            {
                var result = default(Double);
                _mediaCapture.VideoDeviceController.Contrast.TryGetValue(out result);
                return result; 
            }
            set 
            {
                _mediaCapture.VideoDeviceController.Brightness.TrySetValue(value); 
            }
        }

        /// <summary>
        /// Captures the next frame.
        /// </summary>
        public async Task<FrameData> Capture(Double coarsening = 10.0, Boolean fused = false)
        {
            if (_mediaCapture != null)
            {
                if (coarsening < 1.0)
                {
                    throw new YAMPRuntimeException("Video: coarsening must be larger than or equal to 1.0.");
                }

                var imageProperties = ImageEncodingProperties.CreatePng();
                var stream = new InMemoryRandomAccessStream();

                await _mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);

                var decoder = await BitmapDecoder.CreateAsync(stream);
                var pixelData = await decoder.GetPixelDataAsync();
                var rgbValues = pixelData.DetachPixelData();
                var width = (Int32)decoder.PixelWidth;
                var height = (Int32)decoder.PixelHeight;

                var inverseCoarsening = 1.0 / coarsening;
                var finalWidth = (Int32)(width * inverseCoarsening);
                var finalHeight = (Int32)(height * inverseCoarsening);

                var count = new Byte[finalHeight, finalWidth];
                var rvalues = new Double[finalHeight, finalWidth];
                var gvalues = new Double[finalHeight, finalWidth];
                var bvalues = new Double[finalHeight, finalWidth];

                for (var i = 0; i < width; i++)
                {
                    var idx = (Int32)(i * inverseCoarsening);

                    if (idx >= finalWidth)
                    {
                        idx = finalWidth - 1;
                    }

                    for (int j = 0; j < height; j++)
                    {
                        var jdx = (Int32)(j * inverseCoarsening);

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

                    return new FrameData
                    {
                        Blue = rvalues,
                        Green = rvalues,
                        Red = rvalues,
                        IsFused = true
                    };
                }

                return new FrameData
                {
                    Blue = bvalues,
                    Green = gvalues,
                    Red = rvalues,
                    IsFused = false
                };
            }

            return new FrameData();
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            _mediaCapture.Dispose();
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
