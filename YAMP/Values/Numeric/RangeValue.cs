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
using System.IO;

namespace YAMP
{
	public class RangeValue : MatrixValue
    {
        #region Members

        int count;

        #endregion

        #region ctor

        public RangeValue(double start, double end, double step)
		{
			Start = start;
			End = end;
			Step = step;
            count = 1 + (int)Math.Floor((end - start) / step);
            DimensionX = 1;
            DimensionY = count;

			if(count < 0)
				throw new RangeException("Negative number of entries detected"); 
			else if(count >= int.MaxValue / 10)
				throw new RangeException("Too many entries in the range");
		}
		
		public RangeValue (double start, double step) : this(start, start, step)
		{
			All = true;
		}

		public RangeValue () : this(1, 1)
		{
        }

        #endregion

        #region Properties

        public double Step { get; private set; }

        public double Start { get; private set; }

        public double End { get; private set; }

        public bool All { get; private set; }

        #endregion

        #region Methods

        public override MatrixValue Clone()
        {
            var m = new MatrixValue(DimensionY, DimensionX);

            for (var j = 1; j <= DimensionY; j++)
                for (var i = 1; i <= DimensionX; i++)
                    m[j, i] = this[j, i].Clone();

            return m;
        }

        ScalarValue GetValue(int i)
        {
            return new ScalarValue(Start + (i - 1) * Step);
        }

        public override ScalarValue this[int j, int i]
        {
            get
            {
                if (i > DimensionX || i < 1 || j > DimensionY || j < 1)
                    throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

                var index = new MatrixIndex();
                index.Column = i;
                index.Row = j;

                if (ContainsIndex(index))
                    return GetIndex(index);

                if(i == 1)
                    return GetValue(j);

                return new ScalarValue();
            }
            set
            {
                base[j, i] = value;
            }
        }

		public override byte[] Serialize()
		{
			var content = base.Serialize();

			using (var ms = new MemoryStream())
			{
				ms.Write(content, 0, content.Length);

				var start = BitConverter.GetBytes(Start);
				var end = BitConverter.GetBytes(End);
				var step = BitConverter.GetBytes(Step);
				var all = BitConverter.GetBytes(All);

				ms.Write(start, 0, start.Length);
				ms.Write(end, 0, end.Length);
				ms.Write(step, 0, step.Length);
				ms.Write(all, 0, all.Length);

				content = ms.ToArray();
			}

			return content;
		}

		public override Value Deserialize(byte[] content)
		{
			base.Deserialize(content);
			Start = BitConverter.ToDouble(content, content.Length - 25);
			End = BitConverter.ToDouble(content, content.Length - 17);
			Step = BitConverter.ToDouble(content, content.Length - 9);
			All = BitConverter.ToBoolean(content, content.Length - 1);
			return this;
		}

        #endregion
    }
}

