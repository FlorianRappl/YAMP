namespace YAMP.Physics
{
    using System;

    [Description("The unit converter is able to convert some simple and more advanced units. This function understands the basic SI units, the CGS system and is able to use prefixes like k for kilo or M for Mega. See the usage examples for more information.")]
    [Kind(PopularKinds.Conversion)]
    class ConvertFunction : ArgumentFunction
    {
        [Description("Converts the given value of the source unit to the target unit.")]
        [Example("convert(1, \"m/s\", \"km/h\")", "Converts 1 m/s to km/h resulting in 3.6 km/h.")]
        public UnitValue Function(ScalarValue value, StringValue from, StringValue to)
        {
            return Convert(new UnitValue(value, from), to.Value);
        }

        [Description("Convert the given unit value to the target unit.")]
        [Example("convert(unit(5, \"yd\"), \"ft\")", "Converts the unit value 5 yards to feet. This yields 15 ft.")]
        public UnitValue Function(UnitValue value, StringValue to)
        {
            return Convert(value, to.Value);
        }

        public static UnitValue Convert(UnitValue fromValue, String targetUnit)
        {
            var cu = new CombinedUnit(fromValue.Unit);
            var conversions = cu.ConvertTo(targetUnit);
            var re = fromValue.Re;

            foreach (var conversation in conversions)
            {
                re = conversation(re);
            }

            return new UnitValue(cu.Factor * re, cu.Unit);
        }
    }
}
