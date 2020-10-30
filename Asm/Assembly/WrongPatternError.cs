using System;
using System.Collections.Generic;
using System.Text;

using Asm.Parsing.Ast;
using Asm.Tokens;

namespace Asm.Assembly
{
    public class WrongPatternError
    {
        private string message;

        private int line;
        private int col;

        private string lineContent;

        private Instructions instruction;

        public WrongPatternError(string message, Input input, OneOperandNode instruction)
        {
            this.message = message;

            this.line = instruction.Line;
            this.col = instruction.Col;

            this.lineContent = input.GetLine(this.line);

            this.instruction = instruction.Instruction;
        }

        private string Patterns()
        {
            string sInstr = this.instruction.ToString().ToLower();

            var patterns = CodeGenerator.Patterns[sInstr];

            var sb = new StringBuilder();
            foreach (string pat in patterns)
            {
                sb.AppendLine(sInstr + " " + pat);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("================\n");
            sb.Append($"At line {this.line}: {this.message}\n\n");

            string emphase = this.lineContent.EmphasizeChar(this.col);
            sb.Append(emphase);

            string patterns = this.Patterns();
            sb.Append(patterns + "\n================");

            return sb.ToString();
        }
    }
}
