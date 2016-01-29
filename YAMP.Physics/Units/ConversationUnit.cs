using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Used for conversations from one unit to another.
    /// </summary>
    class ConversationUnit : PhysicalUnit
    {
        #region Fields

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
