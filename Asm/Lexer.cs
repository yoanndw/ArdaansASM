﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Asm.Errors;
using Asm.Tokens;

namespace Asm
{
    public class Lexer
    {
        private string source;
        private string[] splittedLines;
        private int current;

        private int line;
        private int col;

        private int tokenStartLine;
        private int tokenStartCol;

        public Lexer(string source)
        {
            this.source = source.Replace("\r", "").ToLower();
            this.splittedLines = this.source.Split('\n');
            this.current = 0;

            this.line = 1;
            this.col = 0;

            this.tokenStartLine = 1;
            this.tokenStartCol = 0;
        }

        private string GetCurrentLine()
        {
            return this.splittedLines[this.line - 1];
        }

        private bool IsAtEnd()
        {
            return this.current >= this.source.Length;
        }

        private char Advance()
        {
            this.col++;
            this.current++;

            return this.source.ElementAt(this.current - 1);
        }

        private void NewLine()
        {
            this.line++;
            this.col = 0;
        }

        private char Peek()
        {
            return this.source.ElementAt(this.current);
        }

        private bool IsLetter(char c)
        {
            return char.IsLetter(c);
        }

        private bool IsHexDigit(char c)
        {
            return Uri.IsHexDigit(c);
        }

        private bool IsSpace(char c)
        {
            return char.IsWhiteSpace(c);
        }

        private bool IsNewLine(char c)
        {
            return c == '\n';
        }

        private byte StringToHex(string s)
        {
            try
            {
                return Convert.ToByte(s, 16);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new Exception("string to hex : no number", e);
            }
            catch (FormatException e)
            {
                throw new UnexpectedDigitsException(this.GetCurrentLine(), this.tokenStartLine, this.tokenStartCol, e);
            }
            catch (OverflowException e)
            {
                throw new TooBigNumberException(this.GetCurrentLine(), this.tokenStartLine, this.tokenStartCol, s, e);
            }
        }

        private void ConsumeSpaces()
        {
            while (!this.IsAtEnd() && this.IsSpace(this.Peek()))
            {
                char c = this.Advance();
                if (this.IsNewLine(c))
                    this.NewLine();
            }
        }

        private void ConsumeComment()
        {
            while (!this.IsAtEnd() && !this.IsNewLine(this.Peek()))
            {
                this.Advance();
            }

            if (!this.IsAtEnd() && this.IsNewLine(this.Peek()))
            {
                // Advance on the '\n'
                this.Advance();
                this.NewLine();
            }
        }

        private void ConsumeSpacesForce()
        {
            this.ConsumeSpaces();
            if (this.IsAtEnd())
            {
                // throw new Exception("consume spaces : EOF");
            }
        }

        private void ConsumeCommentForce()
        {
            this.ConsumeComment();
            if (this.IsAtEnd())
            {
                // throw new Exception("consume comment : EOF");
            }
        }

        private void ProhibitSpaces()
        {
            int oldCurrent = this.current;
            this.ConsumeSpaces();

            // If there was spaces
            if (this.current != oldCurrent || this.IsAtEnd())
            {
                // TODO: change into UnexpectedEOFOrSpaceException
                throw new UnexpectedSpaceAfterDollarException(this.GetCurrentLine(), this.tokenStartLine, this.tokenStartCol);
            }
        }

        /// <summary>
        /// Reads the next character, ignoring spaces, new lines, and comments.
        /// </summary>
        /// <returns>next character</returns>
        private char ReadNext()
        {
            this.ConsumeSpaces();
            if (this.IsAtEnd())
                return '\0';

            char c = this.Advance();
            while (c == ';')
            {
                this.ConsumeComment();
                this.ConsumeSpaces();

                if (!this.IsAtEnd())
                    c = this.Advance();
                else
                    return '\0';
            }

            // c if not a ';' anymore
            return c;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            this.tokenStartLine = this.line;
            this.tokenStartCol = this.col - 1;

            while (!this.IsAtEnd())
            {
                char c = this.ReadNext();

                tokenStartLine = this.line;
                tokenStartCol = this.col - 1; // because `current` points to the next char

                if (this.IsLetter(c))
                {
                    Token tok;

                    string word = this.ScanWord(c);

                    if (InstructionToken.IsKeywordValid(word))
                    {
                        Instructions instr = InstructionToken.GetTypeFromKeyword(word);
                        tok = new InstructionToken(this.tokenStartLine, this.tokenStartCol, instr);
                    }
                    else if (RegisterToken.IsKeywordValid(word))
                    {
                        Registers reg = RegisterToken.GetTypeFromKeyword(word);
                        tok = new RegisterToken(this.tokenStartLine, this.tokenStartCol, reg);
                    }
                    else
                    {
                        continue;
                    }

                    tokens.Add(tok);
                }
                else if (c == '#')
                {
                    byte nb;
                    bool ok = this.ExpectNumber(out nb);

                    if (ok)
                    {
                        var tok = new NumericalValueToken(this.tokenStartLine, this.tokenStartCol, nb);
                        tokens.Add(tok);
                    }
                }
                else if (c == '&')
                {
                    Registers reg;
                    byte nb;
                    if (this.ExpectRegister(out reg))
                    {
                        var tok = new AddressRegisterToken(this.tokenStartLine, this.tokenStartCol, reg);
                        tokens.Add(tok);
                    }
                    else if (this.ExpectNumber(out nb))
                    {
                        var tok = new AddressValueToken(this.tokenStartLine, this.tokenStartCol, nb);
                        tokens.Add(tok);
                    }
                }
                else if (c != '\0')
                {
                    continue;
                }
            }

            return tokens;
        }

        private string ScanWord()
        {
            var sb = new StringBuilder();

            while (!this.IsAtEnd() && this.IsLetter(this.Peek()))
            {
                sb.Append(this.Advance());
            }

            return sb.ToString();
        }

        private string ScanWord(char firstChar)
        {
            var sb = new StringBuilder();
            sb.Append(firstChar);

            while (!this.IsAtEnd() && this.IsLetter(this.Peek()))
            {
                sb.Append(this.Advance());
            }

            return sb.ToString();
        }

        public byte ScanNumericalValue()
        {
            var sb = new StringBuilder();

            while (!this.IsAtEnd() && this.IsHexDigit(this.Peek()))
            {
                sb.Append(this.Advance());
            }

            if (sb.Length > 0)
            {
                return Convert.ToByte(sb.ToString(), 16);
            }
            else
            {
                throw new Exception("expected number, got nothing");
            }
        }

        public bool Expect(char c)
        {
            if (this.IsAtEnd())
                return false;

            return this.Advance() == c;
        }

        public bool ExpectLetter()
        {
            if (this.IsAtEnd())
                return false;

            return this.IsLetter(this.Peek());
        }

        public bool ExpectNumber(out byte number)
        {
            number = 0;
            if (!this.Expect('$'))
            {
                return false;
            }

            if (this.IsAtEnd())
            {
                return false;
            }
            else if (!this.IsHexDigit(this.Peek()))
            {
                return false;
            }

            number = this.ScanNumericalValue();
            return true;
        }

        public bool ExpectRegister(out Registers register)
        {
            register = Registers.RegA;
            if (this.IsAtEnd())
            {
                return false;
            }
            else if (!this.ExpectLetter())
            {
                return false;
            }

            string word = this.ScanWord();
            if (!RegisterToken.IsKeywordValid(word))
            {
                return false;
            }

            register = RegisterToken.GetTypeFromKeyword(word);
            return true;
        }
    }
}
