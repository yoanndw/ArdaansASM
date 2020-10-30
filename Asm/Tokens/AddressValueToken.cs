using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Tokens
{
    public class AddressValueToken : NumericalValueToken
    {
        public AddressValueToken(int line, int pos, byte value)
            : base(line, pos, value)
        {
        }

        public override string ToString() => $"Token<l.{this.Line} c.{this.Col}>[Addr: {this.Value:X2}]";

        public override string DocRepresentation() => "&V";

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
