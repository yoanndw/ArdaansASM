using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Errors
{
    public class ErrorLogger
    {
        private List<SyntaxError> errors;
        public List<SyntaxError> Errors => this.errors;

        public ErrorLogger()
        {
            this.errors = new List<SyntaxError>();
        }

        public bool IsEmpty()
        {
            return this.errors.Count == 0;
        }

        public void LogError(SyntaxError err)
        {
            this.errors.Add(err);
        }

        public void LogError(int line, int col, string lineContent, string message)
        {
            this.LogError(new SyntaxError(line, col, lineContent, message));
        }

        public void PrintErrors()
        {
            Console.WriteLine($"{this.errors.Count} syntax error(s)");
            Console.WriteLine("-------------");
            foreach (var err in this.errors)
            {
                Console.WriteLine(err);
                Console.WriteLine("-------------");
            }
        }
    }
}
