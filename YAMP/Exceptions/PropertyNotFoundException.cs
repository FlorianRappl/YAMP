using System;

namespace YAMP
{
	public class PropertyNotFoundException : YAMPException
	{
		public PropertyNotFoundException(string givenProperty, string[] availableProperties)
			: base("The given property {0} does not exist. Available properties: [ {1} ].", givenProperty, string.Join(", ", availableProperties))
		{

		}
	}
}
