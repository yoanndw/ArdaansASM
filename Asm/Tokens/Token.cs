using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;

namespace Asm.Tokens
{
    public abstract class Token
    {
        public int Line { get;  protected set; }
        public int Col { get; protected set; }

        public Token(int line, int col)
        {
            this.Line = line;
            this.Col = col;
        }

        public abstract byte GenerateCode();

        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            var o = obj as Token;
            if (o == null)
                return false;

            return this.Line == o.Line && this.Col == o.Col;
        }
    }
}
