using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class MovAddr_Reg : InstructionGen
    {
        private AddressValueToken addr;
        private RegisterToken reg;

        public MovAddr_Reg(AddressValueToken addr, RegisterToken reg)
        {
            this.addr = addr;
            this.reg = reg;
        }

        public override byte[] GenerateCode()
        {
            byte addrCode = this.addr.GenerateCode();
            byte regCode = this.reg.GenerateCode();

            byte[] code = { 0x03, addrCode, regCode };
            return code;
        }
    }
}
