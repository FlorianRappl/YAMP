using System;

namespace YAMP
{
    /// <summary>
    /// Contains information about which variable changed its value into what.
    /// </summary>
    public class VariableEventArgs : EventArgs
    {
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
