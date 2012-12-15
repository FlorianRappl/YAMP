/*
    Copyright (c) 2012, Florian Rappl.
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

        public UnitValue(double real, double imag, string unit) : base(real, imag)
        {
            this.unit = unit;
        }

        public UnitValue(ScalarValue value, string unit) : this(value.Value, value.ImaginaryValue, unit)
        {
        }

        public UnitValue(UnitValue value) : this(value.Value, value.ImaginaryValue, value.Unit)
        {
        }

        public UnitValue(ScalarValue value, StringValue unit) : this(value.Value, value.ImaginaryValue, unit.Value)
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
