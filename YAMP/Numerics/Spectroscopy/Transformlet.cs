using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class Transformlet
    {
        #region ctor

        // R is the radix, N the total length, and u contains the Nth complex roots of unity

        public Transformlet(int R, int N, ScalarValue[] u)
        {
            this.R = R;
            this.N = N;
            this.u = u;
            this.dx = N / R;
        }

        #endregion

        #region Members

        protected int R;
        protected int N;
        protected ScalarValue[] u;
        int dx;

        #endregion

        #region Properties

        public int Radix
        {
            get { return R; }
        }

        public int Multiplicity 
        { 
            get; 
            internal set; 
        }

        #endregion

        #region Methods

        public virtual void FftPass(ScalarValue[] x, ScalarValue[] y, int Ns, int sign)
        {
            var v = new ScalarValue[R];
            int dx = N / R;

            for (int j = 0; j < dx; j++)
            {
                int xi = j;
                int ui = 0; 
                
                if (sign < 0) 
                    ui = N;

                int du = (dx / Ns) * (j % Ns); 
                
                if (sign < 0)
                    du = -du;

                v[0] = x[xi];

                for (int r = 1; r < R; r++)
                {
                    xi += dx;
                    ui += du;
                    v[r] = x[xi] * u[ui];
                }

                int y0 = Expand(j, Ns, R);
                FftKernel(v, y, y0, Ns, sign);
            }
        }

        public virtual void FftKernel(ScalarValue[] x, ScalarValue[] y, int y0, int dy, int sign)
        {
            int yi = y0;
            y[yi] = ScalarValue.Zero;

            for (int j = 0; j < R; j++)
                y[yi] += x[j];

            // now do the higher index entries
            for (int i = 1; i < R; i++)
            {
                yi += dy;
                y[yi] = x[0];
                int ui = 0; if (sign < 0) ui = N;
                int du = dx * i; if (sign < 0) du = -du;

                for (int j = 1; j < R; j++)
                {
                    ui += du;

                    if (ui >= N) 
                        ui -= N; 
                    
                    if (ui < 0) 
                        ui += N;

                    y[yi] += x[j] * u[ui];
                }
            }
        }

        protected static int Expand(int idxL, int N1, int N2)
        {
            return ((idxL / N1) * N1 * N2 + (idxL % N1));
        }

        #endregion
    }
}
