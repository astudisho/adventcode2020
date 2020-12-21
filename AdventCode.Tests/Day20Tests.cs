using AdventCode.Exercises;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace AdventCode.Tests
{
    public class Day20Tests
    {
        [Fact]
        public void Should_Be_Same_Hash_Code()
        {
            var side1 = "10100";
            var side2 = "00101";

            var obj1 = side1.Select(x => x == '1').ToArray().ToString();
            var obj2 = side2.Reverse().Select(x => x == '1').ToArray().ToString();

            Assert.Equal(obj1.GetHashCode(), obj2.GetHashCode());
        }

        [Fact]
        public void Exercise1()
        {
            var data = InputFetcher.FetchInput("day20");
        }
    }
}
