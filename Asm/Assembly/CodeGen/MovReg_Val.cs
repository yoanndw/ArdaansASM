using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class MovReg_Val : InstructionGen
    {
        private RegisterToken reg;
        private NumericalValueToken val;

        public MovReg_Val(RegisterToken reg, NumericalValueToken val)
        {
            this.reg = reg;
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.reg.GenerateCode();
            byte valCode = this.val.GenerateCode();

            byte[] code = { 0x01, regCode, valCode };
            return code;
        }
    }
}
