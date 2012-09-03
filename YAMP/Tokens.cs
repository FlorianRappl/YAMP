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
		
		Hashtable operators;
		Hashtable expressions;
		Hashtable functions;
		Hashtable constants;
		Hashtable o_functions;
		Hashtable o_constants;
        Hashtable variables;
		
		#endregion

        #region Fields

        static readonly NumberFormatInfo numFormat = new CultureInfo("en-us").NumberFormat;

        #endregion

        #region ctor

        Tokens ()
		{
			operators = new Hashtable();
			expressions = new Hashtable();
			functions = new Hashtable();
			constants = new Hashtable();
			o_functions = new Hashtable();
			o_constants = new Hashtable();
            variables = new Hashtable();
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
				AddConstant(prop.Name, (double)prop.GetValue(mycst, null), false);
		}
		
		#endregion
		
		#region Add elements
		
		public void AddOperator(string pattern, Type type)
		{
			operators.Add(pattern, type.GetConstructor(Type.EmptyTypes));
		}

        public void AddExpression(string pattern, Type type)
        {
            expressions.Add(new Regex("^" + pattern), type.GetConstructor(Type.EmptyTypes));
        }
		
		public void AddConstant(string name, double constant, bool custom)
		{
			var lname = name.ToLower();
			
			if(constants.ContainsKey(lname))
				constants[lname] = constant;
			else
				constants.Add(lname, constant);
			
			if(!custom)
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
				AddConstant(name, (double)o_constants[lname], true);
			else if(constants.ContainsKey(lname))
				constants.Remove(lname);
		}
		
		public void RemoveFunction(string name)
		{
			var lname = name.ToLower();
			
			if(o_functions.ContainsKey(lname))
				AddFunction(name, o_functions[lname] as FunctionDelegate, true);
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
				return new ScalarValue((double)constants[lname]);
			
			if(name.Equals("i"))
				return new ScalarValue(0.0, 1.0);

            throw new SymbolException(name);
		}
		
		public FunctionDelegate FindFunction(string name)
		{
			var lname = name.ToLower();

			if(functions.ContainsKey(lname))
				return functions[lname] as FunctionDelegate;
			
			throw new FunctionNotFoundException(name);
		}
		
		public Operator FindOperator(string input)
		{
            var i = 0;

			foreach(string op in operators.Keys)
			{
                if (input.Length < op.Length)
                    continue;

                for (i = 0; ; i++)
                {
                    if(i == op.Length)
                        return (operators[op] as ConstructorInfo).Invoke(null) as Operator;
                    else if (input[i] != op[i])
                        break;
                }					
			}
			
			throw new ParseException(input);
		}
		
		public AbstractExpression FindExpression(string input)
		{
			foreach(Regex rx in expressions.Keys)
			{
				if(rx.IsMatch(input))
					return (expressions[rx] as ConstructorInfo).Invoke(null) as AbstractExpression;
			}
			
			return new ZeroExpression();
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
	}
}