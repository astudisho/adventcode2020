using AdventCode.Exercises;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventCode.Tests
{
    public class Day18Tests
    {
        
        [Theory]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Exercise1(string data, int expected)
        {
            var result = Day18.Solve(data.Replace(" ", ""));

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 3 + (4 * 5)", 46)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Exercise2(string data, int expected)
        {
            var result = Day18.Solve(data.Replace(" ", ""), true);

            Assert.Equal(expected, result);
        }
    }
}