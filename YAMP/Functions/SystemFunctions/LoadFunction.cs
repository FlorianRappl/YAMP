using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	[Description("Loads compatible files into YAMP.")]
	[Kind(PopularKinds.System)]
    class LoadFunction : SystemFunction
	{
        public LoadFunction()
        {
        }

        [Description("Loads all variables found in the file.")]
        [Example("load(\"myfile.mat\")", "Opens the file myfile.mat and reads out all variables.")]
        public StringValue Function(StringValue filename)
		{
            if (!File.Exists(filename.Value))
                throw new FileNotFoundException("The specified file has not been found.");

			var error = false;
			var v = Load(filename.Value, out error);
			var count = 0;

			if (error)
			{
				var table = ASCIILoad(filename.Value, out error);

				if (error)
					throw new FileLoadException("File format not supported.", filename.Value);

				var suffix = 0;
				var name = "data";

				do
				{
					suffix++;
				}
				while (Context.Variables.ContainsKey(name + suffix));

				Context.AssignVariable(name, table);
				count = 1;
			}
			else
			{
				foreach (var key in v.Keys)
					Context.AssignVariable(key, v[key]);

				count = v.Count;
			}

			return new StringValue(count + " objects loaded.");
		}

        [Description("Loads specified variables found in the file.")]
        [Example("load(\"myfile.mat\", \"x\", \"y\", \"z\")", "Opens the file myfile.mat and reads out variables that have been named x, y and z.")]
		[Arguments(1, 1)]
        public StringValue Function(StringValue filename, ArgumentsValue args)
        {
            if (!File.Exists(filename.Value))
                throw new FileNotFoundException("The specified file has not been found.");

			var error = false;
            var v = Load(filename.Value, out error);
			var count = 0;

			if (error)
			{
				var table = ASCIILoad(filename.Value, out error);

				if (error)
					throw new FileLoadException("File format not supported.", filename.Value);

				var name = "data";

				if (args.Length > 0 && args.Values[0] is StringValue)
					name = (args.Values[0] as StringValue).Value;
				else
				{
					var suffix = 0;

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
			else
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

			return new StringValue(count + " objects loaded.");
        }

		static IDictionary<string, Value> Load(string filename, out bool error)
		{
			var ht = new Dictionary<string, Value>();
			var lenbuffer = new byte[4];
			var ctnbuffer = new byte[0];

			error = false;

			using (var fs = File.Open(filename, FileMode.Open))
			{
				while (fs.Position < fs.Length)
				{
					fs.Read(lenbuffer, 0, lenbuffer.Length);
					var length = BitConverter.ToInt32(lenbuffer, 0);

					if (fs.Position + length > fs.Length)
					{
						error = true;
						break;
					}

					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var name = Encoding.Unicode.GetString(ctnbuffer);

					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32(lenbuffer, 0);

					if (fs.Position + length > fs.Length)
					{
						error = true;
						break;
					}

					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var header = Encoding.ASCII.GetString(ctnbuffer);

					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32(lenbuffer, 0);

					if (fs.Position + length > fs.Length)
					{
						error = true;
						break;
					}

					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var value = Value.Deserialize(header, ctnbuffer);
					ht.Add(name, value);
				}
			}

			if (error)
				ht.Add("FileParsingError", Value.Empty);

			return ht;
		}

		MatrixValue ASCIILoad(string filename, out bool error)
		{
			error = false;

			using (var fs = File.Open(filename, FileMode.Open))
			{
				var file_bytes = new byte[fs.Length];
				fs.Read(file_bytes, 0, file_bytes.Length);

				var file_string = Encoding.ASCII.GetString(file_bytes);

				var lines = file_string.Split('\n').Select(line => line.Split(new char[]{' ', '\t', ',', '\r'}, StringSplitOptions.RemoveEmptyEntries)).ToList();

				var numberOfLines = lines.Count;

				double parseResult;
				int numberOfHeaderLines = 0;

				while (numberOfHeaderLines < numberOfLines && !double.TryParse(lines[numberOfHeaderLines].FirstOrDefault(), out parseResult))
					numberOfHeaderLines++;

				int numberOfFooterLines = 0;

				while (numberOfFooterLines < numberOfLines - numberOfHeaderLines && !double.TryParse(lines[numberOfLines - 1 - numberOfFooterLines].FirstOrDefault(), out parseResult))
					numberOfFooterLines++;

				if (numberOfLines <= numberOfHeaderLines + numberOfFooterLines)
				{
					error = true;
					return new MatrixValue();
				}

				var tokensPerLine = lines.Select(line => line.Length).SkipWhile((item, index) => index < numberOfHeaderLines).Reverse().SkipWhile((item, index) => index < numberOfFooterLines).Reverse().ToList();

				var numberOfColumns = tokensPerLine.Max();
				var numberOfRows = numberOfLines - numberOfHeaderLines - numberOfFooterLines;

				var result = new MatrixValue(numberOfRows, numberOfColumns);

				for (int i = 0; i < numberOfRows; i++)
				{
					for (int j = 0; j < tokensPerLine[i]; j++)
					{
						if (double.TryParse(lines[numberOfHeaderLines + i][j], out parseResult))
							result[i + 1, j + 1] = new ScalarValue(parseResult);
						else
							result[i + 1, j + 1] = new ScalarValue(double.NaN);
					}
				}

				return result;
			}
		}
	}
}

