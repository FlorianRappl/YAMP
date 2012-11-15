using System;

namespace YAMP
{
	[Description("A custom constant defined by you.")]
	[Kind(PopularKinds.Constant)]
	public class ContainerConstant : IConstants
	{
		string name;
		Value value;

		public ContainerConstant(string name, Value value)
		{
			this.name = name;
			this.value = value;
		}

		public string Name
		{
			get { return name; }
		}

		public Value Value
		{
			get { return value; }
		}
	}
}
