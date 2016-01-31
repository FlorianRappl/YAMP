namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Useful extensions for the Parser class.
    /// </summary>
    public static class ParserExtensions
    {
        #region Evaluation

        /// <summary>
        /// Execute the evaluation of this parser instance without any external symbols.
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="input">The input to evaluate.</param>
        /// <returns>The value from the evaluation.</returns>
        public static Value Evaluate(this Parser parser, String input)
        {
            return parser.Evaluate(input, new Dictionary<String, Value>());
        }

        /// <summary>
        /// Execute the evaluation of this parser instance with external symbols.
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="input">The input to evaluate.</param>
        /// <param name="values">
        /// The values in an anonymous object - containing name - value pairs.
        /// </param>
        /// <returns>The value from the evaluation.</returns>
        public static Value Evaluate(this Parser parser, String input, Object values)
        {
            var symbols = values.ToDictionary();
            return parser.Evaluate(input, symbols);
        }

        #endregion

        #region Extension

        /// <summary>
        /// Adds a custom constant to the parser (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the constant.
        /// </param>
        /// <param name="constant">
        /// The value of the constant.
        /// </param>
        public static void AddCustomConstant(this Parser parser, String name, Double constant)
        {
            parser.Context.AddConstant(name, new ContainerConstant(name, new ScalarValue(constant)));
        }

        /// <summary>
        /// Adds a custom constant to the parser (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the constant.
        /// </param>
        /// <param name="constant">
        /// The value of the constant.
        /// </param>
        public static void AddCustomConstant(this Parser parser, String name, Value constant)
        {
            parser.Context.AddConstant(name, new ContainerConstant(name, constant));
        }

        /// <summary>
        /// Removes a custom constant (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the constant that should be removed.
        /// </param>
        public static void RemoveCustomConstant(this Parser parser, String name)
        {
            parser.Context.RemoveConstant(name);
        }

        /// <summary>
        /// Renames an existing constant (custom or defined).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="oldName">The old name of the constant.</param>
        /// <param name="newName">The new name for the constant.</param>
        public static void RenameConstant(this Parser parser, String oldName, String newName)
        {
            parser.Context.RenameConstant(oldName, newName);
        }

        /// <summary>
        /// Renames an existing function (custom or defined).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="oldName">The old name of the function.</param>
        /// <param name="newName">The new name for the function.</param>
        public static void RenameFunction(this Parser parser, String oldName, String newName)
        {
            parser.Context.RenameFunction(oldName, newName);
        }

        /// <summary>
        /// Adds a custom function to be used by the parser (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the function that should be added.
        /// </param>
        /// <param name="f">
        /// The function that fulfills the signature Value f(Value v).
        /// </param>
        public static void AddCustomFunction(this Parser parser, String name, FunctionDelegate f)
        {
            parser.Context.AddFunction(name, new ContainerFunction(name, f));
        }

        /// <summary>
        /// Removes a custom function (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the function that should be removed.
        /// </param>
        public static void RemoveCustomFunction(this Parser parser, String name)
        {
            parser.Context.RemoveFunction(name);
        }

        /// <summary>
        /// Adds a variable to be used by the parser (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the variable that should be added.
        /// </param>
        /// <param name="value">
        /// The value of the variable.
        /// </param>
        public static void AddVariable(this Parser parser, String name, Value value)
        {
            parser.Context.Variables.Add(name, value);
        }

        /// <summary>
        /// Removes a variable from the workspace (to the primary context).
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="name">
        /// The name of the symbol corresponding to the variable that should be removed.
        /// </param>
        public static void RemoveVariable(this Parser parser, String name)
        {
            parser.Context.Variables.Remove(name);
        }

        /// <summary>
        /// Loads an external library (assembly) that uses IFunction, Operator, ..., into the primary context.
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="assembly">
        /// The assembly to load as a plugin.
        /// </param>
        /// <returns>The ID for the plugin.</returns>
        public static Guid LoadPlugin(this Parser parser, Assembly assembly)
        {
            return parser.Context.Elements.RegisterAssembly(parser.Context, assembly);
        }

        /// <summary>
        /// Unloads a previously loaded plugin.
        /// </summary>
        /// <param name="parser">The parser to extend.</param>
        /// <param name="pluginId">The ID for the assembly to unload.</param>
        /// <returns>The primary parse context.</returns>
        public static void UnloadPlugin(this Parser parser, Guid pluginId)
        {
            parser.Context.Elements.RemoveAssembly(pluginId);
        }

        #endregion
    }
}
