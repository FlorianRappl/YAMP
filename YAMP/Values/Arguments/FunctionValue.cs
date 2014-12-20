/*
    Copyright (c) 2012-2014, Florian Rappl.
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
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// A function value, i.e. a lambda expression or existing function
    /// wrapped as a value that can be used within YAMP.
    /// </summary>
    public sealed class FunctionValue : Value, IFunction
    {
        #region Members

        Func<ParseContext, Value, Value> perform;
        bool canSerialize;
        string[] arguments;
        string body;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new (dummy) instance of a FunctionValue.
        /// </summary>
        public FunctionValue()
        {
            canSerialize = false;
            //Dummy value - just identity.
            perform = (a, b) => b;
        }

        /// <summary>
        /// Creates a new instance of a FunctionValue with a delegate argument.
        /// </summary>
        /// <param name="f">Delegate to be wrapped in a function</param>
        /// <param name="standardFunctionBehaviour">indicates if the wrapper
        /// should include the StandardFunction behaviour, i.e. scalar execution
        /// for ScalarValues and matrix execution for MatrixValues </param>
        public FunctionValue(Func<ParseContext, Value, Value> f, bool standardFunctionBehaviour = false)
        {
            canSerialize = false;
            if (standardFunctionBehaviour)
            {
                perform = (context, argument) =>
                {
                    if (argument is ScalarValue)
                        return f(context, argument);
                    else if (argument is MatrixValue)
                    {
                        var A = argument as MatrixValue;
                        var M = new MatrixValue(A.DimensionY, A.DimensionX);

                        for (var j = 1; j <= A.DimensionY; j++)
                            for (var i = 1; i <= A.DimensionX; i++)
                                M[j, i] = f(context, A[j, i]) as ScalarValue;

                        return M;
                    }
                    else
                        throw new YAMPArgumentWrongTypeException(argument.GetType().ToString(), typeof(ScalarValue).Name + " or " + typeof(MatrixValue).Name, "dynamically created function");
                };
            }
            else
            {
                perform = f;
            }
        }

        /// <summary>
        /// Creates a new FunctionValue with data to parse.
        /// </summary>
        /// <param name="arguments">The list of argument identifiers.</param>
        /// <param name="body">The string representation of the body.</param>
        public FunctionValue(string[] arguments, string body)
        {
            this.arguments = arguments;
            this.body = body;
            canSerialize = true;

            perform = (context, argument) =>
            {
                var query = QueryContext.Dummy(context);
                query.Input = body;
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
        internal FunctionValue(string[] arguments, Expression body) 
        {
            this.arguments = arguments;
            this.body = body.ToCode();
            canSerialize = true;
            SetPerform(arguments, body);
        }

        /// <summary>
        /// Creates a new FunctionValue with a parsed named function (function).
        /// </summary> 
        /// <param name="name">The name of the function.</param>
        /// <param name="arguments">The list of argument identifiers.</param>
        /// <param name="body">The Expression representation of the body.</param>
        internal FunctionValue(string name, string[] arguments, Expression body)
        {
            this.arguments = arguments;
            canSerialize = false;
            SetPerform(name, arguments, body);
        }

        /// <summary>
        /// Creates a new FunctionValue with a given class that contains
        /// a perform method, i.e. implements the IFunction interface.
        /// </summary>
        /// <param name="function">The instance of the class implementing the IFunction interface.</param>
        public FunctionValue(IFunction function)
        {
            canSerialize = false;
            perform = function.Perform;
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

        void SetPerform(string[] arguments, Expression body)
        {
            perform = (context, argument) =>
            {
                var symbols = new Dictionary<string, Value>();
                var av = new ArgumentsValue().Append(argument);

                if (av.Length != arguments.Length)
                    throw new YAMPArgumentNumberException("Anonymous function", av.Length, arguments.Length);

                for (var i = 0; i < arguments.Length; i++)
                    symbols.Add(arguments[i], av.Values[i]);

                return body.Interpret(symbols);
            };
        }

        void SetPerform(string name, string[] arguments, Expression body)
        {
            perform = (context, argument) =>
            {
                var symbols = new Dictionary<string, Value>();
                var av = new ArgumentsValue().Append(argument);

                if (av.Length != arguments.Length)
                    throw new YAMPArgumentNumberException(name, av.Length, arguments.Length);

                for (var i = 0; i < arguments.Length; i++)
                    symbols.Add(arguments[i], av.Values[i].Copy());

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
            return perform(context, argument);
        }

        /// <summary>
        /// Returns a string representation of the function.
        /// </summary>
        /// <returns>The string representing the instance.</returns>
		public override string ToString()
		{
			if (arguments == null || string.IsNullOrEmpty(body))
				return "λ reference";

			return string.Format("({0}) => {1}", string.Join(", ", arguments), body);
		}

        #endregion

        #region Serialization

        /// <summary>
        /// Tries to convert the given instance into bytes.
        /// </summary>
        /// <returns>The binary content.</returns>
        public override byte[] Serialize()
        {
            if (!canSerialize)
                return new byte[0];

            using (var s = Serializer.Create())
            {
                s.Serialize(arguments.Length);

                foreach (var arg in arguments)
                    s.Serialize(arg);

                s.Serialize(body);
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
                arguments = new string[args];

                for (int i = 0; i != args; i++)
                    arguments[i] = ds.GetString();

                body = ds.GetString();
            }

            return new FunctionValue(arguments, body);
        }

        #endregion
    }
}
