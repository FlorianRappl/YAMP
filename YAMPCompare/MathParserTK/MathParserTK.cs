namespace MathParserTK_NET
{
    using System;
    using System.Collections.Generic;
    using System.Text;

     
	//MathParserTK — mathematical parser designed as a light, fast and simple to understand
	//module (class), which receives as input a mathematical expression (string) and 
	//return the output value (double). 
	//Copyright (C) 2012 Yerzhan Kalzhani , kirnbas@gmail.com

	//This program is free software: you can redistribute it and/or modify
	//it under the terms of the GNU General Public License as published by
	//the Free Software Foundation, either version 3 of the License, or
	//(at your option) any later version.

	//This program is distributed in the hope that it will be useful,
	//but WITHOUT ANY WARRANTY; without even the implied warranty of
	//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	//GNU General Public License for more details.

	//You should have received a copy of the GNU General Public License
	//along with this program.  If not, see <http://www.gnu.org/licenses/> 
     
	//Example usage:
	//public static void Main()
	//{     
	//        mathParser parser = new mathParser();
	//        string s = "sin(90)+1*3";
	//        bool radians = false; 
	//        double d = parser.Parsing(s, radians);    
	//}   

    /// <summary>
    /// A class of mathematical parser
    /// </summary>
    public class MathParserTK
    {
        private Dictionary<string, string> operators;
        private Dictionary<string, string> constants;
        private char decimalSeparator; 
        private bool radians;

        /// <summary>
        /// The class constructor initializes the dictionary and reads the decimal separator from the regional settings in OS
        /// </summary>
        public MathParserTK()
        {
            operators = new Dictionary<string, string>(50);
            constants = new Dictionary<string, string>(10);
            
            //dictionary supported math. operators and constants (left token (key), right input notation (value))
            /*--------------------------------------------------------Operators------------------------------------------------*/
            operators.Add("plus", "+");
            operators.Add("minus", "-");
            operators.Add("unPlus", "+");
            operators.Add("unMinus", "-");
            operators.Add("multiply", "*");
            operators.Add("division", "/");
            operators.Add("leftParenthesis", "(");
            operators.Add("rightParenthesis", ")");
            operators.Add("degree", "^");
            operators.Add("√", "√");
            operators.Add("sqrt", "sqrt");
            operators.Add("sin", "sin");
            operators.Add("cos", "cos");
            operators.Add("tg", "tg");
            operators.Add("ctg", "ctg");
            operators.Add("sh", "sh");
            operators.Add("ch", "ch");
            operators.Add("th", "th");
            operators.Add("log", "log");
            operators.Add("ln", "ln");
            operators.Add("exp", "exp");
            operators.Add("abs", "abs");
            /*-----------------------------------------------------------------------------------------------------------------*/
            /*--------------------------------------------------------Constants------------------------------------------------*/
            constants.Add(Math.E.ToString(), "e");
            constants.Add(Math.PI.ToString(), "pi");
            /*-----------------------------------------------------------------------------------------------------------------*/
            /*
            * If you want to add new operator, then add new record in operators and edit methods such as
            * getPrioritet, getNumberArguments, isFunction, calculating.
            * if you want to add new constants, then add new record in constants and check the compatibility with other
            * operators in method lexicalAnalyzator in convert expression to reverse polish notation.
            */

            try
            {
                decimalSeparator = Char.Parse(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            catch (Exception)
            {
                throw new Exception("Error: can't read char decimal separator from system, check your regional settings.");
            }
        }

        /// <summary>
        /// (1) Monitor class organizes the process of parse
        /// </summary>
        /// <param name="input">input expression (string)</param>
        /// <param name="radians">flag defines the argument of trigonometric functions</param>
        /// <returns>Return the calculated value of the expression</returns>
        public double Parse(string input, bool radians) 
        {
            
            double result = 0;
            this.radians = radians;
            
            if (String.IsNullOrEmpty(input) || !CheckParent(input))
            {
                throw new System.Exception();
            }
            
            //convert all symbols in input string to lower case and remove whitespaces in string to get rid of the various input errors
            string infixExp = RemoveWhitespaces(input.ToLower());

            try
            {
                string RPNExp = ConvertToRPN(infixExp);
                result = Convert.ToDouble(Calculate(RPNExp));
            }
            catch (DivideByZeroException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Method of checking the number of parenthesis in expressions
        /// </summary>
        /// <param name="input">input expression</param>
        /// <returns>Returns true if the number of opening and closing parentheses correspond to each other</returns>       
        private bool CheckParent(string input)
        {
            int count = 0;
            int i = 0;
            int l = input.Length;

            for (i = 0; i < l; i++)
            {
                if (input[i] == '(')
                {
                    count++;
                }
                else if (input[i] == ')')
                {
                    count--;
                }
            }
            return (count == 0);
        }

        /// <summary>
        /// Method of removing whitespaces in expressions
        /// </summary>
        /// <param name="input">input expression</param>
        /// <returns>Expression without whitespaces</returns>   
        private string RemoveWhitespaces(string input)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != ' ')
                    s.Append(input[i]);
            }

            return s.ToString();
        }

        /// <summary>
        /// Method returns a value of priority operator
        /// </summary>
        /// <param name="op">token is an operator</param>
        /// <returns>Return a value of priority operator</returns>
        private byte GetPrioritet(string op)
        {
            switch (op)
            {
                case "leftParenthesis":
                    return 0;
                case "rightParenthesis":
                    return 1;
                case "plus":
                case "minus":
                    return 2;
                case "unPlus":
                case "unMinus":
                    return 6;
                case "multiply":
                case "division":
                    return 4;
                case "degree":
                case "√":
                case "sqrt":
                    return 8;
                case "sin":
                case "cos":
                case "tg":
                case "ctg":
                case "sh":
                case "ch":
                case "th":
                case "log":
                case "ln":
                case "exp":
                case "abs":
                    return 10;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// This method need to check token, if (token is function) then (next symbol must be leftparenthesis)
        /// Этот метод необходим для проверка токена, если (токен это ф-ция) то (след. символ должен быть открывающей скобкой)
        /// </summary>
        /// <param name="s">token</param>
        /// <returns>Return true if token is function</returns>
        private bool IsFunction(string s)
        {
            switch (s)
            {
                case "√":
                case "sqrt":
                case "sin":
                case "cos":
                case "tg":
                case "ctg":
                case "sh":
                case "ch":
                case "th":
                case "log":
                case "ln":
                case "exp":
                case "abs":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// The method returns the number of arguments of the operator
        /// </summary>
        /// <param name="op">token is an operator</param>
        /// <returns>Number of arguments</returns>
        private byte GetNumberArguments(string op)
        {
            switch (op)
            {
                case "plus":
                case "minus":
                case "multiply":
                case "division":
                case "degree":
                case "log":                
                    return 2;
                case "unMinus":
                case "unPlus":
                case "√":
                case "sqrt":
                case "tg":
                case "sh":
                case "ch":
                case "th":
                case "ln":
                case "ctg":
                case "sin":
                case "cos":
                case "exp":
                case "abs":
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// (2) Method takes an expression in Reverse Polish Notation
        /// </summary>
        /// <param name="infixExp">expression in infix notation</param>
        /// <returns>expression in postfix notation</returns>
        private string ConvertToRPN(string infixExp)
        {
            string outputStr = ""; 
            Stack<string> stack = new Stack<string>();
            int pos = 0; //position on input string; позиция в строке 
            string token = ""; 

            //parse
            while (pos < infixExp.Length)
            {
                token = LexicalAnalyze(infixExp, pos, token); //lexical analysis; лексический анализ;

                //determination of the length of the source element
                 int length = 0;
                bool chislo = false;
                if (operators.ContainsKey(token))
                {
                    length = ((operators[token]).ToString()).Length;
                    chislo = false;
                }
                else
                {
                    if (constants.ContainsKey(token))
                        length = ((constants[token]).ToString()).Length;
                    else
                        length = token.Length;
                    chislo = true;
                }

                //modify string
                infixExp = infixExp.Remove(pos, length);
                infixExp = infixExp.Insert(pos, token);
                pos += token.Length;

                //processing of the received token
                if ((!chislo))
                {
                    if (token == "rightParenthesis")
                    {
                        while (stack.Peek() != "leftParenthesis")
                        {
                            outputStr += stack.Pop();
                        }
                        stack.Pop();
                    }
                    else
                    {
                        if (stack.Count == 0 || token == "leftParenthesis")
                        {
                            outputStr += " ";
                            stack.Push(token);
                            continue;
                        }
                        if (GetPrioritet(token) <= GetPrioritet(stack.Peek()))
                        {
                            outputStr += stack.Pop();
                            while ((stack.Count > 0) && (GetPrioritet(token) <= GetPrioritet(stack.Peek())))
                                outputStr += stack.Pop();
                            stack.Push(token);
                        }
                        else
                        {
                            outputStr += " ";
                            stack.Push(token);
                        }
                    }
                }
                else
                {
                    outputStr += token;
                }
            }
                        
            //pushing out the remainder of the stack to the output string
            if (stack.Count > 0)
                foreach (string s in stack)
                    outputStr += s;

            return outputStr;
        }

        /// <summary>
        /// (2.1) Lexical analyzer math expression in infix notation which refers method ConvertToRPN
        /// Лексический анализатор мат. выражения в алгебраической форме
        /// </summary>
        /// <param name="infixExp">expression in infix notation</param>
        /// <param name="pos">analyzed the current position in the expressions</param>
        /// <param name="preToken">the previous token</param>
        /// <returns>token</returns>                
        private string LexicalAnalyze(string infixExp, int pos, string preToken)
        {
            string token = "";
            bool unarnyi = false; //flag if operator unary
            if ((pos == 0) || (preToken == "leftParenthesis"))
                unarnyi = true;

            if (Char.IsDigit(infixExp[pos]))
                for (int i = pos; i < infixExp.Length && (Char.IsDigit(infixExp[i]) || infixExp[i] == decimalSeparator); i++)
                    token += infixExp[i];
            else
                if (!(Char.IsDigit(infixExp[pos]) || infixExp[pos] == decimalSeparator))
                    for (int i = pos; (i < infixExp.Length) && !(Char.IsDigit(infixExp[i]) || infixExp[i] == decimalSeparator); i++)
                    {
                        token += infixExp[i];
                        pos = i;
                        if (operators.ContainsValue(token) || constants.ContainsValue(token))
                        {
                            if (constants.ContainsValue(token))
                            {

                                if (token == "e")
                                {
                                    try
                                    {
                                        if (infixExp.Substring(pos, 3) == "exp")
                                        {
                                            token = "exp";
                                            return token;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    //throw exception if this number in scientific notation (for example 1E +10)
                                    bool preC = false;
                                    try
                                    {
                                        if (char.IsDigit(preToken[0]) || char.IsDigit(infixExp[pos + 1])
                                            || (infixExp[pos + 1] == '+') || (infixExp[pos + 1] == '-'))
                                        {
                                            preC = true;
                                            throw new Exception();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (preC) throw new Exception();
                                    }

                                }
                                //get value of constant
                                token = GetKeyInConstants(token);
                            }
                            else if (operators.ContainsValue(token))
                            {
                                token = GetKeyInOperators(token);
                                if (unarnyi && token == "minus") token = "unMinus";
                                if (unarnyi && token == "plus") token = "unPlus";
                                // function must be with parenthesis
                                if (IsFunction(token))
                                {
                                    if (infixExp[pos + 1] != '(')
                                        throw new Exception();
                                }
                            }
                            break;
                        }
                    }

            return token;
        }

        /// <summary>
        /// for get key of input value from dictionary "operators"
        /// </summary>
        /// <param name="token">input value</param>
        /// <returns>return key of input value</returns>        
        private string GetKeyInOperators(string token)
        {
            foreach (string key in operators.Keys)
            {
                if (token == operators[key])
                    token = key;
            }
            return token;
        }

        /// <summary>
        /// for get key of input value from dictionary "constants"
        /// </summary>
        /// <param name="token">input value</param>
        /// <returns>return key of input value</returns>  
        private string GetKeyInConstants(string token)
        {
            foreach (string key in constants.Keys)
            {
                if (token == constants[key])
                    token = key;
            }
            return token;
        }
        /// <summary>
        /// (3) The method of calculating the expression in Reverse Polish Notation
        /// </summary>
        /// <param name="RPNExp"></param>
        /// <returns></returns>        
        private string Calculate(string RPNExp)
        {
            Stack<string> stack = new Stack<string>();

            //parser
            int pos = 0;
            while (pos < RPNExp.Length)
            {
                string token = LexicalAnalyze(RPNExp); //lexical analysis
                RPNExp = RPNExp.Remove(pos, token.Length);

                if (token == " ")
                {
                    continue;
                }

                if (operators.ContainsKey(token))
                {
                    if (GetNumberArguments(token) == 1)
                    {
                        double arg = Convert.ToDouble(stack.Pop());

                        switch (token)
                        {
                            case "unMinus": stack.Push((-1 * arg).ToString()); break;
                            case "unPlus": stack.Push(arg.ToString()); break;
                            case "sqrt":
                            case "√": stack.Push((Math.Sqrt(arg)).ToString()); break;
                            case "sin":
                                if (radians)
                                    stack.Push((Math.Sin(arg)).ToString());
                                else
                                    stack.Push((Math.Sin(arg * Math.PI / 180)).ToString()); break;
                            case "cos":
                                if (radians)
                                    stack.Push((Math.Cos(arg)).ToString());
                                else
                                    stack.Push((Math.Cos(arg * Math.PI / 180)).ToString()); break;
                            case "tg":
                                if (radians)
                                    stack.Push((Math.Tan(arg)).ToString());
                                else
                                    stack.Push((Math.Tan(arg * Math.PI / 180)).ToString()); break;
                            case "ctg":
                                if (radians)
                                    stack.Push((1 / Math.Tan(arg)).ToString());
                                else
                                    stack.Push((1 / Math.Tan(arg * Math.PI / 180)).ToString()); break;
                            case "sh": stack.Push((Math.Sinh(arg)).ToString()); break;
                            case "ch": stack.Push((Math.Cosh(arg)).ToString()); break;
                            case "th": stack.Push((Math.Tanh(arg)).ToString()); break;
                            case "ln": stack.Push((Math.Log(arg)).ToString()); break;
                            case "exp": stack.Push((Math.Exp(arg)).ToString()); break;
                            case "abs": stack.Push((Math.Abs(arg)).ToString()); break;
                        }
                    }
                    else
                    {
                        double arg2 = Convert.ToDouble(stack.Pop());
                        double arg1 = Convert.ToDouble(stack.Pop());

                        switch (token)
                        {
                            case "plus": stack.Push((arg1 + arg2).ToString()); break;
                            case "minus": stack.Push((arg1 - arg2).ToString()); break;
                            case "multiply": stack.Push((arg1 * arg2).ToString()); break;
                            case "division":
                                if (arg2 == 0)
                                    throw new DivideByZeroException();
                                stack.Push((arg1 / arg2).ToString()); break;
                            case "degree": stack.Push((Math.Pow(arg1, arg2).ToString())); break;
                            case "log": stack.Push((Math.Log(arg2) / Math.Log(arg1)).ToString()); break;
                        }
                    }
                }
                else
                    stack.Push(token);
            }

            if (stack.Count > 1)
                throw new Exception();

            return stack.Peek();
        }

        /// <summary>
        /// (3.1) Lexical analyzer math expression in RPN which refers method "calculating"
        /// </summary>
        /// <param name="RPNExp">expression in RPN</param>
        /// <returns>token</returns>   
        private string LexicalAnalyze(string RPNExp)
        {
            //(3.1) Lexical analysis math. exp in RPN
            int pos = 0;
            string token = "" + RPNExp[pos];
            if (token == " ") return " ";

            if (!operators.ContainsKey(token))
            {
                if (Char.IsDigit(RPNExp[pos]))
                    for (int i = pos + 1; i < RPNExp.Length &&
                        (Char.IsDigit(RPNExp[i]) || RPNExp[i] == decimalSeparator); i++)
                        token += RPNExp[i];
                else if (Char.IsLetter(RPNExp[pos]))
                    for (int i = pos + 1; i < RPNExp.Length && (Char.IsLetter(RPNExp[i])); i++)
                    {
                        token += RPNExp[i];
                        if (operators.ContainsKey(token))
                        {
                            return token;
                        }
                    }
            }
            else
            {
                return token;
            }

            return token;
        } 
    }
}