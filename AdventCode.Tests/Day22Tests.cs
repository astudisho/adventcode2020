using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AdventCode.Exercises;

namespace AdventCode.Tests
{
    public class Day22Tests
    {
        [Fact]
        public void Exercise1_Test()
        {
            var data = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";
            Day22.Data = data;

            var result = Day22.Exercise1();

            Assert.Equal(306, result);
        }
    }
}
