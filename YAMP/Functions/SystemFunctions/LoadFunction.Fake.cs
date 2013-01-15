namespace YAMP
{
	[Description("Dummy class; does nothing.")]
	[Kind(PopularKinds.System)]
    class LoadFunction : SystemFunction
	{
        [Description("Dummy method; does nothing.")]
        public StringValue Function(StringValue filename)
        {
            return new StringValue();
		}
	}
}

