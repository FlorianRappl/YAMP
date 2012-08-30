using System;
namespace YAMP
{
	public interface IFunction
	{
		Value Perform(Value argument);
	}
}

