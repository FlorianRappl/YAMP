namespace YAMP
{
    using YAMP.Exceptions;

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
            {
                return GetValue((ScalarValue)argument);
            }
            else if (argument is MatrixValue)
            {
                var A = (MatrixValue)argument;
                var M = new MatrixValue(A.DimensionY, A.DimensionX);

                for (var j = 1; j <= A.DimensionY; j++)
                {
                    for (var i = 1; i <= A.DimensionX; i++)
                    {
                        M[j, i] = GetValue(A[j, i]);
                    }
                }

                return M;
            }
            else if (argument is ArgumentsValue)
            {
                throw new YAMPArgumentNumberException(Name, ((ArgumentsValue)argument).Length, 1);
            }

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
		[Description("StandardFunctionDescriptionForScalar")]
		public virtual ScalarValue Function(ScalarValue x)
		{
			return x;
		}

        /// <summary>
        /// Documentation helper - overload ONLY to do some documention.
        /// </summary>
        /// <param name="x">Matrix</param>
        /// <returns>Matrix</returns>
		[Description("StandardFunctionDescriptionForMatrix")]
		public virtual MatrixValue Function(MatrixValue x)
		{
			return x;
        }

        #endregion
    }
}
