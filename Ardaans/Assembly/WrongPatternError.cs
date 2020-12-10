using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Parsing.Ast;
using Ardaans.Tokens;

namespace Ardaans.Assembly
{
    public class WrongPatternError
    {
        private int line;
        private int col;

        private string lineContent;

        private string instruction;

        public WrongPatternError(InstructionNode1Op instruction)
        {
            this.line = instruction.Line;
            this.col = instruction.Col;

            this.lineContent = instruction.LineContent;

            this.instruction = instruction.Instruction.ToString().ToLower();
        }

        private string Patterns()
        {
            var sb = new StringBuilder("Possible patterns are:\n");

            var patterns = CodeGenerator.Patterns[this.instruction];
            foreach (string pat in patterns)
            {
                sb.AppendLine(this.instruction + "\t" + pat);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("================\n");
            sb.Append($"At line {this.line}: Wrong instruction pattern\n\n");

            string emphase = this.lineContent.EmphasizeChar(this.col);
            sb.Append(emphase + "\n\n");

            string patterns = this.Patterns();
            sb.Append(patterns + "================");


            return sb.ToString();
        }
    }
}
