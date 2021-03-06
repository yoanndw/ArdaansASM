﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Ardaans.Errors;
using Ardaans.Tokens;

namespace Ardaans
{
    public class Lexer
    {
        private Input input;
        private int current;

        private int line;
        private int col;

        private int tokenStartLine;
        private int tokenStartCol;

        private List<Token> tokens;
        private int errorsCount;

        private Lexer(Input input)
        {
            this.input = input;

            this.current = 0;

            this.line = 1;
            this.col = 0;

            this.tokenStartLine = 1;
            this.tokenStartCol = 0;

            this.tokens = new List<Token>();
            this.errorsCount = 0;
        }

        #region Reading
        private string GetCurrentLine()
        {
            return this.input.GetLine(this.line);
        }

        private bool IsAtEnd()
        {
            return this.current >= this.input.Length;
        }

        private char Advance()
        {
            this.col++;
            this.current++;

            return this.input.GetChar(this.current - 1);
        }

        private void NewLine()
        {
            this.line++;
            this.col = 0;
        }

        private char Peek()
        {
            return this.input.GetChar(this.current);
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
        #endregion

        private void LogError(SyntaxError err)
        {
            this.errorsCount++;
            Console.WriteLine(err);
        }

        private void LogUnknownKeywordError(string word)
        {
            var err = new UnknownKeywordError(word, this.tokenStartLine, this.tokenStartCol, this.GetCurrentLine());
            this.LogError(err);
        }

        private void LogExpectedNumberError()
        {
            var err = new ExpectedNumberError(this.tokenStartLine, this.tokenStartCol, this.GetCurrentLine());
            this.LogError(err);
        }

        private void LogUnexpectedTokenForAddressError()
        {
            var err = new UnexpectedTokenForAddressError(this.tokenStartLine, this.tokenStartCol, this.GetCurrentLine());
            this.LogError(err);
        }

        private void LogUnexpectedCharError(char c)
        {
            var err = new UnexpectedCharError(c, this.tokenStartLine, this.tokenStartCol, this.GetCurrentLine());
            this.LogError(err);
        }

        public static List<Token> Tokenize(Input input)
        {
            var lex = new Lexer(input);
            lex.Tokenize();
            return lex.tokens;
        }

        private void Tokenize()
        {
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
                        this.LogUnknownKeywordError(word);
                        continue;
                    }

                    this.tokens.Add(tok);
                }
                else if (c == '#')
                {
                    byte nb;
                    if (this.ExpectNumber(out nb))
                    {
                        var tok = new NumericalValueToken(this.tokenStartLine, this.tokenStartCol, nb);
                        this.tokens.Add(tok);
                    }
                    else
                    {
                        this.LogExpectedNumberError();
                    }
                }
                else if (c == '&')
                {
                    Registers reg;
                    byte nb;
                    if (this.ExpectRegister(out reg))
                    {
                        var tok = new AddressRegisterToken(this.tokenStartLine, this.tokenStartCol, reg);
                        this.tokens.Add(tok);
                    }
                    else if (this.ExpectNumber(out nb))
                    {
                        var tok = new AddressValueToken(this.tokenStartLine, this.tokenStartCol, nb);
                        this.tokens.Add(tok);
                    }
                    else
                    {
                        this.LogUnexpectedTokenForAddressError();
                    }
                }
                else if (c != '\0')
                {
                    this.LogUnexpectedCharError(c);
                    //continue;
                }
            }

            if (this.errorsCount != 0)
            {
                Console.WriteLine(this.errorsCount + " errors found.");
                Console.WriteLine("---");

                throw new SyntaxErrorsException();
            }
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

        public bool ScanNumericalValue(out byte nb)
        {
            nb = 0;

            var sb = new StringBuilder();
            while (!this.IsAtEnd() && this.IsHexDigit(this.Peek()))
            {
                sb.Append(this.Advance());
            }

            if (sb.Length > 0)
            {
                return byte.TryParse(sb.ToString(), NumberStyles.HexNumber, provider: null, out nb);
            }

            return false;
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

            return this.ScanNumericalValue(out number);
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
