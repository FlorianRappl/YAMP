using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	class LoadFunction : ArgumentFunction
	{
        public LoadFunction() : base(2)
        {
        }

		public Value Function(StringValue filename)
		{
            if (!File.Exists(filename.Value))
                throw new FileNotFoundException("The specified file has not been found.");

			var v = Load(filename.Value);

			foreach(var key in v.Keys)
				Tokens.Instance.AssignVariable(key, v[key]);

			return new StringValue(v.Count + " objects loaded.");
		}

        public Value Function(StringValue filename, ArgumentsValue args)
        {
            if (!File.Exists(filename.Value))
                throw new FileNotFoundException("The specified file has not been found.");

            var v = Load(filename.Value);
            var count = 0;

            foreach (var arg in args.Values)
            {
                if (arg is StringValue)
                {
                    var name = (arg as StringValue).Value;

                    if (v.ContainsKey(name))
                    {
                        Tokens.Instance.AssignVariable(name, v[name] as Value);
                        count++;
                    }
                }
            }

            return new StringValue(count + " objects loaded.");
        }
		
		public static IDictionary<string, Value> Load(string filename)
		{
            var ht = new Dictionary<string, Value>();
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

