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

namespace YAMP
{
	abstract class BracketExpression : TreeExpression
	{
		#region Members

		char _openBracket;
		char _closeBracket;

		#endregion

		#region ctor

		public BracketExpression(string pattern, char openBracket, char closeBracket) : base(pattern)
		{
			_openBracket = openBracket;
			_closeBracket = closeBracket;
		}

		#endregion

		#region Methods

		internal override string Input 
		{
			get
			{
				return _openBracket + _input + _closeBracket;
			}
		}

		public override string Set(string input)
		{
			var brackets = 1;

			for (var i = 1; i < input.Length; i++)
			{
				if (input[i] == _closeBracket)
					brackets--;
				else if (input[i] == _openBracket)
					brackets++;

				if (brackets == 0)
				{
					if (input[i] != _closeBracket)
						throw new BracketException(Offset + i, _closeBracket.ToString(), input);

					_input = input.Substring(1, i - 1);
					Tree = CreateParseTree(_input);
					return input.Substring(i + 1);
				}
			}

			throw new BracketException(Offset, _openBracket.ToString(), input);
		}

		protected virtual ParseTree CreateParseTree(string input)
		{
			return new ParseTree(Query, input, Offset);
		}

		#endregion
	}
}