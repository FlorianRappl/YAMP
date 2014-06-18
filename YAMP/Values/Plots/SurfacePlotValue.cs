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
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Contains the data for surface and mesh plots.
    /// </summary>
    public sealed class SurfacePlotValue : XYZPlotValue
    {
        #region Members

        Points<Vertex> data;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new SurfacePlot.
        /// </summary>
        public SurfacePlotValue()
        {
            data = new Points<Vertex>();
            ColorPalette = ColorPalettes.Jet;
            XLabel = "X";
            YLabel = "Y";
            ZLabel = "Z";
            points.Add(data);
            IsMesh = false;
            IsSurf = true;
            MeshThickness = 1.0;
            Color = "#444444";
            ShowLegend = false;
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Represents one point in the surface plot.
        /// </summary>
        public struct Vertex
        {
            /// <summary>
            /// Gets the x value.
            /// </summary>
            public double X;

            /// <summary>
            /// Gets the y value.
            /// </summary>
            public double Y;

            /// <summary>
            /// Gets the z value.
            /// </summary>
            public double Z;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color palette to use.
        /// </summary>
        [StringToEnumConverter(typeof(ColorPalettes))]
        public ColorPalettes ColorPalette
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the mesh.
        /// </summary>
        [StringToStringConverter]
        public string Color
        {
            get { return data.Color; }
            set { data.Color = value; }
        }

        /// <summary>
        /// Gets or sets if it should display the mesh.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool IsMesh
        {
            get { return data.Lines; }
            set { data.Lines = value; }
        }

        /// <summary>
        /// Gets or sets the thickness of the mesh.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MeshThickness
        {
            get { return data.LineWidth; }
            set { data.LineWidth = value; }
        }

        /// <summary>
        /// Gets or sets if it should display the surface.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool IsSurf
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of points in the surface plot.
        /// </summary>
        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

        /// <summary>
        /// Gets the number of horizontal points.
        /// </summary>
        public int Nx
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of vertical points.
        /// </summary>
        public int Ny
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the z values given in the matrix m with the corresponding x and y values.
        /// </summary>
        /// <param name="x">The matrix with the x values.</param>
        /// <param name="y">The matrix with the y values.</param>
        /// <param name="z">The matrix with the function values / z values.</param>
        public void AddPoints(MatrixValue x, MatrixValue y, MatrixValue z)
        {
            data.Clear();

            if (x.Length > 0)
            {
                MinX = x[1, 1].Re;
                MaxX = x[1, 1].Re;
            }
            else
            {
                MinX = 0;
                MaxX = 0;
            }

            if (y.Length > 0)
            {
                MinY = y[1, 1].Re;
                MaxY = y[1, 1].Re;
            }
            else
            {
                MinY = 0;
                MaxY = 0;
            }

            if (z.Length > 0)
            {
                MinZ = z[1, 1].Re;
                MaxZ = z[1, 1].Re;
            }
            else
            {
                MinZ = 0;
                MaxZ = 0;
            }

            if (x.IsVector && y.IsVector)
            {
                var cols = Math.Min(x.Length, z.Columns);
                var rows = Math.Min(y.Length, z.Rows);
                Nx = cols;
                Ny = rows;

                for (var j = 1; j <= rows; j++)
                {
                    for (var i = 1; i <= cols; i++)
                    {
                        var v = new Vertex
                        {
                            X = x[i].Re,
                            Y = y[j].Re,
                            Z = z[j, i].Re
                        };
                        data.Add(v);

                        MinX = Math.Min(MinX, v.X);
                        MaxX = Math.Max(MaxX, v.X);
                        MinY = Math.Min(MinY, v.Y);
                        MaxY = Math.Max(MaxY, v.Y);
                        MinZ = Math.Min(MinZ, v.Z);
                        MaxZ = Math.Max(MaxZ, v.Z);
                    }
                }
            }
            else
            {
                var cols = Math.Min(x.Columns, Math.Min(y.Columns, z.Columns));
                var rows = Math.Min(x.Rows, Math.Min(y.Rows, z.Rows));
                Nx = cols;
                Ny = rows;

                for (var j = 1; j <= rows; j++)
                {
                    for (var i = 1; i <= cols; i++)
                    {
                        var v = new Vertex
                        {
                            X = x[j, i].Re,
                            Y = y[j, i].Re,
                            Z = z[j, i].Re
                        };
                        data.Add(v);

                        MinX = Math.Min(MinX, v.X);
                        MaxX = Math.Max(MaxX, v.X);
                        MinY = Math.Min(MinY, v.Y);
                        MaxY = Math.Max(MaxY, v.Y);
                        MinZ = Math.Min(MinZ, v.Z);
                        MaxZ = Math.Max(MaxZ, v.Z);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the function values given in the matrix m with generated values for x and y.
        /// </summary>
        /// <param name="m">The values for the plot.</param>
        public override void AddPoints(MatrixValue m)
        {
            data.Clear();
            MinX = 1;
            MinY = 1;
            MaxX = m.Columns;
            MaxY = m.Rows;
            Nx = m.Columns;
            Ny = m.Rows;

            if (m.Length > 0)
            {
                MinZ = m[1, 1].Re;
                MaxZ = m[1, 1].Re;
            }
            else
            {
                MinZ = 0;
                MaxZ = 0;
            }

            for (var j = 1; j <= m.Rows; j++)
            {
                for (var i = 1; i <= m.Columns; i++)
                {
                    var z = m[j, i].Re;

                    data.Add(new Vertex
                    {
                        X = i,
                        Y = j,
                        Z = z
                    });

                    MinZ = Math.Min(MinZ, z);
                    MaxZ = Math.Max(MaxZ, z);
                }
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Converts the given instance to an array of bytes.
        /// </summary>
        /// <returns>The binary representation of this instance.</returns>
        public override byte[] Serialize()
        {
            using (var s = Serializer.Create())
            {
                Serialize(s);
                s.Serialize(IsMesh);
                s.Serialize((int)ColorPalette);
                data.Serialize(s);
                s.Serialize(Count);

                for (int j = 0; j < Count; j++)
                {
                    s.Serialize(data[j].X);
                    s.Serialize(data[j].Y);
                    s.Serialize(data[j].Z);
                }

                return s.Value;
            }
        }

        /// <summary>
        /// Converts a set of bytes to a new instance.
        /// </summary>
        /// <param name="content">The binary representation.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var sp = new SurfacePlotValue();

            using (var ds = Deserializer.Create(content))
            {
                sp.Deserialize(ds);
                sp.IsMesh = ds.GetBoolean();
                sp.ColorPalette = (ColorPalettes)ds.GetInt();
                sp.data.Deserialize(ds);
                var count = ds.GetInt();

                for (int j = 0; j < count; j++)
                {
                    var x = ds.GetDouble();
                    var y = ds.GetDouble();
                    var z = ds.GetDouble();

                    data.Add(new Vertex
                    {
                        X = x,
                        Y = y,
                        Z = z
                    });
                }
            }

            return sp;
        }

        #endregion

        #region Index

        /// <summary>
        /// Gets one particular series of the surface plot (we only have 1 surface plot series!).
        /// </summary>
        /// <param name="index">Obsolete - all the same.</param>
        /// <returns>The series of points.</returns>
        public Points<Vertex> this[int index]
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Gets one particular point of the surface plot.
        /// </summary>
        /// <param name="index">Obsolete - all the same.</param>
        /// <param name="point">The vertex to get (0.. N - 1, where N is the number of points).</param>
        /// <returns>The specified vertex value.</returns>
        public Vertex this[int index, int point]
        {
            get
            {
                return data[point];
            }
        }

        #endregion
    }
}
