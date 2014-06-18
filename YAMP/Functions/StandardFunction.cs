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

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all standard functions.
    /// </summary>
    public abstract class StandardFunction : BaseFunction
    {
        /// <summary>
        /// Performs the function - maps each entry of a matrix to a matrix.
        /// </summary>
        /// <param name="argument">Either a Scalar or a Matrix.</param>
        /// <returns>The scalar or matrix.</returns>
        public override Value Perform(Value argument)
        {
            if (argument is ScalarValue)
                return GetValue(argument as ScalarValue);
            else if (argument is MatrixValue)
            {
                var A = argument as MatrixValue;
                var M = new MatrixValue(A.DimensionY, A.DimensionX);

                for (var j = 1; j <= A.DimensionY; j++)
                    for (var i = 1; i <= A.DimensionX; i++)
                        M[j, i] = GetValue(A[j, i]);

                return M;
            }
            else if (argument is ArgumentsValue)
                throw new YAMPArgumentNumberException(Name, (argument as ArgumentsValue).Length, 1);

            throw new YAMPOperationInvalidException(Name, argument);
        }

        /// <summary>
        /// Gets a single value.
        /// </summary>
        /// <param name="value">The argument (single value).</param>
        /// <returns>The result (single value).</returns>
        protected virtual ScalarValue GetValue(ScalarValue value)
        {
            return value;
		}

        #region Documentation Helpers

        /// <summary>
        /// Documentation helper - overload ONLY to do some documention.
        /// </summary>
        /// <param name="x">Scalar</param>
        /// <returns>Scalar</returns>
		[Description("Computes the value and returns the result.")]
		public virtual ScalarValue Function(ScalarValue x)
		{
			return x;
		}

        /// <summary>
        /// Documentation helper - overload ONLY to do some documention.
        /// </summary>
        /// <param name="x">Matrix</param>
        /// <returns>Matrix</returns>
		[Description("Computes the value of each entry of the given matrix and returns a matrix with the same dimension.")]
		public virtual MatrixValue Function(MatrixValue x)
		{
			return x;
        }

        #endregion
    }
}
