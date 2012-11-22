using System;

namespace YAMP
{
    public interface ISetFunction
    {
        Value Perform(ParseContext context, Value indices, Value values);
    }
}
