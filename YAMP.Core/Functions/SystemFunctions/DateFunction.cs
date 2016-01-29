using System;

namespace YAMP
{
	[Description("The date() function allows you to get dates with or without offset.")]
	[Kind(PopularKinds.System)]
	sealed class DateFunction : SystemFunction
	{
		[Description("Gets the current date, taken at the moment of the query request.")]
		[Example("date()", "Prints the current date.")]
		public StringValue Function()
		{
			var dt = DateTime.Today;
			return new StringValue(dt.ToString("d"));
		}

		[Description("Gets the current date with the specified offset in days.")]
		[Example("date(100)", "Prints the date in 100 days.")]
		public StringValue Function(ScalarValue offset)
		{
			var dt = DateTime.Today.AddDays(offset.Re);
			return new StringValue(dt.ToString("d"));
		}
	}
}
