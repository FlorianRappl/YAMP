namespace YAMP.Sets
{
    using System;

    public static class OpDefinitionsSet
    {
        //100
        public const int ExceptXorOperatorLevel = OpDefinitions.PowerOperatorLevel; // 100;
        public const String ExceptXorOperator = OpDefinitions.PowerOperator;

        //6
        public const int ExceptOperatorLevel = OpDefinitions.MinusOperatorLevel; // 6; //Shouldn't be the same as PlusOperatorLevel ?
        public const String ExceptOperator = OpDefinitions.MinusOperator;

        public const int IntersectOperatorLevel = 6;
        public const String IntersectOperator = "&";

        //5
        public const int PlusOperatorLevel = OpDefinitions.PlusOperatorLevel; // 5;
        public const String UnionOperator = OpDefinitions.PlusOperator;

        //4
        public const int DefLogicOperatorLevel = OpDefinitions.DefLogicOperatorLevel; // 4; //Default level for Logical Operators

        public const int EqOperatorLevel = DefLogicOperatorLevel;
        public const String EqOperator = OpDefinitions.EqOperator;

        public const int NeqOperatorLevel = DefLogicOperatorLevel;
        public const String StandardNeqOperator = OpDefinitions.StandardNeqOperator;
        public const String AliasNeqOperator = OpDefinitions.AliasNeqOperator;
    }

}
