using System;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    abstract class AssignmentPrefixOperator : AssignmentOperator
    {        
        BinaryOperator _child;

        public AssignmentPrefixOperator(ParseContext context, BinaryOperator child) : base(context, child.Op)
        {
            _child = child;
        }

        public override Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
        {
            var bottom = _child.Handle(left, right, symbols);
            return Assign(left, bottom, symbols);
        }
    }
}
