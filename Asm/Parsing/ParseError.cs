using System;
using System.Collections.Generic;
using System.Text;

using Asm;
using Asm.Tokens;

namespace Asm.Parsing
{
    public abstract class ParseError
    {
        private string message;

        private int startTokenLine;
        private int startTokenCol;
        private string startTokenLineContent;

        public ParseError(string message, Input input, Token token)
        {
            this.message = message;

            this.startTokenLine = token.Line;
            this.startTokenCol = token.Col;

            this.startTokenLineContent = input.GetLine(this.startTokenLine);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("================\n");
            sb.Append($"At line {this.startTokenLine}: {this.message}\n\n");

            string emphase = this.startTokenLineContent.EmphasizeChar(this.startTokenCol);
            sb.Append(emphase + "\n================");

            return sb.ToString();
        }
    }

    public class ExpectedOneOperandError : ParseError
    {
        public ExpectedOneOperandError(Input input, Token token)
            : base("Expected one operand for this instruction", input, token) { }
    }

    public class ExpectedTwoOperandsError : ParseError
    {
        public ExpectedTwoOperandsError(Input input, Token token)
            : base("Expected two operands for this instruction", input, token) { }
    }
}
