using System;
using System.Text;

namespace YAMP
{
	static class Extensions
	{
		public static string Substring(this StringBuilder sb, int startIndex, int length)
		{
			var chars = new char[length];

			for(var i = 0; i < length; i++)
				chars[i] = sb[i + startIndex];

			return new string(chars);
		}

		public static int IndexOf(this StringBuilder sb, char character)
		{
			for(var i = 0; i < sb.Length; i++)
				if(sb[i] == character)
					return i;

			return -1;
		}
	}
}

