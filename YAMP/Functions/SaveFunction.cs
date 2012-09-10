using System;
using System.IO;
using System.Text;
using System.Collections;

namespace YAMP
{
	class SaveFunction : ArgumentFunction
	{
		public Value Function(StringValue filename)
		{
			Save(filename.Value, Tokens.Instance.Variables);
			return filename;
		}

		public static void Save(string filename, Hashtable workspace)
		{
			using(var fs = File.Create (filename))
			{
				foreach(string variable in workspace.Keys)
				{
					var idx = Encoding.Unicode.GetBytes(variable);
					var len = BitConverter.GetBytes (idx.Length);
					fs.Write(len, 0, len.Length);
					fs.Write(idx, 0, idx.Length);
					var value = workspace[variable] as Value;
					idx = Encoding.ASCII.GetBytes(value.Header);
					len = BitConverter.GetBytes(idx.Length);
					fs.Write(len, 0, len.Length);
					fs.Write(idx, 0, idx.Length);
					idx = value.Serialize();
					len = BitConverter.GetBytes(idx.Length);
					fs.Write(len, 0, len.Length);
					fs.Write(idx, 0, idx.Length);
				}
			}
		}
	}
}

