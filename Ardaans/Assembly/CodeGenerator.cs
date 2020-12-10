using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ardaans.Parsing;
using Ardaans.Parsing.Ast;
using Ardaans.Tokens;

namespace Ardaans.Assembly
{
    public class CodeGenerator
    {
        private static InstructionNode1Op[] possiblePatterns =
        {

        };

        private List<InstructionNode1Op> ast;
        private byte[] output;

        public CodeGenerator(Input input, List<InstructionNode1Op> ast)
        {
            this.ast = ast;
        }

        private byte[] GenerateInstructionCode(InstructionNode1Op instruction)
        {
            // Get instruction opcode
            int opcode = Array.FindIndex
            (
                possiblePatterns,
                pat => instruction.HasSamePattern(pat)
            );

            if (opcode != -1) // if pattern exists
            {
                byte bOpcode = (byte)opcode;

                List<byte> code = instruction.GenerateOperandOpcode().ToList(); // { op1, op2 }
                code.Insert(0, bOpcode); // { instruction, op1, op2 }

                return code.ToArray();
            }

            return null;
        }
    }
}
