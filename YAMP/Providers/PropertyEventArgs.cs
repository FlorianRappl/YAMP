using System;

namespace YAMP
{
    /// <summary>
    /// Contains information about which property changed its value into what.
    /// </summary>
    public class PropertyEventArgs : EventArgs
    {
        public PropertyEventArgs(string propertyName, object value)
        {
            Name = propertyName;
            Value = value;
        }

        /// <summary>
        /// Gets the name of the property that has been changed.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the new value of the property.
        /// </summary>
        public object Value { get; private set; }
    }
}
