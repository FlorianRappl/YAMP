using System;
using System.IO;
using System.Text;

namespace YAMP
{
    [Kind(PopularKinds.System)]
	[Description("Lists the contents of the current working directory.")]
    sealed class LsFunction : SystemFunction
	{
		[Description("Reads the file system table and lists the contents of the current working directory.")]
		[Example("ls()", "Lists name, attributes and the time of the last modification of the current working directory's files and sub-directories.", true)]
		public StringValue Function()
		{
			return Function(new StringValue("*"));
		}

		[Description("Reads the file system table and lists the filtered contents of the current working directory.")]
		[Example("ls(\"*.png\")", "Lists only png files with name, attributes and the time of the last modification of the current working directory.", true)]
		public StringValue Function(StringValue filter)
		{
			var sb = new StringBuilder();
			var dir = new DirectoryInfo(Environment.CurrentDirectory);

			sb.AppendFormat("{0,-32} {1,-10}   {2}", "Name", "Attributes", "Last changed").AppendLine();
			sb.AppendLine("------------------------------------------------------------");

			foreach (var subdir in dir.GetDirectories(filter.Value))
			{
				sb.AppendFormat("{0,-32}  {1,-8}  {2} {3}", Limit(subdir.Name), PrintAttributes(subdir.Attributes),
					subdir.LastWriteTime.ToShortDateString(), subdir.LastWriteTime.ToShortTimeString());
				sb.AppendLine();
			}

			foreach (var file in dir.GetFiles(filter.Value))
			{
				sb.AppendFormat("{0,-32}  {1,-8}  {2} {3}", Limit(file.Name), PrintAttributes(file.Attributes),
					file.LastWriteTime.ToShortDateString(), file.LastWriteTime.ToShortTimeString());
				sb.AppendLine();
			}

			return new StringValue(sb.ToString());
		}

		string Limit(string p)
		{
			if (p.Length > 29)
				return p.Substring(0, 29) + "...";

			return p;
		}

	    string PrintAttributes(FileAttributes fileAttributes)
		{
			var sb = new StringBuilder();
			sb.Append(Check(fileAttributes, FileAttributes.Directory, 'd'));
			sb.Append(Check(fileAttributes, FileAttributes.Hidden, 'h'));
			sb.Append(Check(fileAttributes, FileAttributes.ReadOnly, 'r'));
			sb.Append(Check(fileAttributes, FileAttributes.System, 's'));
			sb.Append(Check(fileAttributes, FileAttributes.Encrypted, 'e'));
			sb.Append(Check(fileAttributes, FileAttributes.Compressed, 'c'));
			sb.Append(Check(fileAttributes, FileAttributes.Archive, 'a'));
			sb.Append(Check(fileAttributes, FileAttributes.Temporary, 't'));
			return sb.ToString();
		}

		char Check(FileAttributes there, FileAttributes premise, char ok)
		{
			if((there & premise) == premise)
				return ok;

			return '-';
		}
	}
}
