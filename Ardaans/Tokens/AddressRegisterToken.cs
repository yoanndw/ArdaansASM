using System;
using System.Collections.Generic;
using System.Text;

namespace Ardaans.Tokens
{
    public class AddressRegisterToken : RegisterToken
    {
        public AddressRegisterToken(int line, int pos, Registers register)
            : base(line, pos, register)
        {
        }

        public override string ToString() => $"Token<l.{this.Line} c.{this.Col}>[AddrReg: {this.Register}]";

        public override string DocRepresentation() => "&R";

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
