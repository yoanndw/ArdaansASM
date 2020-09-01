using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asm.Errors
{
    public class SyntaxError
    {
        private int line;
        private int col;

        private string lineContent;
        private string message;

        public SyntaxError(int line, int col, string lineContent, string message)
        {
            this.line = line;
            this.col = col;

            this.lineContent = lineContent;
            this.message = $"At line {this.line}: {message}";
        }

        private string IndicateOnLine()
        {
            var sb = new StringBuilder();
            sb.Append(this.lineContent + "\n");

            string indicatorSpaces = string.Concat(Enumerable.Repeat(" ", this.col));
            sb.Append(indicatorSpaces + "^");

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.message + "\n\n");
            sb.Append(this.IndicateOnLine());

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var o = obj as SyntaxError;
            if (o == null)
                return false;

            return this.line == o.line 
                && this.col == o.col 
                && this.lineContent == o.lineContent 
                && this.message == o.message;
        }
    }
}
