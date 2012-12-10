using System;
using YAMP;

namespace YAMP.Physics
{
    class UnitValue : ScalarValue
    {
        #region Members

        string unit;

        #endregion

        #region ctor

        public UnitValue() : this(string.Empty)
        {
        }

        public UnitValue(string unit)
        {
            this.unit = unit;
        }

        public UnitValue(double real) : base(real)
        {
            unit = string.Empty;
        }

        public UnitValue(double real, double imag) : base(real, imag)
        {
            unit = string.Empty;
        }

        public UnitValue(double real, string unit) : base(real)
        {
            this.unit = unit;
        }

        public UnitValue(double real, double imag, string unit)
            : base(real, imag)
        {
            this.unit = unit;
        }

        public UnitValue(ScalarValue value, string unit) : this(value.Value, value.ImaginaryValue, unit)
        {
        }

        #endregion

        #region Properties

        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
            }
        }

        #endregion

        #region Methods

        public override ScalarValue Clone()
        {
 	         return new UnitValue(Value, ImaginaryValue, Unit);
        }

        public override Value Subtract(Value right)
        {
            return base.Subtract(right);
        }

        public override Value Power(Value exponent)
        {
            return base.Power(exponent);
        }

        public override Value Divide(Value right)
        {
            return base.Divide(right);
        }

        public override Value Multiply(Value right)
        {
            return base.Multiply(right);
        }

        public override Value Add(Value right)
        {
            return base.Add(right);
        }

        public override string ToString(ParseContext context)
        {
            return base.ToString(context) + " " + unit;
        }

        #endregion
    }
}
