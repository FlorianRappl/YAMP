using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class ODEBase
    {
        protected Func<double, double, double> f;
        protected double begin;
        protected double end;
        protected double y0;
        protected int pointsNum;
        protected double h;

        public double[,] Result { get; protected set; }

        public double Step
        {
            get { return h; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="function">Function to be solved delegate</param>
        /// <param name="begin">Interval start point value</param>
        /// <param name="end">Interval end point value</param>
        /// <param name="y0">Starting condition</param>
        /// <param name="pointsNum">Points number</param>
        public ODEBase(Func<double, double, double> f, double begin, double end, double y0, int pointsNum)
        {
            this.f = f;
            this.begin = begin;
            this.end = end;
            this.y0 = y0;
            this.pointsNum = pointsNum;
            h = (end - begin) / pointsNum;
            Calculate();
        }

        protected abstract void Calculate();
    }
}
