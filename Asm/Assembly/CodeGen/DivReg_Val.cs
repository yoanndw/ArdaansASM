using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class DivReg_Val : InstructionGen
    {
        private RegisterToken reg;
        private NumericalValueToken val;

        public DivReg_Val(RegisterToken reg, NumericalValueToken val)
        {
            this.reg = reg;
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.reg.GenerateCode();
            byte valCode = this.val.GenerateCode();

            byte[] code = { 0x0D, regCode, valCode };
            return code;
        }
    }
}
