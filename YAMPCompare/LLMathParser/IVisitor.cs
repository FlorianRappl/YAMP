using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    public interface IVisitor
    {
        void Action(MathParserTreeNode treeNode);
    }
}
