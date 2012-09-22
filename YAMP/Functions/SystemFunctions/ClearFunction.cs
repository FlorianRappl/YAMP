using System;

namespace YAMP
{
    class ClearFunction : ArgumentFunction
    {
        public ClearFunction() : base(1)
        {
        }

        public Value Function()
        {
            var count = Tokens.Instance.Variables.Count;
            Tokens.Instance.Variables.Clear();
            return new StringValue(count + " objects cleared.");
        }

        public Value Function(ArgumentsValue args)
        {
            var count = 0;

            foreach (var arg in args.Values)
            {
                if (arg is StringValue)
                {
                    var name = (arg as StringValue).Value;

                    if (Tokens.Instance.Variables.ContainsKey(name))
                    {
                        Tokens.Instance.Variables.Remove(name);
                        count++;
                    }
                }
            }

            return new StringValue(count + " objects cleared.");
        }
    }
}
