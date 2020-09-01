using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class IncReg : InstructionGen
    {
        private RegisterToken reg;

        public IncReg(RegisterToken reg)
        {
            this.reg = reg;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.reg.GenerateCode();

            byte[] code = { 0x11, regCode };
            return code;
        }
    }
}
