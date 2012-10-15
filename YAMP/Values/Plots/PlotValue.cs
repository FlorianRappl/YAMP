using System;
using System.Collections.Generic;

namespace YAMP
{
    public abstract class PlotValue<T> : PlotValue
    {
        #region Members

        List<Points<T>> points;

        #endregion

        #region ctor

        public PlotValue ()
	    {
            points = new List<Points<T>>();
	    }

        #endregion

        #region Properties

        public int Count { get { return points.Count; } }

        public Points<T> this[int index]
        {
            get
            {
                return points[index];
            }
        }

        public T this[int index, int point]
        {
            get
            {
                return points[index][point];
            }
        }

        #endregion

        #region Methods

        public void AddValues(Points<T> values)
        {
            values.Color = ColorPalette[Count % ColorPalette.Length];
            points.Add(values);
            Changed("Count");
        }

        #endregion
    }

    public abstract class PlotValue : Value
    {
        #region Members

        string title;
        string xlabel;
        string ylabel;
        double minx;
        double maxx;
        double miny;
        double maxy;

        #endregion

        #region Events

        public event EventHandler<PropertyEventArgs> OnChanged;

        #endregion

        #region ctor

        public PlotValue()
        {
            Title = string.Empty;
            XLabel = "x";
            YLabel = "y";
        }

        #endregion

        #region Properties

        public string Title 
        {
            get { return title; }
            set
            {
                title = value;
                Changed("Title");
            }
        }

        public string XLabel 
        {
            get { return xlabel; }
            set
            {
                xlabel = value;
                Changed("XLabel");
            }
        }

        public string YLabel 
        {
            get { return ylabel; }
            set
            {
                ylabel = value;
                Changed("YLabel");
            }
        }

        public double MinX 
        {
            get { return minx; }
            set
            {
                minx = value;
                Changed("MinX");
            }
        }

        public double MaxX 
        {
            get { return maxx; }
            set
            {
                maxx = value;
                Changed("MaxX");
            }
        }

        public double MinY 
        {
            get { return miny; }
            set
            {
                miny = value;
                Changed("MinY");
            }
        }

        public double MaxY 
        {
            get { return maxy; }
            set
            {
                maxy = value;
                Changed("MaxY");
            }
        }

        public static string[] ColorPalette { get { return colorPalette; } }

        #endregion

        #region Methods

        protected void Changed(string property)
        {
            if (OnChanged != null)
            {
                var prop = GetType().GetProperty(property);
                var value = prop.GetValue(this, null);
                OnChanged(this, new PropertyEventArgs(property, value));
            }
        }

        public abstract void AddPoints(MatrixValue m);

        public void SetXRange(double min, double max)
        {
            MinX = min;
            MaxX = max;
        }

        public void SetYRange(double min, double max)
        {
            MinY = min;
            MaxY = max;
        }

        #endregion

        #region Nested Types

        public enum PointSymbol
        {
            Square,
            Circle,
            Diamond,
            Triangle,
            Dot,
            X,
            Star
        }

        public class Points<T> : List<T>
        {
            public Points()
            {
                Color = "black";
                Label = "Data";
                ShowLabel = false;
                LineWidth = 1f;
                Lines = false;
                Nodes = true;
                Symbol = PointSymbol.X;
            }

            public bool Nodes { get; set; }

            public bool Lines { get; set; }

            public float LineWidth { get; set; }

            public PointSymbol Symbol { get; set; }

            public bool ShowLabel { get; set; }

            public string Color { get; set; }

            public string Label { get; set; }
        }

        #endregion

        #region Statics

        readonly static string[] colorPalette = new string[]
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
            "forestgreen"
        };

        #endregion

        #region Operators

        public override Value Add(Value right)
        {
            throw new OperationNotSupportedException("+", this);
        }

        public override Value Subtract(Value right)
        {
            throw new OperationNotSupportedException("-", this);
        }

        public override Value Multiply(Value right)
        {
            throw new OperationNotSupportedException("*", this);
        }

        public override Value Divide(Value denominator)
        {
            throw new OperationNotSupportedException("/", this);
        }

        public override Value Power(Value exponent)
        {
            throw new OperationNotSupportedException("^", this);
        }

        public override byte[] Serialize()
        {
            return new byte[0];
        }

        public override Value Deserialize(byte[] content)
        {
            return Value.Empty;
        }

        #endregion
    }
}
