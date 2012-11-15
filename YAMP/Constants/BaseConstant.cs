using System;

namespace YAMP
{
	abstract class BaseConstant : IConstants
	{
		string name;

		public BaseConstant()
		{
			name = GetType().Name.Replace("Constant", string.Empty);
		}

		public string Name
		{
			get { return name; }
		}

		public abstract Value Value
		{
			get;
		}
	}
}
