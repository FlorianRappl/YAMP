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
using System.Reflection;

namespace YAMP
{
	public abstract class Value
	{
		static readonly Value _empty = new ScalarValue();
		
		public static Value Empty
		{
			get
			{
				return _empty;
			}
		}

		public string Header
		{
			get
			{
				return GetType().Name.RemoveValueConvention();
			}
		}
		
		public abstract Value Add(Value right);
		
		public abstract Value Subtract(Value right);
		
		public abstract Value Multiply(Value right);
		
		public abstract Value Divide(Value denominator);
		
		public abstract Value Power(Value exponent);

		public abstract byte[] Serialize();

		public abstract Value Deserialize(byte[] content);

		internal static Value Deserialize(string name, byte[] content)
		{
			name = name + "Value";
			var types = Assembly.GetCallingAssembly().GetTypes();

			foreach(var target in types)
			{
				if(target.Name.Equals(name))
				{
                    var value = target.GetConstructor(new Type[0]).Invoke(null) as Value;
					return value.Deserialize(content);
				}
			}

			return Value.Empty;
		}

        public override string ToString()
        {
            return ToString(ParseContext.Default);
        }

        public virtual string ToString(ParseContext context)
        {
            return string.Empty;
        }
	}
}

