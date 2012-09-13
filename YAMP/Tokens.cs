using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace YAMP
{
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
		
		#endregion

        #region Fields

        static readonly NumberFormatInfo numFormat = new CultureInfo("en-us").NumberFormat;

        #endregion

        #region ctor

        Tokens ()
		{
            operators = new Dictionary<string, Operator>();
            expressions = new Dictionary<Regex, Expression>();
            functions = new Dictionary<string, FunctionDelegate>();
            constants = new Dictionary<string, Value>();
            o_functions = new Dictionary<string, FunctionDelegate>();
            o_constants = new Dictionary<string, Value>();
            variables = new Dictionary<string, Value>();
		}
		
		#endregion
		
		#region Properties

        public IDictionary<string, Value> Variables
		{
			get { return variables; }
		}
		
		#endregion
		
		#region Register tokens
		
		void RegisterTokens()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var types = assembly.GetTypes();
			var ir = typeof(IRegisterToken).Name;
			var fu = typeof(IFunction).Name;
			var mycst = new Constants();
			var props = mycst.GetType().GetProperties();
			
			foreach(var type in types)
			{
                if (type.IsAbstract)
                    continue;

				if(type.GetInterface(ir) != null)
					(type.GetConstructor(Type.EmptyTypes).Invoke(null) as IRegisterToken).RegisterToken();
				
				if(type.GetInterface(fu) != null)
					AddFunction(type.Name.Replace("Function", string.Empty), (type.GetConstructor(Type.EmptyTypes).Invoke(null) as IFunction).Perform, false);
			}
			
			foreach(var prop in props)
				AddConstant(prop.Name, prop.GetValue(mycst, null) as Value, false);
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
			var lname = name.ToLower();

			if(functions.ContainsKey(lname))
				return functions[lname];
			
			throw new FunctionNotFoundException(name);
		}
		
		public Operator FindOperator(string input)
		{
            var i = 0;

			foreach(var op in operators.Keys)
			{
                if (input.StartsWith(op))
                    return operators[op].Create();
                
                if (input.Length < op.Length)
                    continue;

                for (i = 0; ; i++)
                {
                    if(i == op.Length)
                        return operators[op].Create();
                    else if (input[i] != op[i])
                        break;
                }
			}
			
			throw new ParseException(input);
		}
		
		public Expression FindExpression(string input)
		{
			foreach(var rx in expressions.Keys)
			{
				if(rx.IsMatch(input))
				{
					var exp = expressions[rx].Create();
					exp.SearchExpression = rx;
					return exp;
				}
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
		
		#endregion
		
		#region Misc
		
		public void Touch()
		{
			//Empty on intention
		}
		
		#endregion
	}
}