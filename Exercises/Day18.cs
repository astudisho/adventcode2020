using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventCode.Exercises
{
    public static class Day18
    {
        public static string Data => InputFetcher.FetchInput("Day18.txt");

        public static string[] GetParsedData() => Data.Split("\r\n")
                                                        .Select(x => x.Replace(" ",""))
                                                        .ToArray();

        public static bool ContainSymbol(string value, string symbol) => value.Contains(symbol);

        public static long SolveWithoutPrecedence(string expresion)
        { // Solve expression without parenthesis.
            var numberRegex = new Regex("\\d+");
            var matches = numberRegex.Matches(expresion);
            var operands = new Stack<long>(matches.Select(x => long.Parse(x.ToString())).Reverse());

            var operators = expresion.Where(x => !char.IsDigit(x)).ToList();

            var op1 = operands.Pop();
            var result = operators.Aggregate(op1, (result, operand) =>
            {
                var op2 = operands.Pop();
                var res = operand switch
                {
                    char value when value == '+' => result + op2,
                    char value when value == '-' => result - op2,
                    char value when value == '*' => result * op2,
                    char value when value == '/' => result / op2,
                    _ => 0
                };

                return res;
            });

            return result;
        }

        public static string RemoveParenthesis(string expresion, bool plusPrecedence = false)
        {
            // Replace parenthesis.
            while (ContainSymbol(expresion,")"))
            {
                var closeIndex = expresion.IndexOf(")");
                var aux = expresion.Substring(0, closeIndex + 1);
                var openIndex = aux.LastIndexOf("(");

                var parenthesisSubstring = expresion.Substring(openIndex, closeIndex - openIndex + 1);
                var parenthesisExpresion = parenthesisSubstring.Trim(new[] { '(', ')' });

                long partialResult = 0L;
                partialResult = Solve(parenthesisExpresion, plusPrecedence);
                expresion = expresion.Replace(parenthesisSubstring, partialResult.ToString());
            }

            return expresion;
        }

        public static string RemovePlusPrecedence(string expresion)
        {
            // Replace parenthesis.
            while (ContainSymbol(expresion, "+"))
            {
                var regex = new Regex("\\d+\\+\\d+");
                var match = regex.Match(expresion);

                var plusExpresion = match.Value;

                var partialResult = SolveWithoutPrecedence(plusExpresion);

                expresion = expresion.Replace(plusExpresion, partialResult.ToString());
            }

            return expresion;
        }

        public static long Solve(string expresion, bool plusPrecedence = false)
        {
            if (string.IsNullOrEmpty(expresion)) return 0;
            var result = 0L;

            expresion = RemoveParenthesis(expresion, plusPrecedence);
            if (plusPrecedence) expresion = RemovePlusPrecedence(expresion);

            result = SolveWithoutPrecedence(expresion);

            return result;
        }

        public static long Exercise1()
        {
            long result = 0;

            var parsedData = GetParsedData();
            var results = parsedData.Select(x => (long)Solve(x)).ToList();            
            result = results.Sum();

            return result;
        }

        public static long Exercise2()
        {
            long result = 0;

            var parsedData = GetParsedData();
            var results = parsedData.Select(x => (long)Solve(x, true)).ToList();
            result = results.Sum();

            return result;
        }
    }
}
