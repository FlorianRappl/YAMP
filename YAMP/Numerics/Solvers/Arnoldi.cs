using System;
using YAMP;

namespace YAMP.Numerics
{
    public class Arnoldi
    {
        MatrixValue A;
        MatrixValue H;
        MatrixValue v;
        int j;
        
        /// <summary>
        /// Creates the instance of an Arnoldi method for orthogonalization of the Krylov subspace.
        /// </summary>
        /// <param name="A">
        /// A <see cref="mat"/> reference to the A matrix, which is to the matrix to solve A * x = b.
        /// </param>
        /// <param name="H">
        /// A <see cref="mat"/> reference to the H matrix.
        /// </param>
        /// <param name="v">
        /// A <see cref="mat"/> reference to the collection of v vectors.
        /// </param>
        public Arnoldi(MatrixValue A, MatrixValue H, MatrixValue v)
        {
            this.A = A;
            this.H = H;
            this.v = v;
            this.j = 0;
        }

        public bool Next()
        {
            j++;
            var right = A * v.GetColumnVector(j);
            var sum = new MatrixValue(right.Length, 1);

            for (int i = 1; i <= j; i++)
            {
                var w = v.GetColumnVector(i);
                H[i, j] = right.Adjungate().Dot(w);
                sum = sum + H[i, j] * w;
            }

            var wj = right - sum;
            H[j + 1, j] = wj.Abs();

            if (H[j + 1, j].Abs() == 0.0)
                return true;

            var y = wj / H[j + 1, j];
            v.SetColumnVector(j + 1, y);
            return false;
        }
    }
}
