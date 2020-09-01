using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Tokens
{
    public class AddressRegisterToken : RegisterToken
    {
        public AddressRegisterToken(int line, int pos, Registers register)
            : base(line, pos, register)
        {
        }

        public override string ToString() => $"Token<l.{this.line} c.{this.col}>[AddrReg: {this.Register}]";

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
