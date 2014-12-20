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
using System.ComponentModel;

namespace YAMP
{
    /// <summary>
    /// Class to enter examples for usage with help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        /// <param name="description">The description to store.</param>
        /// <param name="file">The status if the file system is manipulated.</param>
        public ExampleAttribute(string example, string description, bool file)
        {
            Example = example;
            Description = description;
            IsFile = file;
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        /// <param name="description">The description to store.</param>
        public ExampleAttribute(string example, string description)
            : this(example, description, false)
        {
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        public ExampleAttribute(string example) : this(example, string.Empty, false)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the example performs an operation on the file system.
        /// </summary>
        public bool IsFile { get; private set; }

        /// <summary>
        /// Gets the example.
        /// </summary>
        public string Example { get; private set; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public string Description { get; private set; }

        #endregion
    }
}
