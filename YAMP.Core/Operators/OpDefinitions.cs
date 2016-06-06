namespace YAMP
{
    using System;

    /// <summary>
    /// This public class declares some exposable operator's symbols and levels
    /// </summary>
    public static class OpDefinitions
    {
        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 ArgsOperatorLevel = 1000;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String ArgsOperator = "(";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 MemberOperatorLevel = 990;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String MemberOperator = ".";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 FactorialOperatorLevel = 980;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String FactorialOperator = "!";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PreIncOperatorLevel = 950;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PreIncOperator = "++";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PostIncOperatorLevel = 950;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PostIncOperator = "++";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PreDecOperatorLevel = 950;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PreDecOperator = "--";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PostDecOperatorLevel = 950;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PostDecOperator = "--";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 InvOperatorLevel = 900;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String InvOperator = "~";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PowerOperatorLevel = 100;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PowerOperator = "^";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 AdjungateOperatorLevel = 100;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String AdjungateOperator = "'";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 TransposeOperatorLevel = 100;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String TransposeOperator = ".'";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 ModuloOperatorLevel = 30;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String ModuloOperator = "%";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 LeftDivideOperatorLevel = 20;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String LeftDivideOperator = @"\";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 RightDivideOperatorLevel = 20;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String RightDivideOperator = "/";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 MultiplyOperatorLevel = 10;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String MultiplyOperator = "*";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 NegateOperatorLevel = 7;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String NegateOperator = "-";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PosateOperatorLevel = 7;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PosateOperator = "+";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 MinusOperatorLevel = 6;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String MinusOperator = "-";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 PlusOperatorLevel = 5;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String PlusOperator = "+";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 DefLogicOperatorLevel = 4;

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 EqOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String EqOperator = "==";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 GtEqOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String GtEqOperator = ">=";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 GtOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String GtOperator = ">";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 LtEqOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String LtEqOperator = "<=";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 LtOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String LtOperator = "<";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 NeqOperatorLevel = DefLogicOperatorLevel;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String StandardNeqOperator = "~=";

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String AliasNeqOperator = "!=";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 AndOperatorLevel = 3;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String AndOperator = "&&";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 OrOperatorLevel = 3;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String OrOperator = "||";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 RangeOperatorLevel = 3;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String RangeOperator = ":";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 FatArrowOperatorLevel = 2;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String FatArrowOperator = "=>";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 ColumnOperatorLevel = 1;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String ColumnOperator = ",";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 CommaOperatorLevel = 1;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String CommaOperator = ",";

        /// <summary>
        /// The assigned operator level.
        /// </summary>
        public const Int32 RowOperatorLevel = 0;

        /// <summary>
        /// The operator symbol.
        /// </summary>
        public const String RowOperator = ";";
    }
}
