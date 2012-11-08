using System;
using System.Collections.Generic;

namespace YAMP
{
    public abstract class Transform : IRegisterToken
    {
        public Transform(char trigger)
        {
            Trigger = trigger;
        }

        public char Trigger { get; private set; }

        public abstract string Modify(QueryContext context, string original, Expression premise);

        public virtual void RegisterToken()
        {
            Tokens.Instance.AddTransform(Trigger, this);
        }

        public virtual bool WillTransform(Expression premise)
        {
            return true;
        }

        protected bool Fulfills(Type dependency, Expression premise)
        {
            if (dependency.IsInstanceOfType(premise))
                return true;

            return false;
        }
    }
}
