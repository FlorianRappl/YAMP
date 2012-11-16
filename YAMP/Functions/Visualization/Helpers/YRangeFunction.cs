using System;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Gets or sets the value for the yrange [min, max] of the y-axis of a plot.")]
    class YRangeFunction : PropertyFunction<MatrixValue>
    {
        public YRangeFunction() : base("YRange")
        {
        }

        protected override object GetValue(MatrixValue parameter)
        {
            return new double[] { parameter[1].Value, parameter[parameter.Length].Value };
        }

        protected override MatrixValue GetValue(object original)
        {
            var ov = original as double[];
            var m = new MatrixValue(2, 1);

            for (var i = 0; i < 2; i++)
                m[i + 1, 1].Value = ov[i];

            return m;
        }
    }
}
