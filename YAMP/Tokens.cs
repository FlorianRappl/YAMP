/*
    Copyright (c) 2012, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace YAMP
{
    /// <summary>
    /// Provides internal access to the tokens and handles the token registration and variable assignment.
    /// </summary>
	class Tokens
	{
		#region Members

        IDictionary<string, Operator> operators;
        IDictionary<Regex, Expression> expressions;
        IDictionary<string, FunctionDelegate> functions;
        IDictionary<string, Value> constants;
        IDictionary<string, Value> variables;

        IDictionary<string, FunctionDelegate> o_functions;
        IDictionary<string, Value> o_constants;

		IDictionary<string, string> sanatizers;
        List<string> argumentFunctions;
        List<IFunction> methods;
		
		#endregion

        #region Static Fields

        static readonly NumberFormatInfo numFormat = new CultureInfo("en-us").NumberFormat;
        static int precision = 5;

        #endregion

        #region ctor

        Tokens ()
		{
            methods = new List<IFunction>();
            operators = new Dictionary<string, Operator>();
            expressions = new Dictionary<Regex, Expression>();
            functions = new Dictionary<string, FunctionDelegate>();
            constants = new Dictionary<string, Value>();
            o_functions = new Dictionary<string, FunctionDelegate>();
            o_constants = new Dictionary<string, Value>();
            variables = new Dictionary<string, Value>();
			sanatizers = new Dictionary<string, string>();
            argumentFunctions = new List<string>();
		}
		
		#endregion
		
		#region Properties

        /// <summary>
        /// Gets the assigned variables.
        /// </summary>
        public IDictionary<string, Value> Variables
		{
			get { return variables; }
		}

        /// <summary>
        /// Gets the available sanatizers.
        /// </summary>
		public IDictionary<string, string> Sanatizers
		{
			get { return sanatizers; }
		}

        /// <summary>
        /// Gets the methods provided by the parser.
        /// </summary>
        public List<IFunction> Methods
        {
            get { return methods; }
        }
		
		#endregion
		
		#region Register tokens
		
		void RegisterTokens()
		{
			var assembly = Assembly.GetExecutingAssembly();
            RegisterAssembly(assembly);

			sanatizers.Add("++", "+");
			sanatizers.Add("--", "+");
			sanatizers.Add("+-", "-");
			sanatizers.Add("-+", "-");
			sanatizers.Add("//", "*");
			sanatizers.Add("**", "*");
			sanatizers.Add("^*", "^");
			sanatizers.Add("*^", "^");
			sanatizers.Add("*/", "/");
			sanatizers.Add("/*", "/");
		}

        public void RegisterAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var ir = typeof(IRegisterToken).Name;
            var fu = typeof(IFunction).Name;
            var ct = typeof(IConstants).Name;

            foreach (var type in types)
            {
                if (type.IsAbstract)
                    continue;

                if (type.GetInterface(ir) != null)
                    (type.GetConstructor(Type.EmptyTypes).Invoke(null) as IRegisterToken).RegisterToken();

                if (type.GetInterface(fu) != null)
                {
                    var name = type.Name.RemoveFunctionConvention().ToLower();
                    var method = type.GetConstructor(Type.EmptyTypes).Invoke(null) as IFunction;
                    methods.Add(method);
                    AddFunction(name, method.Perform, false);

                    if (type.IsSubclassOf(typeof(ArgumentFunction)))
                        argumentFunctions.Add(name);
                }

                if (type.GetInterface(ct) != null)
                {
                    var props = type.GetProperties();
                    var mycst = type.GetConstructor(Type.EmptyTypes).Invoke(null);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType.IsSubclassOf(typeof(Value)))
                        {
                            AddConstant(prop.Name, prop.GetValue(mycst, null) as Value, false);
                        }
                    }
                }
            }
        }
		
		#endregion
		
		#region Add elements
		
		public void AddOperator(string pattern, Operator op)
		{
			operators.Add(pattern, op);
		}

        public void AddExpression(string pattern, Expression exp)
        {
            expressions.Add(new Regex("^" + pattern), exp);
        }
		
		public void AddConstant(string name, double constant, bool custom)
		{
            AddConstant(name, new ScalarValue(constant), custom);
		}

        public void AddConstant(string name, Value constant, bool custom)
        {
            var lname = name.ToLower();

            if (constants.ContainsKey(lname))
                constants[lname] = constant;
            else
                constants.Add(lname, constant);

            if (!custom)
                o_constants.Add(lname, constant);
        }
		
		public void AddFunction(string name, FunctionDelegate f, bool custom)
		{
			var lname = name.ToLower();
			
			if(functions.ContainsKey(lname))
				functions[lname] = f;
			else
				functions.Add(lname, f);
			
			if(!custom)
				o_functions.Add(lname, f);
		}
		
		#endregion
		
		#region Remove elements
		
		public void RemoveConstant(string name)
		{
			var lname = name.ToLower();
			
			if(o_constants.ContainsKey(lname))
				AddConstant(name, o_constants[lname], true);
			else if(constants.ContainsKey(lname))
				constants.Remove(lname);
		}
		
		public void RemoveFunction(string name)
		{
			var lname = name.ToLower();
			
			if(o_functions.ContainsKey(lname))
				AddFunction(name, o_functions[lname], true);
			else if(functions.ContainsKey(lname))
				functions.Remove(lname);
		}
		
		#endregion

        #region Variables

        public void AssignVariable(string name, Value value)
        {
            if (value != null)
            {
                if (variables.ContainsKey(name))
                    variables[name] = value;
                else
                    variables.Add(name, value);
            }
            else if (variables.ContainsKey(name))
                variables.Remove(name);
        }

        public Value GetVariable(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name] as Value;

            return null;
        }

        #endregion

        #region Find elements

        public Value FindConstants(string name)
		{
			var lname = name.ToLower();

			if(constants.ContainsKey(lname))
				return constants[lname];

            throw new SymbolException(name);
		}

        public FunctionDelegate FindFunction(string name)
        {
            var dummy = false;
            return FindFunction(name, out dummy);
        }
		
		public FunctionDelegate FindFunction(string name, out bool isList)
		{
			var lname = name.ToLower();

            if (functions.ContainsKey(lname))
            {
                isList = argumentFunctions.Contains(lname);
                return functions[lname];
            }
			
			throw new FunctionNotFoundException(name);
		}

		public Operator FindOperator(string input)
		{
			var maxop = string.Empty;
			var notfound = true;

			foreach(var op in operators.Keys)
			{
				if(op.Length <= maxop.Length)
					continue;

				notfound = false;

				for(var i = 0; i < op.Length; i++)
					if(notfound = (input[i] != op[i]))
						break;

				if(notfound == false)
					maxop = op;
			}

			if(maxop.Length == 0)
				throw new ParseException(input);

			return operators[maxop].Create();
		}
		
		public Expression FindExpression(string input)
		{
			Match m = null;

			foreach(var rx in expressions.Keys)
			{
				m = rx.Match(input);

				if(m.Success)
					return expressions[rx].Create(m);
			}
			
			throw new ExpressionNotFoundException(input);
		}
		
		#endregion
		
		#region Singleton
		
		static Tokens _instance;
		
		public static Tokens Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Tokens();
					_instance.RegisterTokens();
				}
				
				return _instance;
			}
		}
		
		public static IFormatProvider NumberFormat
		{
			get { return numFormat; }
		}

        public static int Precision
        {
            get { return precision; }
            internal set { precision = value; }
        }
		
		#endregion
		
		#region Misc
		
		public void Touch()
		{
			//Empty on intention
		}
		
		#endregion
	}
}