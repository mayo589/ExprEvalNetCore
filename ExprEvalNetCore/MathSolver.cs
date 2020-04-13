using ExprEvalNetCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExprEvalNetCore
{
    public class MathSolver
    {
        private List<char> _mathOperatorsAllowed = new List<char>() { '+', '-', '*', '/', '%', '^' };

        public ExprResult Solve(String input)
        {
            try
            {
                Stack<double> values = new Stack<double>();
                Stack<char> operators = new Stack<char>();
                char prefix = ' ';

                for (int i = 0; i < input.Length; i++)
                {
                    char inputChar = input[i];

                    // Whitespace
                    if (Char.IsWhiteSpace(inputChar))
                        continue;
                    // Number
                    else if (IsNumber(inputChar))
                    {
                        StringBuilder sbNumber = new StringBuilder();
                        while (i < input.Length && (IsNumber(input[i]) || IsDecimalSign(input[i])))
                        {
                            sbNumber.Append(input[i++]);
                        }
                        i--;
                        values.Push(double.Parse((prefix + sbNumber.ToString()).Trim()));
                        prefix = ' ';
                    }
                    // Operator
                    else if (IsMathOperator(inputChar) && !IsPrefix(inputChar, input, i))
                    {
                        while (operators.Count > 0 && HasSecondOpPrecedence(inputChar, operators.Peek()))
                        {
                            values.Push(EvalOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Push(inputChar);
                    }
                    // Prefix
                    else if (IsMathOperator(inputChar) && IsPrefix(inputChar, input, i))
                    {
                        prefix = inputChar;
                    }
                    // Opening brace  
                    else if (inputChar == '(')
                    {
                        operators.Push(inputChar);
                    }
                    // Closing brace encountered, solve entire brace  
                    else if (inputChar == ')')
                    {
                        while (operators.Peek() != '(')
                        {
                            values.Push(EvalOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Pop();
                    }
                }

                // Entire expression has been parsed at this point, apply remaining  
                // ops to remaining values  
                while (operators.Count > 0)
                {
                    values.Push(EvalOperator(operators.Pop(), values.Pop(), values.Pop()));
                }

                return new ExprResult() { Error = String.Empty, Value = values.Pop() };
            }
            catch(Exception ex)
            {
                return new ExprResult() { Error = "Expression error", Value = 0 };
            }
        }

        private bool IsPrefix(char op, string input, int index)
        {
            if(op == '+' || op == '-')
            {
                return index == 0 || input[index-1] == '(';
            }
            return false;
        }

        private double EvalOperator(char op, double value1, double value2)
        {
            switch (op)
            {
                case '+':
                    return value2 + value1;
                case '-':
                    return value2 - value1;
                case '*':
                    return value2 * value1;
                case '/':
                    if (value1 == 0)
                    {
                        throw new Exception("Dividing by zero is not supproted!");
                    }
                    return value2 / value1;
                case '%':
                    return value2 % value1;
                case '^':
                    return Math.Pow(value2, value1);
            }
            return 0;
        }

        private bool HasSecondOpPrecedence(char op1, char op2)
        {
            if (op2 == '(' || op2 == ')')
                return false;
            if ((op1 == '*' || op1 == '/' || op1 == '%' || op1 == '^') && (op2 == '+' || op2 == '-'))
                return false;
            return true;
        }

        private bool IsNumber(char inputChar)
        {
            return inputChar >= '0' && inputChar <= '9';
        }

        private bool IsDecimalSign(char inputChar)
        {
            return inputChar == '.';
        }

        private bool IsMathOperator(char inputChar)
        {
            return _mathOperatorsAllowed.Contains(inputChar);
        }
    }
}
