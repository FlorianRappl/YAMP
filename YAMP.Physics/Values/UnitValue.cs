namespace YAMP.Physics
{
    using System;
    using System.Collections.Generic;
    using YAMP;

    /// <summary>
    /// Scalar value with unit string.
    /// </summary>
    public sealed class UnitValue : ScalarValue
    {
        #region Fields

        string _unit;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        public UnitValue() 
            : this(0.0)
        {
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="unit">The unit to hold.</param>
        public UnitValue(String unit)
            : this(0.0, unit)
        {
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        public UnitValue(Double value)
            : this(value, String.Empty)
        {
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        /// <param name="unit">The unit to hold.</param>
        public UnitValue(Double value, String unit)
            : base(value)
        {
            _unit = unit;
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        /// <param name="unit">The unit to hold.</param>
        public UnitValue(ScalarValue value, String unit)
            : this(value.Re, unit)
        {
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="value">The unit to copy.</param>
        public UnitValue(UnitValue value)
            : this(value.Re, value.Unit)
        {
        }

        /// <summary>
        /// Creates a new unit value.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        /// <param name="unit">The unit to hold.</param>
        public UnitValue(ScalarValue value, StringValue unit)
            : this(value.Re, unit.Value)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Transforms the instance into a binary representation.
        /// </summary>
        /// <returns>The binary representation.</returns>
        public override Byte[] Serialize()
        {
            var bytes = new List<Byte>();
            bytes.AddRange(BitConverter.GetBytes(Re));
            bytes.AddRange(BitConverter.GetBytes(Im));
            bytes.AddRange(BitConverter.GetBytes(_unit.Length));

            for (var i = 0; i < _unit.Length; i++)
            {
                bytes.AddRange(BitConverter.GetBytes(_unit[i]));
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Transforms a binary representation into a new instance.
        /// </summary>
        /// <param name="content">The binary data.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(Byte[] content)
        {
            var unit = new UnitValue();
            unit.Re = BitConverter.ToDouble(content, 0);
            unit.Im = BitConverter.ToDouble(content, 8);
            var str = new char[BitConverter.ToInt32(content, 16)];

            for (var i = 0; i < str.Length; i++)
            {
                str[i] = BitConverter.ToChar(content, 20 + 2 * i);
            }

            unit._unit = new String(str);
            return unit;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the content.
        /// </summary>
        public override void Clear()
        {
            _unit = String.Empty;
            base.Clear();
        }

        /// <summary>
        /// Clones the value.
        /// </summary>
        /// <returns>The exact clone.</returns>
        public override ScalarValue Clone()
        {
 	         return new UnitValue(Re, Unit);
        }

        /// <summary>
        /// Stringifies the content.
        /// </summary>
        /// <param name="context">The context to use.</param>
        /// <returns>The string representation.</returns>
        public override String ToString(ParseContext context)
        {
            return base.ToString(context) + " " + _unit;
        }

        #endregion

        #region Register Operators

        /// <summary>
        /// Registers the operators.
        /// </summary>
        protected override void RegisterOperators()
        {
            RegisterPlus(typeof(UnitValue), typeof(UnitValue), AddUU);
            RegisterMinus(typeof(UnitValue), typeof(UnitValue), SubUU);

            RegisterMultiply(typeof(UnitValue), typeof(UnitValue), MulUU);
            RegisterMultiply(typeof(ScalarValue), typeof(UnitValue), MulSU);
            RegisterMultiply(typeof(UnitValue), typeof(ScalarValue), MulUS);

            RegisterDivide(typeof(UnitValue), typeof(UnitValue), DivUU);
            RegisterDivide(typeof(ScalarValue), typeof(UnitValue), DivSU);
            RegisterDivide(typeof(UnitValue), typeof(ScalarValue), DivUS);

            RegisterPower(typeof(UnitValue), typeof(ScalarValue), PowUS);
        }

        static UnitValue AddUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new UnitValue(left + target, left.Unit);
        }

        static UnitValue SubUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new UnitValue(left - target, left.Unit);
        }

        static UnitValue MulUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var unit = new CombinedUnit(left.Unit).Multiply(right.Unit).Simplify();
            return new UnitValue(unit.Factor * left * right, unit.Unpack());
        }

        static UnitValue DivUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var unit = new CombinedUnit(left.Unit).Divide(right.Unit).Simplify();
            return new UnitValue(unit.Factor * left / right, unit.Unpack());
        }

        static UnitValue MulSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (UnitValue)b;
            return new UnitValue(left * right, right.Unit);
        }

        static UnitValue DivSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (UnitValue)b;
            return new UnitValue(left / right, right.Unit);
        }

        static UnitValue MulUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left * right, left.Unit);
        }

        static UnitValue DivUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left / right, left.Unit);
        }

        static UnitValue PowUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left.Pow(right), new CombinedUnit(left._unit).Raise(right.Re).Unpack());
        }

        #endregion
    }
}
