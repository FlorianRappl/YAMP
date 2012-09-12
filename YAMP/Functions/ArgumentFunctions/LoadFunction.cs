using System;
using System.IO;
using System.Text;
using System.Collections;

namespace YAMP
{
	class LoadFunction : ArgumentFunction
	{
		public Value Function(StringValue filename)
		{
            if (!File.Exists(filename.Value))
                throw new FileNotFoundException("The specified file has not been found.");

			var v = Load (filename.Value);

			foreach(string key in v.Keys)
				Tokens.Instance.AssignVariable(key, v[key] as Value);

			return new StringValue(v.Count + " objects loaded.");
		}
		
		public static Hashtable Load(string filename)
		{
			var ht = new Hashtable();
			var lenbuffer = new byte[4];
			var ctnbuffer = new byte[0];

			using(var fs = File.Open (filename, FileMode.Open))
			{
				while(fs.Position < fs.Length)
				{
					fs.Read(lenbuffer, 0, lenbuffer.Length);
					var length = BitConverter.ToInt32 (lenbuffer, 0);
					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var name = Encoding.Unicode.GetString(ctnbuffer);
					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32 (lenbuffer, 0);
					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var header = Encoding.ASCII.GetString(ctnbuffer);
					fs.Read(lenbuffer, 0, lenbuffer.Length);
					length = BitConverter.ToInt32 (lenbuffer, 0);
					ctnbuffer = new byte[length];
					fs.Read(ctnbuffer, 0, ctnbuffer.Length);
					var value = Value.Deserialize(header, ctnbuffer);
					ht.Add(name, value);
				}
			}

			return ht;
		}
	}
}

