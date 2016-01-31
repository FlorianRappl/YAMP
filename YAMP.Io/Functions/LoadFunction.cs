namespace YAMP.Io
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using YAMP.Exceptions;

	[Description("Loads compatible files into YAMP.")]
	[Kind(PopularKinds.System)]
    sealed class LoadFunction : SystemFunction
    {
        const Double rfactor = 256 * 256;
        const Double gfactor = 256;
        const Double bfactor = 1;

        public LoadFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("Loads all variables found in the file, if the file contains YAMP variables. Else it treats the file as an ASCII data table or an image file and stores the content as a matrix with the name \"data\" or \"image\".")]
        [Example("load(\"myfile.mat\")", "Opens the file myfile.mat and reads out all variables.", true)]
        public void Function(StringValue filename)
		{
            if (!File.Exists(filename.Value))
            {
                throw new YAMPFileNotFoundException(filename.Value);
            }

			var error = false;
			var v = Load(filename.Value, out error);
			var count = 0;

            if(!error)
			{
                foreach (var key in v.Keys)
                {
                    Context.AssignVariable(key, v[key]);
                }

				count = v.Count;
			}

            if (error)
            {
                var table = ImageLoad(filename.Value, out error);

                if (!error)
                {
                    var suffix = -1;
                    var name = "image";

                    do
                    {
                        suffix++;
                    }
                    while (Context.Variables.ContainsKey(name + suffix));

                    Context.AssignVariable(name + suffix, table);
                    count = 1;
                }
            }

			if (error)
			{
				var table = ASCIILoad(filename.Value, out error);

                if (!error)
                {
                    var suffix = -1;
                    var name = "data";

                    do
                    {
                        suffix++;
                    }
                    while (Context.Variables.ContainsKey(name + suffix));

                    Context.AssignVariable(name + suffix, table);
                    count = 1;
                }
			}

            if (error)
            {
                throw new YAMPFileFormatNotSupportedException(filename.Value);
            }

            Notify(count);
		}

        [Description("Tries to load the file as the specified file type.")]
        [Example("load(\"myfile.mat\", \"binary\")", "Opens the file myfile.mat and reads out all variables.", true)]
        [Example("load(\"myfile.bmp\", \"image\")", "Opens the image myfile.bmp and transforms the data to a matrix.", true)]
        [Example("load(\"myfile.txt\", \"text\")", "Opens the textfile myfile.txt converts the data to a matrix.", true)]
        public void Function(StringValue filename, StringValue filetype)
        {
            var type = (FileType)(new YAMP.Converter.StringToEnumConverter(typeof(FileType)).Convert(filetype));
            var error = false;
            var count = 0;

            switch (type)
            {
                case FileType.Text:
                    var table = ASCIILoad(filename.Value, out error);

                    if (!error)
                    {
                        var suffix = -1;
                        var name = "data";

                        do
                        {
                            suffix++;
                        }
                        while (Context.Variables.ContainsKey(name + suffix));

                        Context.AssignVariable(name + suffix, table);
                        count = 1;
                    }

                    break;

                case FileType.Image:
                    var data = ImageLoad(filename.Value, out error);

                    if (!error)
                    {
                        var suffix = -1;
                        var name = "image";

                        do
                        {
                            suffix++;
                        }
                        while (Context.Variables.ContainsKey(name + suffix));

                        Context.AssignVariable(name + suffix, data);
                        count = 1;
                    }

                    break;

                case FileType.Binary:
			        var v = Load(filename.Value, out error);

                    if(!error)
			        {
				        foreach (var key in v.Keys)
					        Context.AssignVariable(key, v[key]);

				        count = v.Count;
			        }

                    break;
            }

            if (error)
                throw new YAMPFileFormatNotSupportedException(filename.Value);

            Notify(count);
        }

        [Description("Loads specified variables found in the file, if the file contains YAMP variables. Else it treats the file as an ASCII data table or an image file and stores the content as a matrix with the name of the first variable.")]
        [Example("load(\"myfile.mat\", \"x\", \"y\", \"z\")", "Opens the file myfile.mat and reads out variables that have been named x, y and z.", true)]
		[Arguments(1, 1)]
        public void Function(StringValue filename, ArgumentsValue args)
        {
            if (!File.Exists(filename.Value))
                throw new YAMPFileNotFoundException(filename.Value);

			var error = false;
            var v = Load(filename.Value, out error);
			var count = 0;

            if(!error)
			{
				foreach (var arg in args.Values)
				{
					if (arg is StringValue)
					{
						var name = (arg as StringValue).Value;

						if (v.ContainsKey(name))
						{
							Context.AssignVariable(name, v[name] as Value);
							count++;
						}
					}
				}
            }

            if (error)
            {
                var table = ImageLoad(filename.Value, out error);

                if (!error)
                {
                    var name = "image";

                    if (args.Length > 0 && args.Values[0] is StringValue)
                    {
                        name = (args.Values[0] as StringValue).Value;
                    }
                    else
                    {
                        var suffix = -1;

                        do
                        {
                            suffix++;
                        }
                        while (Context.Variables.ContainsKey(name + suffix));

                        name = name + suffix;
                    }

                    Context.AssignVariable(name, table);
                    count = 1;
                }
            }

			if (error)
			{
				var table = ASCIILoad(filename.Value, out error);

                if (!error)
                {
                    var name = "data";

                    if (args.Length > 0 && args.Values[0] is StringValue)
                    {
                        name = (args.Values[0] as StringValue).Value;
                    }
                    else
                    {
                        var suffix = -1;

                        do
                        {
                            suffix++;
                        }
                        while (Context.Variables.ContainsKey(name + suffix));

                        name = name + suffix;
                    }

                    Context.AssignVariable(name, table);
                    count = 1;
                }
			}

            if (error)
                throw new YAMPFileFormatNotSupportedException(filename.Value);

            Notify(count);
        }

        #region Helpers

        void Notify(Int32 count)
        {
            RaiseNotification(NotificationType.Success, count + " objects loaded.");
        }

        static IDictionary<String, Value> Load(String filename, out Boolean error)
		{
			var ht = new Dictionary<String, Value>();
			var lenbuffer = new Byte[4];
			var ctnbuffer = new Byte[0];

			error = false;

			using (var fs = File.Open(filename, FileMode.Open))
			{
				while (fs.Position < fs.Length)
				{
					fs.Read(lenbuffer, 0, lenbuffer.Length);
					var length = BitConverter.ToInt32(lenbuffer, 0);

                    if (fs.Position + length > fs.Length || length < 0)
					{
						error = true;
						break;
					}

					ctnbuffer = new Byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var name = Encoding.Unicode.GetString(ctnbuffer);

					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32(lenbuffer, 0);

                    if (fs.Position + length > fs.Length || length < 0)
					{
						error = true;
						break;
					}

					ctnbuffer = new Byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var header = Encoding.ASCII.GetString(ctnbuffer);

					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32(lenbuffer, 0);

                    if (fs.Position + length > fs.Length || length < 0)
					{
						error = true;
						break;
					}

					ctnbuffer = new Byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var value = Value.Deserialize(header, ctnbuffer);
					ht.Add(name, value);
				}
			}

            if (error)
            {
                ht.Add("FileParsingError", Value.Empty);
            }

			return ht;
		}

        MatrixValue ASCIILoad(String filename, out Boolean error)
        {
            error = false;

            using (var fs = File.Open(filename, FileMode.Open))
            {
                var file_bytes = new Byte[fs.Length];
                fs.Read(file_bytes, 0, file_bytes.Length);
                var file_string = Encoding.ASCII.GetString(file_bytes);
                var lines = file_string.Split('\n').Select(line => line.Split(new [] { ' ', '\t', ',', '\r' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                var numberOfLines = lines.Count;
                var parseResult = 0.0;
                var numberOfHeaderLines = 0;
                var numberOfFooterLines = 0;

                while (numberOfHeaderLines < numberOfLines && !Double.TryParse(lines[numberOfHeaderLines].FirstOrDefault(), out parseResult))
                {
                    numberOfHeaderLines++;
                }

                while (numberOfFooterLines < numberOfLines - numberOfHeaderLines && !Double.TryParse(lines[numberOfLines - 1 - numberOfFooterLines].FirstOrDefault(), out parseResult))
                {
                    numberOfFooterLines++;
                }

                if (numberOfLines <= numberOfHeaderLines + numberOfFooterLines)
                {
                    error = true;
                    return new MatrixValue();
                }

                var tokensPerLine = lines.Select(line => line.Length).SkipWhile((item, index) => index < numberOfHeaderLines).Reverse().SkipWhile((item, index) => index < numberOfFooterLines).Reverse().ToList();
                var numberOfColumns = tokensPerLine.Max();
                var numberOfRows = numberOfLines - numberOfHeaderLines - numberOfFooterLines;
                var result = new MatrixValue(numberOfRows, numberOfColumns);

                for (var i = 0; i < numberOfRows; i++)
                {
                    for (var j = 0; j < tokensPerLine[i]; j++)
                    {
                        if (Double.TryParse(lines[numberOfHeaderLines + i][j], NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out parseResult))
                        {
                            result[i + 1, j + 1] = new ScalarValue(parseResult);
                        }
                        else
                        {
                            result[i + 1, j + 1] = new ScalarValue(Double.NaN);
                        }
                    }
                }

                return result;
            }
        }

        MatrixValue ImageLoad(String filename, out Boolean error, Double coarsening = Double.NaN)
        {
            error = false;
            var imageType = String.Empty;

            using (var fs = File.Open(filename, FileMode.Open))
            {
                var file_bytes = new Byte[fs.Length];
                fs.Read(file_bytes, 0, 8);

                var png_magic_number = new Byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };

                if (!file_bytes.Take(8).Select((b, i) => b == png_magic_number[i]).Contains(false))
                    imageType = "png";

                var tiff_magic_number_0 = new Byte[] { 0x49, 0x49, 0x2a, 0x00 };

                if (!file_bytes.Take(4).Select((b, i) => b == tiff_magic_number_0[i]).Contains(false))
                    imageType = "tiff";

                var tiff_magic_number_1 = new Byte[] { 0x4d, 0x4d, 0x2a, 0x00 };

                if (!file_bytes.Take(4).Select((b, i) => b == tiff_magic_number_1[i]).Contains(false))
                    imageType = "tiff";

                var bmp_magic_number = new Byte[] { 0x42, 0x4D };

                if (!file_bytes.Take(2).Select((b, i) => b == bmp_magic_number[i]).Contains(false))
                    imageType = "bmp";

                var gif_magic_number_0 = new Byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };

                if (!file_bytes.Take(6).Select((b, i) => b == gif_magic_number_0[i]).Contains(false))
                    imageType = "gif";

                var gif_magic_number_1 = new Byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };

                if (!file_bytes.Take(6).Select((b, i) => b == gif_magic_number_1[i]).Contains(false))
                    imageType = "gif";

                var jpg_magic_number = new Byte[] { 0xff, 0xd8 };

                if (!file_bytes.Take(2).Select((b, i) => b == jpg_magic_number[i]).Contains(false))
                    imageType = "jpg";
            }

            if (imageType == String.Empty)
            {
                error = true;
                return new MatrixValue();
            }

            using (var bmp = new Bitmap(filename))
            {
                var result = default(MatrixValue);

                if (bmp == null)
                {
                    error = true;
                    result = new MatrixValue();
                }
                else
                {
                    var height = bmp.Height;
                    var width = bmp.Width;

                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
                    var ptr = bmpData.Scan0;
                    var bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                    var rgbValues = new Byte[bytes];
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                    bmp.UnlockBits(bmpData);
                    var bytesPerPixel = 0;

                    if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Canonical ||
                        bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb ||
                        bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb ||
                        bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                    {
                        bytesPerPixel = 4;
                    }
                    else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                    {
                        bytesPerPixel = 3;
                    }
                    else
                    {
                        throw new YAMPPixelFormatNotSupportedException(filename);
                    }

                    if (Double.IsNaN(coarsening))
                    {
                        const Double maxPixelPerDirection = 100.0;

                        if (width > maxPixelPerDirection || height > maxPixelPerDirection)
                        {
                            coarsening = Math.Max(width / maxPixelPerDirection, height / maxPixelPerDirection);
                        }
                        else
                        {
                            coarsening = 1.0;
                        }
                    }

                    if (coarsening < 1.0)
                    {
                        throw new YAMPArgumentInvalidException("Load", "ImageCoarsening");
                    }

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

                        for (var j = 0; j < height; j++)
                        {
                            var jdx = (Int32)(j * cI);

                            if (jdx >= finalHeight)
                            {
                                jdx = finalHeight - 1;
                            }

                            rvalues[jdx, idx] += rgbValues[(j * width + i) * bytesPerPixel + 2];
                            gvalues[jdx, idx] += rgbValues[(j * width + i) * bytesPerPixel + 1];
                            bvalues[jdx, idx] += rgbValues[(j * width + i) * bytesPerPixel + 0];
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

                return result;
            }
        }

        //TODO This will MOST probably be cleaned up in future released...
        //Possible Changes: extract all the files to special YAMP File Types and include
        //Methods like Detect() and Extract() for investigating if a certain file
        //has a specific structure.
        //Probably also replace the dependency of System.Drawing with custom routines...
        //This, however, could be too much.
        enum FileType
        {
            Binary,
            Text,
            Image
        }

        #endregion
    }
}

