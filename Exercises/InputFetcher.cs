using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventCode.Exercises
{
    static class InputFetcher
    {
        internal static string FetchInput(string fileName)
        {
            var fullPath = AppDomain.CurrentDomain.BaseDirectory + $"..\\..\\..\\Exercises\\Inputs\\{fileName}";
            using (var sr = new StreamReader(fullPath))
            {
                var text = sr.ReadToEnd();
                return text;
            }
        }
    }
}
