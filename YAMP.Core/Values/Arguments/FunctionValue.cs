namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// A function value, i.e. a lambda expression or existing function
    /// wrapped as a value that can be used within YAMP.
    /// </summary>
    public sealed class FunctionValue : Value, IFunction
    {
        #region Fields

        Func<ParseContext, Value, Value> _perform;
        Boolean _canSerialize;
        String[] _arguments;
        String _body;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new (dummy) instance of a FunctionValue.
        /// </summary>
        public FunctionValue()
        {
            _canSerialize = false;
            //Dummy value - just identity.
            _perform = (a, b) => b;
        }

        /// <summary>
        /// Creates a new instance of a FunctionValue with a delegate argument.
        /// </summary>
        /// <param name="f">Delegate to be wrapped in a function</param>
        /// <param name="standardFunctionBehaviour">indicates if the wrapper
        /// should include the StandardFunction behaviour, i.e. scalar execution
        /// for ScalarValues and matrix execution for MatrixValues </param>
        public FunctionValue(Func<ParseContext, Value, Value> f, Boolean standardFunctionBehaviour = false)
        {
            _canSerialize = false;

            if (standardFunctionBehaviour)
            {
                _perform = (context, argument) =>
                {
                    if (argument is ScalarValue)
                    {
                        return f(context, argument);
                    }
                    else if (argument is MatrixValue)
                    {
                        var A = argument as MatrixValue;
                        var M = new MatrixValue(A.DimensionY, A.DimensionX);

                        for (var j = 1; j <= A.DimensionY; j++)
                        {
                            for (var i = 1; i <= A.DimensionX; i++)
                            {
                                M[j, i] = f(context, A[j, i]) as ScalarValue;
                            }
                        }

                        return M;
                    }
                    else
                    {
                        throw new YAMPArgumentWrongTypeException(argument.GetType().ToString(), typeof(ScalarValue).Name + " or " + typeof(MatrixValue).Name, "dynamically created function");
                    }
                };
            }
            else
            {
                _perform = f;
            }
        }

        /// <summary>
        /// Creates a new FunctionValue with data to parse.
        /// </summary>
        /// <param name="arguments">The list of argument identifiers.</param>
        /// <param name="body">The string representation of the body.</param>
        public FunctionValue(String[] arguments, String body)
        {
            _arguments = arguments;
            _body = body;
            _canSerialize = true;

            _perform = (context, argument) =>
            {
                var query = new QueryContext(context, body);
                var expression = query.Parser.ParseStatement().Container;
                SetPerform(arguments, expression);
                return Perform(context, argument);
            };
        }

        /// <summary>
        /// Creates a new FunctionValue with a parsed object (lambda expression).
        /// </summary>
        /// <param name="arguments">The list of argument identifiers.</param>
        /// <param name="body">The Expression representation of the body.</param>
        internal FunctionValue(String[] arguments, Expression body) 
        {
            _arguments = arguments;
            _body = body.ToCode();
            _canSerialize = true;
            SetPerform(arguments, body);
        }

        /// <summary>
        /// Creates a new FunctionValue with a parsed named function (function).
        /// </summary> 
        /// <param name="name">The name of the function.</param>
        /// <param name="arguments">The list of argument identifiers.</param>
        /// <param name="body">The Expression representation of the body.</param>
        internal FunctionValue(String name, String[] arguments, Expression body)
        {
            _arguments = arguments;
            _canSerialize = false;
            SetPerform(name, arguments, body);
        }

        /// <summary>
        /// Creates a new FunctionValue with a given class that contains
        /// a perform method, i.e. implements the IFunction interface.
        /// </summary>
        /// <param name="function">The instance of the class implementing the IFunction interface.</param>
        public FunctionValue(IFunction function)
        {
            _canSerialize = false;
            _perform = function.Perform;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Does not clone the function, but just returns the same function
        /// again.
        /// </summary>
        /// <returns>The original function.</returns>
        public override Value Copy()
        {
            return this;
        }

        /// <summary>
        /// Tries to cast the function to a member function.
        /// </summary>
        /// <returns>The member function or null.</returns>
        public MemberFunction AsMemberFunction()
        {
            if (_perform != null && _perform.Target is MemberFunction)
            {
                return _perform.Target as MemberFunction;
            }

            return null;
        }

        void SetPerform(String[] arguments, Expression body)
        {
            _perform = (context, argument) =>
            {
                var symbols = new Dictionary<String, Value>();
                var av = new ArgumentsValue().Append(argument);

                if (av.Length != arguments.Length)
                {
                    throw new YAMPArgumentNumberException("Anonymous function", av.Length, arguments.Length);
                }

                for (var i = 0; i < arguments.Length; i++)
                {
                    symbols.Add(arguments[i], av.Values[i]);
                }

                return body.Interpret(symbols);
            };
        }

        void SetPerform(String name, String[] arguments, Expression body)
        {
            _perform = (context, argument) =>
            {
                var symbols = new Dictionary<String, Value>();
                var av = new ArgumentsValue().Append(argument);

                if (av.Length != arguments.Length)
                {
                    throw new YAMPArgumentNumberException(name, av.Length, arguments.Length);
                }

                for (var i = 0; i < arguments.Length; i++)
                {
                    symbols.Add(arguments[i], av.Values[i].Copy());
                }

                return body.Interpret(symbols);
            };
        }

        /// <summary>
        /// Invokes the given function value.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <param name="argument">The argument(s) to use for the invocation.</param>
        /// <returns>The evaluated value.</returns>
        public Value Perform(ParseContext context, Value argument)
        {
            return _perform(context, argument);
        }

        /// <summary>
        /// Returns a string representation of the function.
        /// </summary>
        /// <returns>The string representing the instance.</returns>
		public override String ToString()
		{
			if (_arguments == null || String.IsNullOrEmpty(_body))
				return "λ reference";

			return String.Format("({0}) => {1}", String.Join(", ", _arguments), _body);
		}

        #endregion

        #region Serialization

        /// <summary>
        /// Tries to convert the given instance into bytes.
        /// </summary>
        /// <returns>The binary content.</returns>
        public override byte[] Serialize()
        {
            if (!_canSerialize)
                return new byte[0];

            using (var s = Serializer.Create())
            {
                s.Serialize(_arguments.Length);

                foreach (var arg in _arguments)
                    s.Serialize(arg);

                s.Serialize(_body);
                return s.Value;
            }
        }

        /// <summary>
        /// Tries to create a new instance from the given bytes.
        /// </summary>
        /// <param name="content">The binary content.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            if (content.Length == 0)
                return this;

            using (var ds = Deserializer.Create(content))
            {
                var args = ds.GetInt();
                _arguments = new string[args];

                for (int i = 0; i != args; i++)
                    _arguments[i] = ds.GetString();

                _body = ds.GetString();
            }

            return new FunctionValue(_arguments, _body);
        }

        #endregion
    }
}
