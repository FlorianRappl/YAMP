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

namespace YAMP
{
    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public sealed partial class ParseContext
    {
        #region Comfort Methods

        /// <summary>
        /// Loads the workspace from the given file.
        /// </summary>
        /// <param name="fromFileName">The path to the file.</param>
        public void Load(string fromFileName)
        {
            var lf = new LoadFunction();
            lf.Context = this;
            lf.Function(new StringValue(fromFileName));
        }

        /// <summary>
        /// Saves the workspace in the given file.
        /// </summary>
        /// <param name="toFileName">The path to the file.</param>
        public void Save(string toFileName)
        {
            SaveFunction.Save(toFileName, variables);
        }

        #endregion
    }
}
