namespace YAMP
{
    using System;

    /// <summary>
    /// Represents information of a variable.
    /// </summary>
    public class VariableInfo
    {
        private readonly String _name;
        private readonly Boolean _assigned;
        private readonly ParseContext _context;

        /// <summary>
        /// Creates a new variable info.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="assigned">Is the variable assigned?</param>
        /// <param name="context">The context of the variable.</param>
        public VariableInfo(String name, Boolean assigned, ParseContext context)
        {
            _name = name;
            _assigned = assigned;
            _context = context;
        }

        /// <summary>
        /// Gets if the variable is assigned.
        /// </summary>
        public Boolean IsAssigned
        {
            get { return _assigned; }
        }

        /// <summary>
        /// Gets the context of the variable.
        /// </summary>
        public ParseContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }
    }
}
