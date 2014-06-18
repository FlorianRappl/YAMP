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

namespace YAMP.Physics
{
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

        public static UnitValue Convert(UnitValue fromValue, string targetUnit)
        {
            var cu = new CombinedUnit(fromValue.Unit);
            var conversations = cu.ConvertTo(targetUnit);
            var re = fromValue.Re;

            foreach (var conversation in conversations)
                re = conversation(re);

            return new UnitValue(cu.Factor * re, cu.Unit);
        }
    }
}
