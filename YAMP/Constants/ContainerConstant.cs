using System;

namespace YAMP
{
    /// <summary>
    /// A container constant is a wrapper for custom constants
    /// that are added to the engine just by a double with a name.
    /// </summary>
	[Description("A custom constant defined by you.")]
	[Kind(PopularKinds.Constant)]
	class ContainerConstant : IConstants
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
