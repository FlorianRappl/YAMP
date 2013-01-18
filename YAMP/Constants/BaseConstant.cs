using System;

namespace YAMP
{
    /// <summary>
    /// This class gives an abstract base class for your own implementations
    /// of classes that represent constants.
    /// </summary>
	public abstract class BaseConstant : IConstants
    {
        #region Members

        string name;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance with the name given by convention.
        /// </summary>
		public BaseConstant()
		{
			name = GetType().Name.Replace("Constant", string.Empty);
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The name for the constant.</param>
        public BaseConstant(string name)
        {
            this.name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the constant (by convention).
        /// Convention: The name of the class without the word "Constant".
        /// </summary>
		public string Name
		{
			get { return name; }
		}

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
		public abstract Value Value
		{
			get;
        }

        #endregion
    }
}
