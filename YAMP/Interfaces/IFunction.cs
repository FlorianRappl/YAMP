using System;

namespace YAMP
{
	public interface IFunction
	{
		Value Perform(ParseContext context, Value argument);
	}
}