using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventCode.Exercises
{
    public static class InputFetcher
    {
        public static string FetchInput(string fileName, string path = "..\\..\\..\\Exercises\\Inputs\\")
        {
            var fullPath = AppDomain.CurrentDomain.BaseDirectory + $"{path}{fileName}";
            using (var sr = new StreamReader(fullPath))
            {
                var text = sr.ReadToEnd();
                return text;
            }
        }
    }
}
