using System;

namespace YAMP
{
	class YAMPTypesNotEqualException : YAMPRuntimeException
	{
        public YAMPTypesNotEqualException(string leftType, string rightType)
            : base("The types {0} and {1} must be equal, however, they are not.", leftType, rightType)
		{
		}
	}
}

