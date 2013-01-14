﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    static class Logical
    {
        public static void Register(CalcEngine ce)
        {
            ce.RegisterFunction("AND", 1, int.MaxValue, And);
            ce.RegisterFunction("OR", 1, int.MaxValue, Or);
            ce.RegisterFunction("NOT", 1, Not);
            ce.RegisterFunction("IF", 3, If);
            ce.RegisterFunction("TRUE", 0, True);
            ce.RegisterFunction("FALSE", 0, False);
        }
#if DEBUG
        public static void Test(CalcEngine ce)
        {
            ce.Test("AND(true, true)", true);
            ce.Test("AND(true, false)", false);
            ce.Test("AND(false, true)", false);
            ce.Test("AND(false, false)", false);
            ce.Test("OR(true, true)", true);
            ce.Test("OR(true, false)", true);
            ce.Test("OR(false, true)", true);
            ce.Test("OR(false, false)", false);
            ce.Test("NOT(false)", true);
            ce.Test("NOT(true)", false);
            ce.Test("IF(5 > 4, true, false)", true);
            ce.Test("IF(5 > 14, true, false)", false);
            ce.Test("TRUE()", true);
            ce.Test("FALSE()", false);
        }
#endif
        static object And(List<Expression> p)
        {
            var b = true;
            foreach (var v in p)
            {
                b = b && (bool)v;
            }
            return b;
        }
        static object Or(List<Expression> p)
        {
            var b = false;
            foreach (var v in p)
            {
                b = b || (bool)v;
            }
            return b;
        }
        static object Not(List<Expression> p)
        {
            return !(bool)p[0];
        }
        static object If(List<Expression> p)
        {
            return (bool)p[0] 
                ? p[1].Evaluate() 
                : p[2].Evaluate();
        }
        static object True(List<Expression> p)
        {
            return true;
        }
        static object False(List<Expression> p)
        {
            return false;
        }
    }
}
