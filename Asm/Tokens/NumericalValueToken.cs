using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Tokens
{
    public class NumericalValueToken : Token
    {
        public byte Value { get; protected set; }

        public NumericalValueToken(int line, int pos, byte value)
            : base(line, pos)
        {
            this.Value = value;
        }

        public override byte GenerateCode()
        {
            return this.Value;
        }

        public override string ToString() => $"Token<l.{this.Line} c.{this.Col}>[Num: {this.Value:X2}]";

        public override bool Equals(object obj)
        {
            var o = obj as NumericalValueToken;
            if (o == null)
                return false;

            return base.Equals(obj) && this.Value == o.Value;
        }
    }
}
