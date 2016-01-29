using System;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This class represents the abstract basis for all combined
    /// assignment operators (binary operator and =).
    /// </summary>
    abstract class AssignmentPrefixOperator : AssignmentOperator
    {        
        BinaryOperator _child;

        public AssignmentPrefixOperator(BinaryOperator child) : base(child.Op)
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
