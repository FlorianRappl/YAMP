namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Holds information about a stored plugin.
    /// </summary>
    sealed class Plugin
    {
        #region Fields

        readonly Assembly _assembly;
        readonly Guid _id;
        readonly ParseContext _context;
        readonly List<IFunctionLoader> _loaders;
        readonly List<String> _constants;
        readonly List<String> _functions;
        readonly List<String> _valueTypes;

        #endregion

        #region Constants

        static readonly Type[] ParseContextType = new[] { typeof(ParseContext) };

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Plugin information holder.
        /// </summary>
        /// <param name="context">The assigned context.</param>
        /// <param name="name">The name of the plugin (fullname).</param>
        public Plugin(ParseContext context, Assembly assembly)
        {
            _assembly = assembly;
            _id = Guid.NewGuid();
            _context = context;
            _loaders = new List<IFunctionLoader>();
            _constants = new List<String>();
            _functions = new List<String>();
            _valueTypes = new List<String>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id of the plugin.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public String Name
        {
            get { return _assembly.FullName; }
        }

        /// <summary>
        /// Gets the context of the plugin.
        /// </summary>
        public ParseContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets the function loaders, if any.
        /// </summary>
        public List<IFunctionLoader> Loaders
        {
            get { return _loaders; }
        }

        /// <summary>
        /// Gets the names of the constants delivered by the plugin.
        /// </summary>
        public List<String> Constants
        {
            get { return _constants; }
        }

        /// <summary>
        /// Gets the names of the functions delivered by the plugin.
        /// </summary>
        public List<String> Functions
        {
            get { return _functions; }
        }

        /// <summary>
        /// Gets the names of the value types delivered by the plugin.
        /// </summary>
        public List<String> ValueTypes
        {
            get { return _valueTypes; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uninstalls the plugin.
        /// </summary>
        public void Uninstall()
        {
            foreach (var function in _functions)
            {
                _context.RemoveFunction(function);
            }

            foreach (var constant in _constants)
            {
                _context.RemoveConstant(constant);
            }

            foreach (var valueType in _valueTypes)
            {
                var trash = new List<String>();

                foreach (var variable in _context.Variables)
                {
                    if (variable.Value.Header == valueType)
                    {
                        trash.Add(variable.Key);
                    }
                }

                foreach (var entry in trash)
                {
                    _context.Variables.Remove(entry);
                }
            }
        }

        /// <summary>
        /// Installs the plugin.
        /// </summary>
        public void Install()
        {
            var types = _assembly.GetTypes();
            var elements = _context.Elements;

            foreach (var type in types)
            {
                if (!type.IsAbstract)
                {
                    if (type.Name.EndsWith("Value", StringComparison.Ordinal))
                    {
                        var value = Instantiate<IRegisterElement>(elements, type);
                        value.RegisterElement(elements);
                        _valueTypes.Add(type.Name.RemoveValueConvention());
                    }
                    else
                    {
                        var interfaces = type.GetInterfaces();
                        var element = Instantiate<IRegisterElement>(elements, type, interfaces);

                        if (element != null)
                        {
                            element.RegisterElement(elements);
                        }

                        var function = Instantiate<IFunction>(elements, type, interfaces);

                        if (function != null)
                        {
                            var name = type.Name.RemoveFunctionConvention().ToLower();
                            _functions.Add(name);
                            _context.AddFunction(name, function);
                        }

                        var constant = Instantiate<IConstants>(elements, type, interfaces);

                        if (constant != null)
                        {
                            var name = constant.Name;
                            _constants.Add(name);
                            _context.AddConstant(name, constant);
                        }

                        var loader = Instantiate<IFunctionLoader>(elements, type, interfaces);

                        if (loader != null)
                        {
                            _loaders.Add(loader);
                        }
                    }
                }
            }
        }

        T Instantiate<T>(Elements elements, Type type, Type[] interfaces)
        {
            var name = typeof(T).Name;

            if (interfaces.Any(iface => iface.Name.Equals(name)))
            {
                return Instantiate<T>(elements, type);
            }

            return default(T);
        }

        T Instantiate<T>(Elements elements, Type type)
        {
            var ctor = type.GetConstructor(Value.EmptyTypes) ?? type.GetConstructor(ParseContextType);

            if (ctor != null)
            {
                var args = ctor.GetParameters().Length == 1 ? new[] { _context } : null;
                return (T)ctor.Invoke(args);
            }

            return default(T);
        }

        #endregion
    }
}
