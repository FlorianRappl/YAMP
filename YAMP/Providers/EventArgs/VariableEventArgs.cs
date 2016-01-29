using System;

namespace YAMP
{
    /// <summary>
    /// Contains information about which variable changed its value into what.
    /// </summary>
    public class VariableEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public VariableEventArgs(string name, Value value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the name of the variable that has been changed.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the new value of the variable.
        /// </summary>
        public Value Value { get; private set; }
    }
}
