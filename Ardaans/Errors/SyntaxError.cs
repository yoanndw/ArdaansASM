using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardaans.Errors
{
    public abstract class SyntaxError
    {
        private string message;

        private int line;
        private int col;

        private string lineContent;

        public SyntaxError(string message, int line, int col, string lineContent)
        {
            this.message = message;

            this.line = line;
            this.col = col;

            this.lineContent = lineContent;
        }

        private string IndicateOnLine()
        {
            var sb = new StringBuilder(this.lineContent + "\n");
            string spaces = string.Concat(Enumerable.Repeat(" ", this.col));

            sb.Append(spaces + "^");

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("================\n");
            sb.Append($"At line {this.line}: {this.message}\n\n");
            sb.Append(this.IndicateOnLine() + "\n================");

            return sb.ToString();
        }
    }

    public class UnknownKeywordError : SyntaxError
    {
        public UnknownKeywordError(string word, int line, int col, string lineContent)
            : base($"Unknown keyword '{word}'", line, col, lineContent) { }
    }

    public class ExpectedNumberError : SyntaxError
    {
        public ExpectedNumberError(int line, int col, string lineContent)
            : base("Expected hex number", line, col, lineContent) { }
    }

    public class UnexpectedTokenForAddressError : SyntaxError
    {
        public UnexpectedTokenForAddressError(int line, int col, string lineContent)
            : base("Expected number or register for the address", line, col, lineContent) { }
    }

    public class UnexpectedCharError : SyntaxError
    {
        public UnexpectedCharError(char c, int line, int col, string lineContent)
            : base($"Unexpected character '{c}'", line, col, lineContent) { }
    }
}
