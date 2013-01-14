/*
    Copyright (c) 2012-2013, Florian Rappl.
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
        /// Creates a new FunctionValue with a parsed object.
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

        void SetPerform(string[] arguments, Expression body)
        {
            perform = (context, argument) =>
            {
                var av = new ArgumentsValue();
                var symbols = new Dictionary<string, Value>();

                if (argument is ArgumentsValue)
                    av = (ArgumentsValue)argument;
                else
                    av.Insert(argument);

                if (av.Length != arguments.Length)
                    throw new YAMPArgumentNumberException("Anonymous function", av.Length, arguments.Length);

                for (var i = 0; i < arguments.Length; i++)
                    symbols.Add(arguments[i], av.Values[i]);

                return body.Interpret(symbols);
            };
        }

        public Value Perform(ParseContext context, Value argument)
        {
            return perform(context, argument);
        }

		public override string ToString()
		{
			if (arguments == null || string.IsNullOrEmpty(body))
				return "λ reference";

			return string.Format("({0}) => {1}", string.Join(", ", arguments), body);
		}

        #endregion

        #region Serialization

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
