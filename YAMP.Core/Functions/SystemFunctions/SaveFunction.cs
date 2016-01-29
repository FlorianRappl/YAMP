using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	[Description("Saves variables from memory in the filesystem.")]
	[Kind(PopularKinds.System)]
    sealed class SaveFunction : SystemFunction
	{
        [Description("Saves all variables that are currently available.")]
        [Example("save(\"myfile.mat\")", "Saves all variables in the file myfile.mat.", true)]
        public void Function(StringValue fileName)
		{
            Save(fileName.Value, Context.Variables);
            Notify(Context.Variables.Count);
		}

        [Description("Saves the specified variables in the file.")]
        [Example("save(\"myfile.mat\", \"x\", \"y\")", "Saves the variables x and y in the file myfile.mat.", true)]
		[Arguments(1)]
        public void Function(StringValue fileName, ArgumentsValue args)
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

            Save(fileName.Value, workspace);
            Notify(workspace.Count);
        }

        #region Helpers

        public static void Save(string filename, IDictionary<string, Value> workspace)
        {
            using (var fs = File.Create(filename))
            {
                foreach (string variable in workspace.Keys)
                {
                    var idx = Encoding.Unicode.GetBytes(variable);
                    var len = BitConverter.GetBytes(idx.Length);
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

        void Notify(int count)
        {
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Success, count + " objects saved."));
        }

        #endregion
    }
}

