using System;

namespace YAMP
{
	class FloorFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var re = Math.Floor(value.Value);
            var im = Math.Floor(value.ImaginaryValue);
			return new ScalarValue(re, im);
        }	
	}
}

