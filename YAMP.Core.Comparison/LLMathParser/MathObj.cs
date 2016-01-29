using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace MathParserDataStructures
{
    [CLSCompliant(true)]
    public class MathObj
    {
        private Operation[] _polishPostfixExpression;
        private int _sizeOfPolishPostfixExpression;
        private int _indexOfChar;
        private List<double> _listOfDoubleInputs;
        private List<char> _listOfVariableInputs;

        private string _mathInput;
        private Dictionary<char, double> _varsAndValues;  //variables and their values

        private MathParserBinaryTree _binaryTree;
        private Dictionary<string, Action<Stack<MathParserTreeNode>>> _mathFuncDecision;

        private bool _continueParsing;
        private bool _persistState;

        private MathParserTreeNode _traverseNode;

        #region Constructors
        /// <summary>
        /// Create new MathObj object
        /// </summary>
        public MathObj()
        {
            _listOfDoubleInputs = new List<double>();
            _listOfVariableInputs = new List<char>();
            _mathFuncDecision = new Dictionary<string, Action<Stack<MathParserTreeNode>>>();
            _varsAndValues = new Dictionary<char,double>();
            //mathematical function decision//
            _mathFuncDecision.Add("sin(", (stack) => Sin(stack));
            _mathFuncDecision.Add("sec(", (stack) => Sec(stack));
            _mathFuncDecision.Add("sqrt", (stack) => Sqrt(stack));
            _mathFuncDecision.Add("cos(", (stack) => Cos(stack));
            _mathFuncDecision.Add("cot(", (stack) => Cot(stack));
            _mathFuncDecision.Add("csc(", (stack) => Csc(stack));
            _mathFuncDecision.Add("tan(", (stack) => Tan(stack));
            _mathFuncDecision.Add("asin", (stack) => Asin(stack));
            _mathFuncDecision.Add("acos", (stack) => Acos(stack));
            _mathFuncDecision.Add("atan", (stack) => Atan(stack));
            _mathFuncDecision.Add("acot", (stack) => Acot(stack));
            _mathFuncDecision.Add("exp(", (stack) => Exp(stack));
            _mathFuncDecision.Add("log(", (stack) => Log(stack));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Math parser Binary tree.
        /// </summary>
        public MathParserBinaryTree MathParserBinaryTree
        {
            get
            {
                return _binaryTree;
            }
        }
        /// <summary>
        /// Polish postfix expression.
        /// </summary>
        public MathParserDataStructures.Operation[] GetPolishPostfixExpression()
        {
            return (Operation[])_polishPostfixExpression.Clone();
        }
        /// <summary>
        /// Expression to be parsed.
        /// </summary>
        public string MathInput
        {
            get
            {
                return _mathInput;
            }
        }
        /// <summary>
        /// Gets the array of variables
        /// </summary>
        /// <returns></returns>
        public char[] GetVariables()
        {
            char[] temp = new char[_varsAndValues.Keys.Count];
            _varsAndValues.Keys.CopyTo(temp, 0);
            return temp;
        }
        /// <summary>
        /// Gets the array of variable's values
        /// </summary>
        /// <returns></returns>
        public double[] GetVariablesValues()
        {
            double[] temp = new double[_varsAndValues.Values.Count];
            _varsAndValues.Values.CopyTo(temp, 0);
            return temp;
        }
        #endregion

        #region Semantic Decision Methods
        /// <summary>
        /// Expression to TermW (E->TW)
        /// </summary>
        /// <param name="stack">Stack</param>
        private void ExpressionToTermW(Stack<MathParserTreeNode> stack)
        {
            if(this._mathInput.Length <= this._indexOfChar) //end of stream isn't expected
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function, '+','-', at input[" + _indexOfChar.ToString() + "]");
            if (Char.IsDigit(this._mathInput[_indexOfChar]) || 
                this._varsAndValues.ContainsKey(this._mathInput[_indexOfChar]) ||
                this._mathInput[_indexOfChar] == '.' ||
                this._mathInput[_indexOfChar] == 's' ||
                this._mathInput[_indexOfChar] == 'l' || 
                this._mathInput[_indexOfChar] == 'c' ||
                this._mathInput[_indexOfChar] == 'e' ||
                this._mathInput[_indexOfChar] == 't' || 
                this._mathInput[_indexOfChar] == 'a' ||
                this._mathInput[_indexOfChar] == '-' ||
                this._mathInput[_indexOfChar] == '+' || 
                this._mathInput[_indexOfChar] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Term), new MathParserTreeNode(Nonterminal.W));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function, '+','-', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// W->XW or W->eps
        /// </summary>
        /// <param name="stack">Stack</param>
        private void WtoXWorEps(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput.Length <= this._indexOfChar)
                return; //end of stream allowed
            if (this._mathInput[this._indexOfChar] == '+' || this._mathInput[this._indexOfChar] == '-')  //W->XW
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.X) , new MathParserTreeNode(Nonterminal.W));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                if (this._mathInput[this._indexOfChar] != ')')//this._mathInput[this._indexOfChar] != '$')
                    throw new MathParserException("Wrong input string. Expecting for: '-','+',')', end of input, at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// X->-T or X->+T
        /// </summary>
        /// <param name="stack"></param>
        private void XtoMinusTermOrXtoPlusTerm(Stack<MathParserTreeNode> stack)
        {
            if(this._mathInput.Length <= this._indexOfChar) //end of stream isn't expected
                throw new MathParserException("Wrong input string. Expecting for: '+','-', at input[" + _indexOfChar.ToString() + "]");
            switch (this._mathInput[this._indexOfChar])
            { 
                case '+':   //X->+Term
                    _traverseNode.Operation = Operation.Plus;
                    _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.Term));
                    this._indexOfChar++;
                    this._sizeOfPolishPostfixExpression++;
                    stack.Push(_traverseNode.Left);
                    break;
                case '-':   //X->-Term
                    _traverseNode.Operation = Operation.Minus;
                    _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.Term));
                    this._indexOfChar++;
                    this._sizeOfPolishPostfixExpression++;
                    stack.Push(_traverseNode.Left);
                    break;
                default:
                    throw new MathParserException("Wrong input string. Expecting for: '+','-', at input[" + _indexOfChar.ToString() + "]");
            }
        }
        
        /// <summary>
        /// Term -> PowerK 
        /// </summary>
        /// <param name="stack">Stack</param>
        private void TermToPowerK(Stack<MathParserTreeNode> stack)
        {
            if(this._mathInput.Length <= this._indexOfChar) //end of stream isn't expected
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function, '+','-','(', number [0..9], at input[" + _indexOfChar.ToString() + "]");
            if (Char.IsDigit(this._mathInput[_indexOfChar]) ||
                this._varsAndValues.ContainsKey(this._mathInput[_indexOfChar]) ||
                this._mathInput[_indexOfChar] == '.' ||
                this._mathInput[_indexOfChar] == 's' ||
                this._mathInput[_indexOfChar] == 'l' ||
                this._mathInput[_indexOfChar] == 'c' ||
                this._mathInput[_indexOfChar] == 'e' ||
                this._mathInput[_indexOfChar] == 't' ||
                this._mathInput[_indexOfChar] == 'a' ||
                this._mathInput[_indexOfChar] == '-' ||
                this._mathInput[_indexOfChar] == '+' ||
                this._mathInput[_indexOfChar] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Power), new MathParserTreeNode(Nonterminal.K));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function, '+','-','(', number [0..9], at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// K->YK or K->eps
        /// </summary>
        /// <param name="stack">Stack</param>
        private void KtoYKorKtoEps(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput.Length <= this._indexOfChar)
                return; //end of stream allowed
            if (this._mathInput[this._indexOfChar] == '*' || this._mathInput[this._indexOfChar] == '/')  //K->YK
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Y), new MathParserTreeNode(Nonterminal.K));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                if (this._mathInput[this._indexOfChar] != ')' &&
                    this._mathInput[this._indexOfChar] != '+' && this._mathInput[this._indexOfChar] != '-')
                    throw new MathParserException("Wrong input string. Expecting for: '*','/','+','-',')', end of input, at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Y->*P or Y->/P
        /// </summary>
        /// <param name="stack">Stack</param>
        private void YtoMultiplyPowerOrYtoDividePower(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput.Length <= this._indexOfChar) //end of stream isn't expected
                throw new MathParserException("Wrong input string. Expecting for: '*','/', at input[" + _indexOfChar.ToString() + "]");
            switch (this._mathInput[this._indexOfChar])
            {
                case '*':   //Y->*Power
                    _traverseNode.Operation = Operation.Multiply;
                    _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.Power));
                    this._indexOfChar++;
                    this._sizeOfPolishPostfixExpression++;
                    stack.Push(_traverseNode.Left);
                    break;
                case '/':   //Y->/Power
                    _traverseNode.Operation = Operation.Divide;
                    _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.Power));
                    this._indexOfChar++;
                    this._sizeOfPolishPostfixExpression++;
                    stack.Push(_traverseNode.Left);
                    break;
                default:
                    throw new MathParserException("Wrong input string. Expecting for: '*','/', at input[" + _indexOfChar.ToString() + "]");
            }
        }
        /// <summary>
        /// Power->FactorV
        /// </summary>
        /// <param name="stack">Stack</param>
        private void PowerToFactoV(Stack<MathParserTreeNode> stack)
        {
            if(_mathInput.Length <= this._indexOfChar)
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function,'(','+','-',number [0..9] at input[" + _indexOfChar.ToString() + "]");
            if (Char.IsDigit(this._mathInput[_indexOfChar]) ||
                this._varsAndValues.ContainsKey(this._mathInput[_indexOfChar]) ||
                this._mathInput[_indexOfChar] == '.' ||
                this._mathInput[_indexOfChar] == 's' ||
                this._mathInput[_indexOfChar] == 'l' ||
                this._mathInput[_indexOfChar] == 'c' ||
                this._mathInput[_indexOfChar] == 'e' ||
                this._mathInput[_indexOfChar] == 't' ||
                this._mathInput[_indexOfChar] == 'a' ||
                this._mathInput[_indexOfChar] == '-' ||
                this._mathInput[_indexOfChar] == '+' ||
                this._mathInput[_indexOfChar] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Factor), new MathParserTreeNode(Nonterminal.V));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for: 'variable', math function,'(','+','-',number [0..9] at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// V->ZV or V->eps
        /// </summary>
        /// <param name="stack"></param>
        private void VtoZVorVtoEps(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput.Length <= this._indexOfChar)
                return; //end of stream allowed
            if (this._mathInput[this._indexOfChar] == '^')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Z), new MathParserTreeNode(Nonterminal.V));
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                if (this._mathInput[this._indexOfChar] != '+' && this._mathInput[this._indexOfChar] != '-' &&
                    this._mathInput[this._indexOfChar] != '*' && this._mathInput[this._indexOfChar] != '/' &&
                    this._mathInput[this._indexOfChar] != ')')
                    throw new MathParserException("Wrong input string. Expecting for: '^','+','-','*','/',')', end of input, at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Z->^Factor
        /// </summary>
        /// <param name="stack"></param>
        private void ZtoPowerFactor(Stack<MathParserTreeNode> stack)
        {
            if(_mathInput.Length <= this._indexOfChar)
                throw new MathParserException("Wrong input string. Expecting for: '^', at input[" + _indexOfChar.ToString() + "]");
            if (this._mathInput[this._indexOfChar] == '^')
            {
                _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.Factor));
                _traverseNode.Operation = Operation.Power;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar++;
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for: '^', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Factor
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Factor(Stack<MathParserTreeNode> stack)
        {
            if(this._mathInput.Length <= this._indexOfChar)
                throw new MathParserException("Wrong input string. At input[" + _indexOfChar.ToString() + "]");
            string temp = null;
            if (this._varsAndValues.ContainsKey(this._mathInput[_indexOfChar]))
            {
                _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.AnyValue, Operation.Variable));
                this._sizeOfPolishPostfixExpression++;
                stack.Push(_traverseNode.Left);
                this._listOfVariableInputs.Add(this._mathInput[_indexOfChar++]);
                return;
            }

            if (Char.IsDigit(this._mathInput[this._indexOfChar]) || this._mathInput[this._indexOfChar] == '.')  //check if next character is digit
            {
                _traverseNode.AddLeftChild(new MathParserTreeNode(Nonterminal.AnyValue, Operation.AnyReal));
                this._sizeOfPolishPostfixExpression++;
                _traverseNode.Value = this.FindValueOfSubstring();
                this._listOfDoubleInputs.Add(_traverseNode.Value);
                stack.Push(_traverseNode.Left);
                return;
            }

            if (this._mathInput[this._indexOfChar] == '(')  //check if closing paranthesis
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _indexOfChar++;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
                return;
            }

            if (this._mathInput[this._indexOfChar] == '+')   //check if unary plus
            {
                _traverseNode.Operation = Operation.Plus;
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.AnyValue), new MathParserTreeNode(Nonterminal.Factor));
                _traverseNode.Left.Operation = Operation.UnaryPlus;
                this._sizeOfPolishPostfixExpression += 2;
                stack.Push(_traverseNode.Right);
                this._indexOfChar++;
                return;
            }

            if (this._mathInput[this._indexOfChar] == '-')   //check if unary minus
            {
                _traverseNode.Operation = Operation.Minus;
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.AnyValue), new MathParserTreeNode(Nonterminal.Factor));
                _traverseNode.Left.Operation = Operation.UnaryMinus;
                this._sizeOfPolishPostfixExpression += 2;
                stack.Push(_traverseNode.Right);
                this._indexOfChar++;
                return;
            }
            if (this._mathInput.Length > this._indexOfChar + 4)
            {
                temp = this._mathInput.Substring(this._indexOfChar, 4);
                if (_mathFuncDecision.ContainsKey(temp))               //check if the next item is math function
                {
                    try
                    {
                        _mathFuncDecision[temp].Invoke(stack);
                        return;
                    }
                    catch (MathParserException)
                    {
                        throw;
                    }
                }
            }
            throw new MathParserException("Wrong input string. At input[" + _indexOfChar.ToString() + "]");
            
            
        }
        /// <summary>
        /// Closing paranthesis.
        /// </summary>
        /// <param name="stack">Stack</param>
        private void ParanthesisClose(Stack<MathParserTreeNode> stack)
        {
            if(this._mathInput.Length <= this._indexOfChar)
                throw new MathParserException("Wrong input string. Expecting for: ')', at input[" + _indexOfChar.ToString() + "]");
            if (this._mathInput[this._indexOfChar] == ')')
                this._indexOfChar++;
            else
                throw new MathParserException("Wrong input string. Expecting for: ')', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// End of the input stream.
        /// </summary>
        /// <param name="stack">Stack</param>
        private void End(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput.Length == this._indexOfChar /* == '$'*/)
                _continueParsing = false;
            else
                throw new MathParserException("Wrong input string. Expecting for end of input stream, at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Finds the value of substring in the input string.
        /// </summary>
        /// <returns>Double</returns>
        private double FindValueOfSubstring()
        {
            StringBuilder strBuilder = new StringBuilder();
            while (Char.IsDigit(this._mathInput[this._indexOfChar]) || this._mathInput[this._indexOfChar] == '.')
            {
                strBuilder.Append(this._mathInput[this._indexOfChar++]);
                if (_indexOfChar == _mathInput.Length)
                    break;
            }
            return double.Parse(strBuilder.ToString(), CultureInfo.GetCultureInfo("en-US"));
        }
        #endregion

        #region Mathematical decision methods used in semantic transformation
        /// <summary>
        /// Sine
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Sin(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Sin;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Sqrt
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Sqrt(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput[this._indexOfChar + 4] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _traverseNode.Left.Operation = Operation.Sqrt;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar += 5;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for end '(', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Secant
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Sec(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Sec;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Consine
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Cos(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Cos;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Cotangent
        /// </summary>
        /// <param name="stack"></param>
        private void Cot(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Cot;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Tangent
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Tan(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Tan;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Cosecant
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Csc(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Csc;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Asin
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Asin(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput[this._indexOfChar + 4] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _traverseNode.Left.Operation = Operation.Asin;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar += 5;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for end '(', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Acos
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Acos(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput[this._indexOfChar + 4] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _traverseNode.Left.Operation = Operation.Acos;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar += 5;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for end '(', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Atan
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Atan(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput[this._indexOfChar + 4] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _traverseNode.Left.Operation = Operation.Atan;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar += 5;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for end '(', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Acot
        /// </summary>
        /// <param name="stack">Stack</param>
        private void Acot(Stack<MathParserTreeNode> stack)
        {
            if (this._mathInput[this._indexOfChar + 4] == '(')
            {
                _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
                _traverseNode.Left.Operation = Operation.Acot;
                this._sizeOfPolishPostfixExpression++;
                this._indexOfChar += 5;
                stack.Push(_traverseNode.Right);
                stack.Push(_traverseNode.Left);
            }
            else
                throw new MathParserException("Wrong input string. Expecting for end '(', at input[" + _indexOfChar.ToString() + "]");
        }
        /// <summary>
        /// Exp
        /// </summary>
        /// <param name="stack"></param>
        private void Exp(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Exp;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        /// <summary>
        /// Logarithm
        /// </summary>
        /// <param name="stack"></param>
        private void Log(Stack<MathParserTreeNode> stack)
        {
            _traverseNode.AddChildren(new MathParserTreeNode(Nonterminal.Expression), new MathParserTreeNode(Nonterminal.ParenthesisClose));
            _traverseNode.Left.Operation = Operation.Log;
            this._sizeOfPolishPostfixExpression++;
            this._indexOfChar += 4;
            stack.Push(_traverseNode.Right);
            stack.Push(_traverseNode.Left);
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Evaluate the mathematical expression.
        /// </summary>
        /// <param name="mathInput">Mathematical expression</param>
        /// <param name="vars">Character variables contained within the expression[x,y,z etc.]</param>
        /// <param name="varsValues">Coresponding values of character variables within the expression</param>
        /// <returns>The result of evaluation</returns>
        /// <exception cref="MathParserException">MathParserException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public virtual double Evaluate(string mathInput, char[] vars, double[] varsValues)
        {
			// We do not want to cache the results - bad for benchmarks!
			this._mathInput = string.Empty;

            if (String.IsNullOrEmpty(mathInput))
                throw new ArgumentException("String mathInput is empty or null");

            if (this._mathInput == mathInput)
                _persistState = true;
            else
            {
                this._mathInput = mathInput;
                _persistState = false;
            }

            if (_varsAndValues.Count != 0)
                _varsAndValues.Clear();
            if (vars != null && varsValues != null)
            {
                if (vars.Length != varsValues.Length)
                    throw new ArgumentException("vars and varsValues arrays length are not equal");    // for each variable should be define the coresponding value
                for (int i = 0; i < vars.Length; i++)
                    this._varsAndValues.Add(vars[i], varsValues[i]);    //building the dictionary with vars and values.
            }
            double retVal;
            try
            {

                if (!_persistState)
                {
                    this.SemanticTransform();   //semantic transform (building the tree)
                    this._polishPostfixExpression = new Operation[this._sizeOfPolishPostfixExpression]; //
                    this.GeneratePolishPostfixExpression();
                }
                retVal = this.CalculateValue();
            }
            catch (MathParserException)
            {
                throw;
            }
            return retVal;
        }
        /// <summary>
        /// Perform semantic transform of the introduced expression. Check if the expression is valid and build BinaryTree needed for polish postfix expression.
        /// </summary>
        /// <exception cref="MathParserException">MathParserException</exception>
        protected virtual void SemanticTransform()
        {
            _continueParsing = true;
            _binaryTree = new MathParserBinaryTree(new MathParserTreeNode(Nonterminal.End));
            this._sizeOfPolishPostfixExpression = 0;    //initializing
            _indexOfChar = 0;
            if (_listOfDoubleInputs.Count != 0)
                _listOfDoubleInputs.Clear();
            if (_listOfVariableInputs.Count != 0)
                _listOfVariableInputs.Clear();
            //this._mathInput = this._mathInput.Trim() + "$"; //appeding the end-character
            this._binaryTree.Root.AddLeftChild(new MathParserTreeNode(Nonterminal.Expression)); //Expression$
            Stack<MathParserTreeNode> stack = new Stack<MathParserTreeNode>();
            stack.Push(this._binaryTree.Root);  //$
            _traverseNode = this._binaryTree.Root.Left;
            stack.Push(_traverseNode);   //starting from Nonterminal.Expression

            while (_continueParsing)    //main cycle of parsing
            {
                _traverseNode = stack.Pop();
                switch (_traverseNode.Name)
                { 
                    case Nonterminal.Expression:
                        this.ExpressionToTermW(stack);
                        break;
                    case Nonterminal.W:
                        this.WtoXWorEps(stack);
                        break;
                    case Nonterminal.X:
                        this.XtoMinusTermOrXtoPlusTerm(stack);
                        break;
                    case Nonterminal.Term:
                        this.TermToPowerK(stack);
                        break;
                    case Nonterminal.K:
                        this.KtoYKorKtoEps(stack);
                        break;
                    case Nonterminal.Y:
                        this.YtoMultiplyPowerOrYtoDividePower(stack);
                        break;
                    case Nonterminal.Power:
                        this.PowerToFactoV(stack);
                        break;
                    case Nonterminal.V:
                        this.VtoZVorVtoEps(stack);
                        break;
                    case Nonterminal.Z:
                        this.ZtoPowerFactor(stack);
                        break;
                    case Nonterminal.Factor:
                        this.Factor(stack);
                        break;
                    case Nonterminal.ParenthesisClose:
                        this.ParanthesisClose(stack);
                        break;
                    case Nonterminal.End:
                        this.End(stack);
                        break;
                    case Nonterminal.AnyValue:
                        break;
                }
            }
        }
        /// <summary>
        /// Generates polish postfix expression.
        /// </summary>
        protected virtual void GeneratePolishPostfixExpression()
        {
            PolishVisitor polishVisitor = new PolishVisitor(this._sizeOfPolishPostfixExpression);
            this._binaryTree.Visit(polishVisitor);
            this._polishPostfixExpression = polishVisitor.GetPolishPostfixExpression();
        }
        /// <summary>
        /// Calculate the value of the polish postfix notation
        /// </summary>
        /// <returns></returns>
        protected virtual double CalculateValue()
        {
            double a, b;
            int indexVariables = 0;
            int indexValues = 0;
            Stack<double> stack = new Stack<double>();
            for (int i = 0; i < this._sizeOfPolishPostfixExpression; i++)
            {
                switch (this._polishPostfixExpression[i])
                {
                    case Operation.UnaryMinus:
                        stack.Push(0);
                        break;
                    case Operation.UnaryPlus:
                        stack.Push(0);
                        break;
                    case Operation.Variable:
                        stack.Push(this._varsAndValues[this._listOfVariableInputs[indexVariables++]]);
                        break;
                    case Operation.AnyReal:
                        stack.Push(this._listOfDoubleInputs[indexValues++]);
                        break;
                    case Operation.Sin:
                        stack.Push(Math.Sin(stack.Pop()));
                        break;
                    case Operation.Cos:
                        stack.Push(Math.Cos(stack.Pop()));
                        break;
                    case Operation.Cot:
                        stack.Push(1 / Math.Tan(stack.Pop()));
                        break;
                    case Operation.Sqrt:
                        stack.Push(Math.Sqrt(stack.Pop()));
                        break;
                    case Operation.Asin:
                        stack.Push(Math.Asin(stack.Pop()));
                        break;
                    case Operation.Acos:
                        stack.Push(Math.Acos(stack.Pop()));
                        break;
                    case Operation.Atan:
                        stack.Push(Math.Atan(stack.Pop()));
                        break;
                    case Operation.Acot:
                        stack.Push(Math.Atan(1 / stack.Pop()));
                        break;
                    case Operation.Sec:
                        stack.Push(1 / Math.Cos(stack.Pop()));
                        break;
                    case Operation.Csc:
                        stack.Push(1 / Math.Sin(stack.Pop()));
                        break;
                    case Operation.Tan:
                        stack.Push(Math.Tan(stack.Pop()));
                        break;
                    case Operation.Exp:
                        stack.Push(Math.Exp(stack.Pop()));
                        break;
                    case Operation.Log:
                        stack.Push(Math.Log(stack.Pop()));
                        break;
                    case Operation.Power:
                        b = stack.Pop();
                        a = stack.Pop();
                        stack.Push(Math.Pow(a, b));
                        break;
                    case Operation.Plus:
                        stack.Push(stack.Pop() + stack.Pop());
                        break;
                    case Operation.Minus:
                        b = stack.Pop();
                        a = stack.Pop();
                        stack.Push(a - b);
                        break;
                    case Operation.Multiply:
                        stack.Push(stack.Pop() * stack.Pop());
                        break;
                    case Operation.Divide:
                        b = stack.Pop();
                        a = stack.Pop();
                        stack.Push(a / b);
                        break;
                }
            }
            return stack.Pop();
        }
        #endregion
    }
}
