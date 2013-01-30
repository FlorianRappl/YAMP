using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// Holds information about a stored plugin.
    /// </summary>
    class Plugin
    {
        #region Members

        static int _nextPluginIdentifier = 1000;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Plugin information holder.
        /// </summary>
        /// <param name="context">The assigned context.</param>
        /// <param name="name">The name of the plugin (fullname).</param>
        public Plugin(ParseContext context, string name)
        {
            Id = _nextPluginIdentifier++;
            Name = name;
            Context = context;

            Constants = new List<string>();
            Functions = new List<string>();
            ValueTypes = new List<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id of the plugin.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the context of the plugin.
        /// </summary>
        public ParseContext Context { get; private set; }

        /// <summary>
        /// Gets the names of the constants delivered by the plugin.
        /// </summary>
        public List<string> Constants { get; private set; }

        /// <summary>
        /// Gets the names of the functions delivered by the plugin.
        /// </summary>
        public List<string> Functions { get; private set; }

        /// <summary>
        /// Gets the names of the value types delivered by the plugin.
        /// </summary>
        public List<string> ValueTypes { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Uninstalls the plugin.
        /// </summary>
        public void Uninstall()
        {
            var context = Context;

            foreach (var function in Functions)
                context.RemoveFunction(function);

            foreach (var constant in Constants)
                context.RemoveConstant(constant);

            foreach (var valueType in ValueTypes)
            {
                var trash = new List<string>();

                foreach (var variable in context.Variables)
                    if (variable.Value.Header == valueType)
                        trash.Add(variable.Key);

                foreach (var entry in trash)
                    context.Variables.Remove(entry);
            }
        }

        #endregion
    }
}
