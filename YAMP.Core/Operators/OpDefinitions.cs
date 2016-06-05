namespace YAMP
{
    using System;

    /// <summary>
    /// This public class declares some exposable operator's symbols and levels
    /// </summary>
    public static class OpDefinitions
    {
        public const int ArgsOperatorLevel = 1000;
        public const String ArgsOperator = "(";

        public const int MemberOperatorLevel = 990; //Moved from 10000 to 990 to be under ArgsOperatorLevel
        public const String MemberOperator = ".";

        public const int FactorialOperatorLevel = 980; //Moved from 1000 to 980 to be under ArgsOperatorLevel (and allow space for MemberOperatorLevel)
        public const String FactorialOperator = "!";

        public const int PreIncOperatorLevel = 950; //Moved from 999 to 950 to give space below FactorialOperatorLevel and ArgsOperatorLevel
        public const String PreIncOperator = "++";

        public const int PostIncOperatorLevel = 950; //Moved from 999 to 950 to give space below FactorialOperatorLevel and ArgsOperatorLevel
        public const String PostIncOperator = "++";

        public const int PreDecOperatorLevel = 950; //Moved from 999 to 950 to give space below FactorialOperatorLevel and ArgsOperatorLevel
        public const String PreDecOperator = "--";

        public const int PostDecOperatorLevel = 950; //Moved from 999 to 950 to give space below FactorialOperatorLevel and ArgsOperatorLevel
        public const String PostDecOperator = "--";

        public const int InvOperatorLevel = 900; //Moved from 995 to 900 to give some space.
        public const String InvOperator = "~";

        public const int PowerOperatorLevel = 100;
        public const String PowerOperator = "^";

        public const int AdjungateOperatorLevel = 100;
        public const String AdjungateOperator = "'";

        public const int TransposeOperatorLevel = 100;
        public const String TransposeOperator = ".'";

        public const int ModuloOperatorLevel = 30;
        public const String ModuloOperator = "%";

        public const int LeftDivideOperatorLevel = 20;
        public const String LeftDivideOperator = @"\";

        public const int RightDivideOperatorLevel = 20;
        public const String RightDivideOperator = "/";

        public const int MultiplyOperatorLevel = 10;
        public const String MultiplyOperator = "*";

        public const int NegateOperatorLevel = 7;
        public const String NegateOperator = "-";

        public const int PosateOperatorLevel = 7;
        public const String PosateOperator = "+";

        public const int MinusOperatorLevel = 6; //Shouldn't be the same as PlusOperatorLevel ?
        public const String MinusOperator = "-";

        public const int PlusOperatorLevel = 5;
        public const String PlusOperator = "+";

        
        public const int DefLogicOperatorLevel = 4; //Default level for Logical Operators

        public const int EqOperatorLevel = DefLogicOperatorLevel;
        public const String EqOperator = "==";

        public const int GtEqOperatorLevel = DefLogicOperatorLevel;
        public const String GtEqOperator = ">=";

        public const int GtOperatorLevel = DefLogicOperatorLevel;
        public const String GtOperator = ">";

        public const int LtEqOperatorLevel = DefLogicOperatorLevel;
        public const String LtEqOperator = "<=";

        public const int LtOperatorLevel = DefLogicOperatorLevel;
        public const String LtOperator = "<";

        public const int NeqOperatorLevel = DefLogicOperatorLevel;
        public const String StandardNeqOperator = "~=";
        public const String AliasNeqOperator = "!=";

        public const int AndOperatorLevel = 3; //Shouldn't AndOperatorLevel be higher than OrOperatorLevel ?
        public const String AndOperator = "&&";

        public const int OrOperatorLevel = 3;
        public const String OrOperator = "||";

        public const int RangeOperatorLevel = 3;
        public const String RangeOperator = ":";

        public const int FatArrowOperatorLevel = 2;
        public const String FatArrowOperator = "=>";
       

        public const int ColumnOperatorLevel = 1;
        public const String ColumnOperator = ",";

        public const int CommaOperatorLevel = 1;
        public const String CommaOperator = ",";

        public const int RowOperatorLevel = 0;
        public const String RowOperator = ";";

    }
}
