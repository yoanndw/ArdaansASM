using System;
using System.Collections.Generic;
using System.Text;

using Asm.Parsing.Ast;
using Asm.Tokens;

namespace Asm.Assembly
{
    public class WrongPatternError
    {
        private int line;
        private int col;

        private string lineContent;

        public WrongPatternError(OneOperandNode instruction)
        {
            this.line = instruction.Line;
            this.col = instruction.Col;

            this.lineContent = instruction.LineContent;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("================\n");
            sb.Append($"At line {this.line}: Wrong instruction pattern\n\n");

            string emphase = this.lineContent.EmphasizeChar(this.col);
            sb.Append(emphase + "\n================");

            return sb.ToString();
        }
    }
}
