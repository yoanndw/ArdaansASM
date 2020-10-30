﻿using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class CmpInstruction : TwoOperandsNode
    {
        public CmpInstruction(Token instructionToken, Token operand1, Token operand2)
            : base(instructionToken, operand1, operand2)
        {
        }

        public override bool GenerateCode(Action logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();
            Type operand2Type = this.operand2.GetType();

            if (CodeGenerator.IsValidPattern((operand1Type, operand2Type), CodeGenerator.CmpOpcodes))
            {
                byte opcode = CodeGenerator.CmpOpcodes[(operand1Type, operand2Type)];

                code = CodeGenerator.Generate(opcode, operand1, operand2);
                return true;
            }
            else
            {
                logErrorFunc();
                return false;
            }
        }

        public override string ToString()
            => $"Node<Cmp>{{ {this.operand1}, {this.operand2} }}";
    }
}
