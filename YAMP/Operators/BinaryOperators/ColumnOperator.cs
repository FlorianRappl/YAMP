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

namespace YAMP
{
    /// <summary>
    /// The matrix column operator , - used to seperate various columns in the
    /// entered matrix.
    /// </summary>
	class ColumnOperator : BinaryOperator
    {
        #region ctor

        public ColumnOperator() : base(",", 1)
		{
		}

        public ColumnOperator(ParseEngine engine)
            : this()
        {
            StartColumn = engine.CurrentColumn;
            StartLine = engine.CurrentLine;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left, Value right)
		{
			if (!(left is NumericValue))
				throw new YAMPOperationInvalidException(",", left);

			if (!(right is NumericValue))
				throw new YAMPOperationInvalidException(",", right);

			return MatrixValue.Create(left).AddColumn(right);
		}

		public override void RegisterElement()
		{
            //Nothing to do here.
		}

        public override Operator Create()
        {
            return new ColumnOperator();
        }

        #endregion
    }
}
