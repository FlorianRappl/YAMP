using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    public enum Nonterminal
    {
        End /*stands for $*/, 
        Expression,  
        W, X, 
        Term,   
        K, Y, 
        Power, 
        V, Z, 
        Factor,   
        ParenthesisClose, 
        Constant,   //a
        NoValue, 
        AnyValue
    }
}
