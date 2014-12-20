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
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Abstract base class for any plot.
    /// </summary>
	public abstract class PlotValue : Value
	{
		#region Events

        /// <summary>
        /// The event that is invoked once the plot data has been changed.
        /// </summary>
		public event EventHandler<PlotEventArgs> OnPlotChanged;

		#endregion

		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public PlotValue()
		{
            Title = string.Empty;
		}

		#endregion

        #region Properties

        /// <summary>
        /// Gets the number of series.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Gets or sets the plot title.
        /// </summary>
        [StringToStringConverter]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of standardcolors (those are just some suggestions, a lot
        /// more colors are possible).
        /// </summary>
		public static string[] StandardColors { get { return standardColors; } }

		#endregion

		#region Methods

        /// <summary>
        /// Passes a reference (not a copy!) to the current object.
        /// </summary>
        /// <returns>The reference to the current object.</returns>
        public override Value Copy()
        {
            return this;
        }

        /// <summary>
        /// Invokes the OnPlotChanged event if it has been set.
        /// </summary>
        /// <param name="property">The property name to take as argument.</param>
		internal void RaisePlotChanged(string property)
		{
			if (OnPlotChanged != null)
			{
				var args = new PlotEventArgs(this, property);
				OnPlotChanged(this, args);
			}
		}

        /// <summary>
        /// Updates the data region, i.e. the complete plot.
        /// </summary>
		public void Update()
		{
			RaisePlotChanged("Data");
		}

        /// <summary>
        /// Updates the properties of the plot series.
        /// </summary>
		public void UpdateProperties()
		{
			RaisePlotChanged("Properties");
		}

        /// <summary>
        /// Updates the layout of the plot, i.e. the properties of the plot itself.
        /// </summary>
		public void UpdateLayout()
		{
			RaisePlotChanged("Layout");
		}

        /// <summary>
        /// Increases the size of an array to n elements, repeating the containing elements.
        /// </summary>
        /// <param name="values">The current array.</param>
        /// <param name="n">The desired size of the array.</param>
        protected double[] MakeArrayPeriodic(double[] values, int n)
        {
            var dest = new double[n];

            if (values.Length > 0)
            {
                for (var i = 0; i < n; i++)
                    dest[i] = values[i % values.Length];
            }

            return dest;
        }

        /// <summary>
        /// Generates an array of double values.
        /// </summary>
        /// <param name="minValue">The first value in the array.</param>
        /// <param name="step">The difference between each element.</param>
        /// <param name="count">The number of elements in the array.</param>
        /// <returns>The double array containing the values.</returns>
        protected double[] Generate(double minValue, double step, int count)
		{
			count = Math.Max(count, 0);
			var values = new double[count];

			if(count > 0)
			{
				values[0] = minValue;

				for (int i = 1; i < count; i++)
				{
					values[i] = values[i - 1] + step;
				}
			}

			return values;
		}

        /// <summary>
        /// Converts a given matrixvalue (seen as a vector) into a double array.
        /// </summary>
        /// <param name="m">The MatrixValue to convert.</param>
        /// <param name="offset">The offset in the matrix (0 = start with 1st element).</param>
        /// <param name="length">The number of elements to convert.</param>
        /// <returns>The double array with the values.</returns>
		protected double[] Convert(MatrixValue m, int offset, int length)
		{
			var values = new double[length];
			var j = offset + 1;

			for (int i = 0; i < length; i++)
			{
				values[i] = m[j].Re;
				j++;
			}

			return values;
		}

        /// <summary>
        /// Converts only column values of the matrix into a double array.
        /// </summary>
        /// <param name="m">The MatrixValue to convert.</param>
        /// <param name="dx">The offset in columns.</param>
        /// <param name="length">The number of rows to consider.</param>
        /// <param name="dy">The offset in rows.</param>
        /// <returns>The double array with the values.</returns>
		protected double[] ConvertX(MatrixValue m, int dx, int length, int dy)
		{
			var values = new double[length];
			var j = dy + 1;
			var k = dx + 1;

			for (int i = 0; i < length; i++)
			{
				values[i] = m[j, k].Re;
				k++;
			}

			return values;
		}

        /// <summary>
        /// Converts only row values of the matrix into a double array.
        /// </summary>
        /// <param name="m">The MatrixValue to convert.</param>
        /// <param name="dy">The offset in rows.</param>
        /// <param name="length">The number of columns to consider.</param>
        /// <param name="dx">The offset in columns.</param>
        /// <returns>The double array with the values.</returns>
		protected double[] ConvertY(MatrixValue m, int dy, int length, int dx)
		{
			var values = new double[length];
			var j = dy + 1;
			var k = dx + 1;

			for (int i = 0; i < length; i++)
			{
				values[i] = m[j, k].Re;
				j++;
			}

			return values;
		}

		#endregion

		#region Statics

		readonly static string[] standardColors = new string[]
		{
			"red", 
			"green",
			"blue",
			"pink",
			"teal",
			"orange",
			"brown",
			"lightblue",
			"violet",
			"yellow",
			"gray",
			"lightgreen",
			"cyan",
			"steelblue",
			"black",
			"gold",
			"silver",
			"forestgreen",
			"blueviolet",
			"darkorange",
			"gainsboro",
			"lightcoral",
			"olivedrab",
			"turquoise",
			"tan",
			"peachpuff"
		};

		#endregion
	}
}
