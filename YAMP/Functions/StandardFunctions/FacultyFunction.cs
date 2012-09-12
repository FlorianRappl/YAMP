using System;

namespace YAMP
{
	class FacultyFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Faculty();
        }
	}
}

