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
            // 0x00 => no instruction
            null,

            // 0x01 => mov R V
            new InstructionNode2Ops
            (
                null, 
                new InstructionToken(0, 0, Instructions.Mov),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x02 => mov R &V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mov),
                new RegisterToken(0, 0, Registers.RegA),
                new AddressValueToken(0, 0, 0)
            ),

            // 0x03 => mov &V R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mov),
                new AddressValueToken(0, 0, 0),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x04 => mov R &R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mov),
                new RegisterToken(0, 0, Registers.RegA),
                new AddressRegisterToken(0, 0, Registers.RegA)
            ),

            // 0x05 => mov &R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mov),
                new AddressRegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x06 => mov R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mov),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),
        };

        private List<InstructionNode1Op> ast;
        private byte[] output;

        public CodeGenerator(Input input, List<InstructionNode1Op> ast)
        {
            this.ast = ast;
        }

        public static byte[] GenerateCode(Input input, List<InstructionNode1Op> ast)
        {
            var codegen = new CodeGenerator(input, ast);
            codegen.GenerateCode();

            return codegen.output;
        }

        private void GenerateCode()
        {
            // Foreach node: Node => byte[]
            List<byte[]> opcodes = this.ast.ConvertAll(this.GenerateInstructionCode);

            // Flatten: List<byte[]> => byte[]
            this.output = opcodes.SelectMany(c => c).ToArray();
        }

        private bool GenerateInstructionOpcode(InstructionNode1Op instruction, out byte opcode)
        {
            int iOpcode = Array.FindIndex
            (
                possiblePatterns,
                pat => instruction.HasSamePattern(pat)
            );

            // Not found
            if (iOpcode == -1)
            {
                opcode = 0;
                return false;
            }
            else
            {
                opcode = (byte)iOpcode;
                return true;
            }
        }

        private byte[] GenerateInstructionCode(InstructionNode1Op instruction)
        {
            if (this.GenerateInstructionOpcode(instruction, out byte opcode)) // if pattern exists
            {
                List<byte> code = instruction.GenerateOperandOpcode().ToList(); // { op1, op2 }
                code.Insert(0, opcode); // { instruction, op1, op2 }

                return code.ToArray();
            }

            return null;
        }
    }
}
