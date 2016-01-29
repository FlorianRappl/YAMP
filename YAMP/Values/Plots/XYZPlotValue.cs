using System;
using System.Collections.Generic;
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Abstract base class for plot values that are based on an XYZ
    /// (or similar) coordinate system.
    /// </summary>
    public abstract class XYZPlotValue : XYPlotValue
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public XYZPlotValue()
        {
            ZLabel = "z";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pairs of minimum and maximum x, y coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public override double[] View
        {
            get { return new double[] { MinX, MaxX, MinY, MaxY, MinZ, MaxZ }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 6);

                MinX = elements[0];
                MaxX = elements[1];
                MinY = elements[2];
                MaxY = elements[3];
                MinZ = elements[4];
                MaxZ = elements[5];
            }
        }

        /// <summary>
        /// Gets or sets the label of the z axis.
        /// </summary>
        [StringToStringConverter]
        public string ZLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum z coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MinZ
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum z coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MaxZ
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum z coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public double[] ZRange
        {
            get { return new double[] { MinZ, MaxZ }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 2);
                MinZ = elements[0];
                MaxZ = elements[1];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the values MinX, MaxX, MinY, MaxY, MinZ and MaxZ.
        /// </summary>
        protected override void InitializeBoundaries()
        {
            base.InitializeBoundaries();
            MinZ = double.MaxValue;
            MaxZ = double.MinValue;
        }

        /// <summary>
        /// Sets the z-range (min and max) in one statement.
        /// </summary>
        /// <param name="min">The minimum for the z-axis.</param>
        /// <param name="max">The maximum for the z-axis.</param>
        public void SetZRange(double min, double max)
        {
            MinZ = min;
            MaxZ = max;
        }

        #endregion

        #region Serialization

        internal override void Serialize(Serializer s)
        {
            base.Serialize(s);
            s.Serialize(MinZ);
            s.Serialize(MaxZ);
            s.Serialize(ZLabel);
        }

        internal override void Deserialize(Deserializer ds)
        {
            base.Deserialize(ds);
            MinZ = ds.GetDouble();
            MaxZ = ds.GetDouble();
            ZLabel = ds.GetString();
        }

        #endregion
    }
}
