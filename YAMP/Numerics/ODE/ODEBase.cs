using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Abstract base class for all ODE algorithms.
    /// </summary>
    public abstract class ODEBase
    {
        #region Members

        /// <summary>
        /// The function
        /// </summary>
        protected Func<double, double, double> f;

        /// <summary>
        /// Start point
        /// </summary>
        protected double begin;

        /// <summary>
        /// End point
        /// </summary>
        protected double end;

        /// <summary>
        /// Start condition
        /// </summary>
        protected double y0;

        /// <summary>
        /// Number of points
        /// </summary>
        protected int pointsNum;

        /// <summary>
        /// Delta
        /// </summary>
        protected double h;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the result.
        /// </summary>
        public double[,] Result { get; protected set; }

        /// <summary>
        /// Gets the set step size.
        /// </summary>
        public double Step
        {
            get { return h; }
        }

        #endregion

        #region ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="f">Function to be solved delegate</param>
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

        #endregion

        #region Methods

        /// <summary>
        /// Computes the result.
        /// </summary>
        protected abstract void Calculate();

        #endregion
    }
}
