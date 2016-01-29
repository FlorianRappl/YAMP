using System;
using System.Linq;
using System.Reflection;

namespace YAMP.Converter
{
    /// <summary>
    /// Abstract base class for any value converter.
    /// </summary>
	public abstract class ValueConverterAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="expected">The expected type (target).</param>
        /// <param name="converter">The conversion function.</param>
		public ValueConverterAttribute(Type expected, Func<Value, object> converter)
		{
			Converter = converter;
			Expected = expected;
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="expected">The expected type (target)</param>
		public ValueConverterAttribute(Type expected)
		{
			Expected = expected;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the converter to use.
        /// </summary>
        public Func<Value, object> Converter
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the expected type.
        /// </summary>
        public Type Expected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the expected type (without the value convention).
        /// </summary>
        public string Type
        {
            get { return Expected.Name.RemoveValueConvention(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the given value to a standard CLR type.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <returns>The standard CLR type.</returns>
        public object Convert(Value argument)
		{
			return Converter.Invoke(argument);
		}

        /// <summary>
        /// Indicates if a given argument can be converted.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <returns>A boolean if this is possible.</returns>
		public bool CanConvertFrom(Value argument)
		{
			return Expected.IsInstanceOfType(argument);
        }

        #endregion
    }
}
