using System;
using YAMP;

namespace YAMP.Physics
{
    [Description("")]
    [Kind(PopularKinds.Function)]
    class UnitFunction : ArgumentFunction
    {
        public UnitValue Function(ScalarValue value, StringValue unit)
        {
            return new UnitValue(value, unit.Value);
        }
    }
}
