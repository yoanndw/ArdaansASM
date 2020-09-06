using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Asm
{
    abstract class LexingException : Exception
    {
        protected string lineContent;

        protected int line;
        protected int col;
        
        public LexingException(string lineContent, int line, int col) 
            : base() 
        {
            this.lineContent = lineContent;

            this.line = line;
            this.col = col;
        }

        public LexingException(string message, string lineContent, int line, int col) 
            : base(message)
        {
            this.lineContent = lineContent;

            this.line = line;
            this.col = col;
        }

        public LexingException(string message, Exception inner, string lineContent, int line, int col) 
            : base(message, inner)
        {
            this.lineContent = lineContent;

            this.line = line;
            this.col = col;
        }

        protected string IndicateOnLine()
        {
            var sb = new StringBuilder();
            sb.Append(this.lineContent + "\n");

            string indicatorSpaces = string.Concat(Enumerable.Repeat(" ", this.col));
            sb.Append(indicatorSpaces + "^");

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Message + "\n\n");
            sb.Append(this.IndicateOnLine());

            return sb.ToString();
        }
    }

    class EOFException : LexingException
    {
        public EOFException(string lineContent, int line, int col)
            : base("Reached end of file", lineContent, line, col)
        {
        }

        public EOFException(string lineContent, int line, int col, Exception inner)
            : base("Reached end of file", inner, lineContent, line, col)
        {
        }
    }

    class UnknwonKeywordException : LexingException
    {
        public UnknwonKeywordException(string lineContent, int line, int col, string word)
            : base($"Unknown keyword '{word}'", lineContent, line, col)
        {
        }

        public UnknwonKeywordException(string lineContent, int line, int col, string word, Exception inner)
            : base($"Unknown keyword '{word}'", inner, lineContent, line, col)
        {
        }
    }

    class UnexpectedCharException : LexingException
    {
        public UnexpectedCharException(string lineContent, int line, int col, char c)
            : base($"Unexpected char '{c}'", lineContent, line, col)
        {
        }

        public UnexpectedCharException(string lineContent, int line, int col, char c, Exception inner)
            : base($"Unexpected char '{c}'", inner, lineContent, line, col)
        {
        }
    }

    class UnclosedBracketsException : LexingException
    {
        public UnclosedBracketsException(string lineContent, int line, int col)
            : base("Unclosed brackets", lineContent, line, col)
        {
        }

        public UnclosedBracketsException(string lineContent, int line, int col, Exception inner)
            : base("Unclosed brackets", inner, lineContent, line, col)
        {
        }
    }

    class UnknownRegisterForAddressException : LexingException
    {
        public UnknownRegisterForAddressException(string lineContent, int line, int col, string reg)
            : base($"Unknown register '{reg}'", lineContent, line, col)
        {
        }

        public UnknownRegisterForAddressException(string lineContent, int line, int col, string reg, Exception inner)
            : base($"Unknown register '{reg}'", inner, lineContent, line, col)
        {
        }
    }

    class UnexpectedSpaceAfterDollarException : LexingException
    {
        public UnexpectedSpaceAfterDollarException(string lineContent, int line, int col)
            : base("Unexpected space after '$'", lineContent, line, col)
        {
        }

        public UnexpectedSpaceAfterDollarException(string lineContent, int line, int col, Exception inner)
            : base("Unexpected space after '$'", inner, lineContent, line, col)
        {
        }
    }

    class UnexpectedDigitsException : LexingException
    {
        public UnexpectedDigitsException(string lineContent, int line, int col)
            : base($"Unexpected digits", lineContent, line, col)
        {
        }

        public UnexpectedDigitsException(string lineContent, int line, int col, Exception inner)
            : base($"Unexpected digits", inner, lineContent, line, col)
        {
        }
    }

    class TooBigNumberException : LexingException
    {
        public TooBigNumberException(string lineContent, int line, int col, string number)
            : base($"'{number}' is too big for one byte (max = 0xFF = 255)", lineContent, line, col)
        {
        }

        public TooBigNumberException(string lineContent, int line, int col, string number, Exception inner)
            : base($"'{number}' is too big for one byte (max = 0xFF = 255)", inner, lineContent, line, col)
        {
        }
    }
}
