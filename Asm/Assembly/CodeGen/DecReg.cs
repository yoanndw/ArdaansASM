using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class DecReg : InstructionGen
    {
        private RegisterToken reg;

        public DecReg(RegisterToken reg)
        {
            this.reg = reg;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.reg.GenerateCode();

            byte[] code = { 0x12, regCode };
            return code;
        }
    }
}
