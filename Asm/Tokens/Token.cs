using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;

namespace Asm.Tokens
{
    public abstract class Token
    {
        protected int line;
        protected int col;

        public Token(int line, int col)
        {
            this.line = line;
            this.col = col;
        }

        public abstract byte GenerateCode();

        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            var o = obj as Token;
            if (o == null)
                return false;

            return this.line == o.line && this.col == o.col;
        }
    }
}
