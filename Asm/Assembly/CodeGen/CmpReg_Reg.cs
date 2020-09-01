using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class CmpReg_Reg : InstructionGen
    {
        private RegisterToken reg1;
        private RegisterToken reg2;

        public CmpReg_Reg(RegisterToken reg1, RegisterToken reg2)
        {
            this.reg1 = reg1;
            this.reg2 = reg2;
        }

        public override byte[] GenerateCode()
        {
            byte reg1Code = this.reg1.GenerateCode();
            byte reg2Code = this.reg2.GenerateCode();

            byte[] code = { 0x10, reg1Code, reg2Code };
            return code;
        }
    }
}
