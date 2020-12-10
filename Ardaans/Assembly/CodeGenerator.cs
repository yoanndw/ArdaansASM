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

            // 0x07 => add R V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Add),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x08 => add R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Add),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x09 => sub R V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Sub),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x0A => sub R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Sub),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x0B => mul R V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mul),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x0C => mul R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Mul),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x0D => div R V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Div),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x0E => div R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Div),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x0F => cmp R V
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Cmp),
                new RegisterToken(0, 0, Registers.RegA),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x10 => cmp R R
            new InstructionNode2Ops
            (
                null,
                new InstructionToken(0, 0, Instructions.Cmp),
                new RegisterToken(0, 0, Registers.RegA),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x11 => inc R
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Inc),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x12 => dec R
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Dec),
                new RegisterToken(0, 0, Registers.RegA)
            ),

            // 0x13 => jmp V
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Jmp),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x14 => jeq V
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Jeq),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x15 => jne V
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Jne),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x16 => jsm V
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Jsm),
                new NumericalValueToken(0, 0, 0)
            ),

            // 0x17 => jns V
            new InstructionNode1Op
            (
                null,
                new InstructionToken(0, 0, Instructions.Jns),
                new NumericalValueToken(0, 0, 0)
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

        public static bool GenerateInstructionOpcode(InstructionNode1Op instruction, out byte opcode)
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
            if (GenerateInstructionOpcode(instruction, out byte opcode)) // if pattern exists
            {
                List<byte> code = instruction.GenerateOperandOpcode().ToList(); // { op1, op2 }
                code.Insert(0, opcode); // { instruction, op1, op2 }

                return code.ToArray();
            }

            return null;
        }
    }
}
