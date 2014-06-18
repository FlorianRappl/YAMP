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
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Used for conversations from one unit to another.
    /// </summary>
    class ConversationUnit : PhysicalUnit
    {
        #region Members

        PhysicalUnit _from;

        #endregion

        #region ctor

        public ConversationUnit(string name, PhysicalUnit from)
        {
            Unit = name;
            _from = from;
        }

        #endregion

        #region Methods

        protected override PhysicalUnit Create()
        {
            return new ConversationUnit(Unit, _from);
        }

        public override bool HasConversation(string target)
        {
            if (_from.CanBe(target))
                return true;

            return _from.HasConversation(target);
        }

        public override Func<double, double> GetConversation(string unit)
        {
            //Example: Conversation from yd -> ft
            //Get transformation from yd -> m
            var backTransformation = _from.GetInverseConversation(Unit);

            //In case we want just yd to m
            if (_from.Unit == unit)
                return backTransformation;

            //Get transformation from m -> ft
            var newTransformation = _from.GetConversation(unit);
            //Apply toFt(toM(yd))
            return x => newTransformation(backTransformation(x));
        }

        public override Func<double, double> GetInverseConversation(string unit)
        {
            //Example: Conversation from 1/yd -> 1/ft
            //Get transformation from 1/yd -> 1/m
            var backTransformation = _from.GetConversation(Unit);

            //In case we want just 1/yd to 1/m
            if (_from.Unit == unit)
                return backTransformation;

            //Get transformation from 1/m -> 1/ft
            var newTransformation = _from.GetInverseConversation(unit);
            //Apply toInverseFt(toInverseM(yd))
            return x => newTransformation(backTransformation(x));
        }

        #endregion
    }
}
