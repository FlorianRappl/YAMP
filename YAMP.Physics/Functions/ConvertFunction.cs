using System;

namespace YAMP.Physics
{
    class ConvertFunction : ArgumentFunction
    {
        public UnitValue Function(ScalarValue value, StringValue from, StringValue to)
        {
            return Function(new UnitValue(value, from), to);
        }

        public UnitValue Function(UnitValue value, StringValue to)
        {
            var cu = new CombinedUnit(value.Unit);
            cu.ConvertTo(to.Value);
            return new UnitValue(cu.Factor * value, cu.Unit);
        }
    }
}
