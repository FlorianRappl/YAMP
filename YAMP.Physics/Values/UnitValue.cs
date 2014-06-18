/*
    Copyright (c) 2012-2014, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using YAMP;

namespace YAMP.Physics
{
    public sealed class UnitValue : ScalarValue
    {
        #region Members

        string _unit;

        #endregion

        #region ctor

        public UnitValue() 
            : this(0.0)
        {
        }

        public UnitValue(string unit)
            : this(0.0, unit)
        {
        }

        public UnitValue(double value)
            : this(value, string.Empty)
        {
        }

        public UnitValue(double value, string unit)
            : base(value)
        {
            this._unit = unit;
        }

        public UnitValue(ScalarValue value, string unit)
            : this(value.Re, unit)
        {
        }

        public UnitValue(UnitValue value)
            : this(value.Re, value.Unit)
        {
        }

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
        public override byte[] Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(Re));
            bytes.AddRange(BitConverter.GetBytes(Im));
            bytes.AddRange(BitConverter.GetBytes(_unit.Length));

            for (int i = 0; i < _unit.Length; i++)
                bytes.AddRange(BitConverter.GetBytes(_unit[i]));

            return bytes.ToArray();
        }

        /// <summary>
        /// Transforms a binary representation into a new instance.
        /// </summary>
        /// <param name="content">The binary data.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var unit = new UnitValue();
            unit.Re = BitConverter.ToDouble(content, 0);
            unit.Im = BitConverter.ToDouble(content, 8);
            var str = new char[BitConverter.ToInt32(content, 16)];

            for (int i = 0; i < str.Length; i++)
                str[i] = BitConverter.ToChar(content, 20 + 2 * i);

            unit._unit = new string(str);
            return unit;
        }

        #endregion

        #region Methods

        public override void Clear()
        {
            _unit = string.Empty;
            base.Clear();
        }

        public override ScalarValue Clone()
        {
 	         return new UnitValue(Re, Unit);
        }

        public override string ToString(ParseContext context)
        {
            return base.ToString(context) + " " + _unit;
        }

        #endregion

        #region Register Operators

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

        public static UnitValue AddUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new UnitValue(left + target, left.Unit);
        }

        public static UnitValue SubUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new UnitValue(left - target, left.Unit);
        }

        public static UnitValue MulUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var unit = new CombinedUnit(left.Unit).Multiply(right.Unit).Simplify();
            return new UnitValue(unit.Factor * left * right, unit.Unpack());
        }

        public static UnitValue DivUU(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (UnitValue)b;
            var unit = new CombinedUnit(left.Unit).Divide(right.Unit).Simplify();
            return new UnitValue(unit.Factor * left / right, unit.Unpack());
        }

        public static UnitValue MulSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (UnitValue)b;
            return new UnitValue(left * right, right.Unit);
        }

        public static UnitValue DivSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (UnitValue)b;
            return new UnitValue(left / right, right.Unit);
        }

        public static UnitValue MulUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left * right, left.Unit);
        }

        public static UnitValue DivUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left / right, left.Unit);
        }

        public static UnitValue PowUS(Value a, Value b)
        {
            var left = (UnitValue)a;
            var right = (ScalarValue)b;
            return new UnitValue(left.Pow(right), new CombinedUnit(left._unit).Raise(right.Re).Unpack());
        }

        #endregion
    }
}
