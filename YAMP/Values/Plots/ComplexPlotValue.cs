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

namespace YAMP
{
    /// <summary>
    /// Holds the complexplot data.
    /// </summary>
    public sealed class ComplexPlotValue : XYPlotValue
    {
        #region Members

        FunctionValue f;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new complex plot.
        /// </summary>
        public ComplexPlotValue()
        {
            XLabel = "Real";
            YLabel = "Imaginary";
            ShowLegend = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the function to calculate the complex values.
        /// </summary>
        public Func<ScalarValue, ScalarValue> Fz
        {
            get
            {
                if (f != null)
                    return z => f.Perform(ParseContext.Default, z) as ScalarValue;

                return z => z;
            }
        }

        #endregion

        #region Deactivated Properties

        //Those properties are being "deactivated" by design - we just
        //override them but do not use any Converter for them. Therefore
        //YAMP will ignore them since Reflection does not consider the
        //Attributes being "inherited".

        /// <summary>
        /// Obsolete.
        /// </summary>
        public override bool ShowLegend
        {
            get
            {
                return base.ShowLegend;
            }
            set
            {
                base.ShowLegend = value;
            }
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        public override string LegendBackground
        {
            get
            {
                return base.LegendBackground;
            }
            set
            {
                base.LegendBackground = value;
            }
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        public override string LegendLineColor
        {
            get
            {
                return base.LegendLineColor;
            }
            set
            {
                base.LegendLineColor = value;
            }
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        public override double LegendLineWidth
        {
            get
            {
                return base.LegendLineWidth;
            }
            set
            {
                base.LegendLineWidth = value;
            }
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        public override LegendPosition LegendPosition
        {
            get
            {
                return base.LegendPosition;
            }
            set
            {
                base.LegendPosition = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the function to use for the complex plot.
        /// </summary>
        /// <param name="function">The function to consider.</param>
        public void SetFunction(FunctionValue function)
        {
            f = function;
        }

        /// <summary>
        /// A complex plot cannot have any points assigned. You have to
        /// assign a function instead.
        /// </summary>
        /// <param name="m">Useless.</param>
        public override void AddPoints(MatrixValue m)
        {
            //Leave empty
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Converts the instance into bytes.
        /// </summary>
        /// <returns>The binary content of this instance.</returns>
        public override byte[] Serialize()
        {
            using (var s = Serializer.Create())
            {
                Serialize(s);
                s.Serialize(f.Serialize());
                return s.Value;
            }
        }

        /// <summary>
        /// Creates a new instance from the given bytes.
        /// </summary>
        /// <param name="content">The binary content to create a new instance from.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var cp = new ComplexPlotValue();

            using (var ds = Deserializer.Create(content))
            {
                cp.Deserialize(ds);
                var ctn = ds.GetBytes();
                cp.f = new FunctionValue().Deserialize(ctn) as FunctionValue;
            }

            return cp;
        }

        #endregion
    }
}
