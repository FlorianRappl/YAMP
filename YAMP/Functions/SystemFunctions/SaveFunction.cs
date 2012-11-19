using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	[Description("Saves variables from memory in the filesystem.")]
	[Kind(PopularKinds.System)]
    class SaveFunction : SystemFunction
	{
        public SaveFunction()
        {
        }

        [Description("Saves all variables that are currently available.")]
        [Example("save(\"myfile.mat\")", "Saves all variables in the file myfile.mat")]
        public StringValue Function(StringValue filename)
		{
            Save(filename.Value, Context.Variables);
            return new StringValue(Context.Variables.Count + " objects saved.");
		}

        [Description("Saves the specified variables in the file.")]
        [Example("save(\"myfile.mat\", \"x\", \"y\")", "Saves the variables x and y in the file myfile.mat")]
		[Arguments(1)]
        public StringValue Function(StringValue filename, ArgumentsValue args)
        {
            var workspace = new Dictionary<string, Value>();

            foreach (var arg in args.Values)
            {
                if (arg is StringValue)
                {
                    var name = (arg as StringValue).Value;

                    if (Context.Variables.ContainsKey(name))
                        workspace.Add(name, Context.Variables[name]);
                }
            }

            Save(filename.Value, workspace);
            return new StringValue(workspace.Count + " objects saved.");
        }

		static void Save(string filename, IDictionary<string, Value> workspace)
		{
			using(var fs = File.Create(filename))
			{
				foreach(string variable in workspace.Keys)
				{
					var idx = Encoding.Unicode.GetBytes(variable);
					var len = BitConverter.GetBytes (idx.Length);
					fs.Write(len, 0, len.Length);
					fs.Write(idx, 0, idx.Length);
					var value = workspace[variable];
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

