namespace YAMP
{
    using System;

	[Description("DateFunctionDescription")]
	[Kind(PopularKinds.System)]
	sealed class DateFunction : SystemFunction
	{
        public DateFunction(ParseContext context)
            : base(context)
        {
        }


		[Description("DateFunctionDescriptionForVoid")]
		[Example("date()", "DateFunctionExampleForVoid1")]
		public StringValue Function()
		{
			var dt = DateTime.Today;
			return new StringValue(dt.ToString("d"));
		}

		[Description("DateFunctionDescriptionForScalar")]
		[Example("date(100)", "DateFunctionExampleForScalar1")]
		public StringValue Function(ScalarValue offset)
		{
			var dt = DateTime.Today.AddDays(offset.Re);
			return new StringValue(dt.ToString("d"));
		}
	}
}
