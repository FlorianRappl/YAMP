namespace YAMP
{
    using System;

    /// <summary>
    /// A container constant is a wrapper for custom constants
    /// that are added to the engine just by a double with a name.
    /// </summary>
	[Description("ContainerConstantDescription")]
	[Kind(PopularKinds.Constant)]
	sealed class ContainerConstant : IConstants
	{
		readonly String _name;
		readonly Value _value;

		public ContainerConstant(String name, Value value)
		{
			_name = name;
			_value = value;
		}

		public String Name
		{
			get { return _name; }
		}

		public Value Value
		{
			get { return _value; }
		}
	}
}
