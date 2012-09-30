using System;

namespace YAMP
{
    [Description("Represents the faculty function, which is used for the ! operator and integer expressions.")]
	class FacultyFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Faculty();
        }
	}
}

