using AdventCode.Exercises;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventCode.Tests
{
    public class Day21Tests
    {
        private readonly string TestData =
            @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";
        public Day21Tests()
        {

        }
        [Fact]
        public void Exercise1Test()
        {
            Day21.Data = TestData;

            var result = Day21.Exercise1();

            Assert.Equal(5, result);
        }
        [Fact]
        public void Exercise2Test()
        {
            Day21.Data = TestData;

            var result = Day21.Exercise2();

            Assert.Equal("mxmxvkd,sqjhc,fvjkl", result);
        }
    }
}
