/*
	Copyright (c) 2012-2013, Florian Rappl.
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
    /// A container class for various plots.
    /// </summary>
    public sealed class SubPlotValue : PlotValue, IFunction, ISetFunction
    {
        #region Members

        List<SubPlot> subplots;

        #endregion

        #region ctor

        public SubPlotValue()
        {
            subplots = new List<SubPlot>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of contained plots.
        /// </summary>
        public override int Count
        {
            get { return subplots.Count; }
        }

        /// <summary>
        /// Gets or sets the number of rows in the subplot.
        /// </summary>
        [ScalarToIntegerConverter]
        public int Rows
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of columns in the subplot.
        /// </summary>
        [ScalarToIntegerConverter]
        public int Columns
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public SubPlotValue AddSubPlot(int row, int column, PlotValue plot, int rowSpan = 1, int columnSpan = 1)
        {
            if (row < 1 || row > Rows)
                throw new YAMPIndexOutOfBoundException(row, 1, Rows);
            else if (column < 1 || column > Columns)
                throw new YAMPIndexOutOfBoundException(column, 1, Columns);

            subplots.Add(new SubPlot
            {
                Row = row,
                RowSpan = Math.Min(Rows - row + 1, rowSpan),
                Column = column,
                ColumnSpan = Math.Min(Columns - column + 1, columnSpan),
                Plot = plot
            });

            UpdateLayout();
            return this;
        }

        #endregion

        #region Serialization

        public override byte[] Serialize()
        {
            using (var s = Serializer.Create())
            {
                s.Serialize(Title);
                s.Serialize(Rows);
                s.Serialize(Columns);
                s.Serialize(Count);

                foreach (var subplot in subplots)
                {
                    s.Serialize(subplot.Row);
                    s.Serialize(subplot.RowSpan);
                    s.Serialize(subplot.Column);
                    s.Serialize(subplot.ColumnSpan);
                    s.Serialize(subplot.Plot.Header);
                    s.Serialize(subplot.Plot.Serialize());
                }

                return s.Value;
            }
        }

        public override Value Deserialize(byte[] content)
        {
            using (var ds = Deserializer.Create(content))
            {
                Title = ds.GetString();
                Rows = ds.GetInt();
                Columns = ds.GetInt();
                var length = ds.GetInt();

                for (var i = 0; i < length; i++)
                {
                    var subplot = new SubPlot();
                    subplot.Row = ds.GetInt();
                    subplot.RowSpan = ds.GetInt();
                    subplot.Column = ds.GetInt();
                    subplot.ColumnSpan = ds.GetInt();
                    var name = ds.GetString();
                    subplot.Plot = (PlotValue)Value.Deserialize(name, ds.GetBytes());
                    subplots.Add(subplot);
                }

                return this;
            }
        }

        #endregion

        #region Nested

        public class SubPlot
        {
            public int Row { get; set; }

            public int Column { get; set; }

            public int RowSpan { get; set; }

            public int ColumnSpan { get; set; }

            public PlotValue Plot { get; set; }
        }

        #endregion

        #region Function Behavior

        public Value Perform(ParseContext context, Value argument)
        {
            if (argument is ScalarValue)
            {
                var index = ((ScalarValue)argument).IntValue;

                if (index < 1 || index > subplots.Count)
                    throw new YAMPIndexOutOfBoundException(index, 1, subplots.Count);

                return subplots[index - 1].Plot;
            }
            else if (argument is ArgumentsValue)
            {
                var av = (ArgumentsValue)argument;

                if (av.Length != 2)
                    throw new YAMPArgumentNumberException("SubPlot", av.Length, 2);

                var row = 1;
                var column = 1;

                if (av[1] is ScalarValue)
                    row = ((ScalarValue)av[1]).IntValue;
                else
                    throw new YAMPArgumentWrongTypeException(av[1].Header, "Scalar", "SubPlot");

                if (av[2] is ScalarValue)
                    column = ((ScalarValue)av[2]).IntValue;
                else
                    throw new YAMPArgumentWrongTypeException(av[2].Header, "Scalar", "SubPlot");

                if (row < 1 || row > Rows)
                    throw new YAMPIndexOutOfBoundException(row, 1, Rows);
                else if (column < 1 || column > Columns)
                    throw new YAMPIndexOutOfBoundException(column, 1, Columns);

                for (var i = subplots.Count - 1; i >= 0; i--)
                {
                    if (subplots[i].Row <= row && subplots[i].Row + subplots[i].RowSpan - 1 >= row &&
                        subplots[i].Column <= column && subplots[i].Column + subplots[i].ColumnSpan - 1 >= column)
                        return subplots[i].Plot;
                }

                return this;
            }

            throw new YAMPWrongTypeSuppliedException(argument.Header, "Scalar");
        }

        public Value Perform(ParseContext context, Value indices, Value values)
        {
            if (values is PlotValue)
            {
                if (indices is ArgumentsValue)
                {
                    var av = (ArgumentsValue)indices;

                    if(av.Length != 2)
                        throw new YAMPArgumentNumberException("SubPlot", av.Length, 2);

                    int rowIndex = 0;
                    int rowSpan = 0;
                    int colIndex = 0;
                    int colSpan = 0;

                    InspectIndex(av[1], out rowIndex, out rowSpan);
                    InspectIndex(av[2], out colIndex, out colSpan);
                    return AddSubPlot(rowIndex, colIndex, (PlotValue)values, rowSpan, colSpan);
                }

                throw new YAMPArgumentNumberException("SubPlot", 1, 2);
            }

            throw new YAMPWrongTypeSuppliedException(values.Header, "Plot");
        }

        void InspectIndex(Value value, out int index, out int span)
        {
            index = 1;
            span = 1;

            if (value is ScalarValue)
                index = ((ScalarValue)value).IntValue;
            else if (value is MatrixValue)
            {
                var M = (MatrixValue)value;
                index = M[1].IntValue;
                span = M.Length;
            }
            else
                throw new YAMPArgumentWrongTypeException(value.Header, new string[] { "Matrix", "Scalar" }, "SubPlot");
        }

        #endregion
    }
}
