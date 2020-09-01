using Microsoft.VisualStudio.TestTools.UnitTesting;

using Asm;
using Asm.Tokens;
using System.Collections.Generic;
using Asm.Errors;

namespace AsmTests
{
    [TestClass]
    public class LexerTest
    {
        /*[TestMethod]
        public void Test()
        {
            Assert.AreEqual(new InstructionToken(1, 0, Instructions.Add), new InstructionToken(1, 0, Instructions.Add));
        }*/

        [TestMethod]
        public void TestInstructions()
        {
            string p = @"mov add sub div
mul cmp inc dec jmp jeq jne jsm jns";
            var lex = new Lexer(p);

            var expected = new List<Token>
            {
                new InstructionToken(1, 0, Instructions.Mov),
                new InstructionToken(1, 4, Instructions.Add),
                new InstructionToken(1, 8, Instructions.Sub),
                new InstructionToken(1, 12, Instructions.Div),
                new InstructionToken(2, 0, Instructions.Mul),
                new InstructionToken(2, 4, Instructions.Cmp),
                new InstructionToken(2, 8, Instructions.Inc),
                new InstructionToken(2, 12, Instructions.Dec),
                new InstructionToken(2, 16, Instructions.Jmp),
                new InstructionToken(2, 20, Instructions.Jeq),
                new InstructionToken(2, 24, Instructions.Jne),
                new InstructionToken(2, 28, Instructions.Jsm),
                new InstructionToken(2, 32, Instructions.Jns)
            };

            CollectionAssert.AreEqual(expected, lex.Tokenize());
        }

        [TestMethod]
        public void TestNumbersAndAddresses()
        {
            string p = @"$5 $05 $A $0A [$5] [
$05
]";
            var lex = new Lexer(p);

            var expected = new List<Token>
            {
                new NumericalValueToken(1, 0, 0x05),
                new NumericalValueToken(1, 3, 0x05),
                new NumericalValueToken(1, 7, 0x0A),
                new NumericalValueToken(1, 10, 0x0A),
                new AddressValueToken(1, 14, 0x05),
                new AddressValueToken(1, 19, 0x05)
            };

            CollectionAssert.AreEqual(expected, lex.Tokenize());
        }

        [TestMethod]
        public void TestEmpty()
        {
            var lex = new Lexer("");

            CollectionAssert.AreEqual(new List<Token>(), lex.Tokenize());
        }

        [TestMethod]
        public void TestEmptyComment()
        {
            var lex = new Lexer(";");

            CollectionAssert.AreEqual(new List<Token>(), lex.Tokenize());
        }

        [TestMethod]
        public void TestCodeAndComments()
        {
            string p = @"mov ;$A
a
;long
; comment bla
; blu

; blo
b ; end";
            var lex = new Lexer(p);

            var expected = new List<Token>
            {
                new InstructionToken(1, 0, Instructions.Mov),
                new RegisterToken(2, 0, Registers.RegA),
                new RegisterToken(8, 0, Registers.RegB),
            };

            CollectionAssert.AreEqual(expected, lex.Tokenize());
        }

        [TestMethod]
        public void TestUnexpectedChar()
        {
            string p = ". , : / * -+";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unexpected char '.'"),
                new SyntaxError(1, 2, p, "Unexpected char ','"),
                new SyntaxError(1, 4, p, "Unexpected char ':'"),
                new SyntaxError(1, 6, p, "Unexpected char '/'"),
                new SyntaxError(1, 8, p, "Unexpected char '*'"),
                new SyntaxError(1, 10, p, "Unexpected char '-'"),
                new SyntaxError(1, 11, p, "Unexpected char '+'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnknownKeyword()
        {
            string p = "mzv dd";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unknown keyword 'mzv'"),
                new SyntaxError(1, 4, p, "Unknown keyword 'dd'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnknownRegisterForAddress()
        {
            string p = "[e]";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unknown register 'e'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestEOFAfterDollar()
        {
            string p = "$";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unexpected space after '$'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestSpaceAfterDollar()
        {
            string p = "$ ";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unexpected space after '$'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestSpaceAfterDollarThenNumber()
        {
            string p = "$ 05";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, p, "Unexpected space after '$'"),
                new SyntaxError(1, 2, p, "Unexpected char '0'"),
                new SyntaxError(1, 3, p, "Unexpected char '5'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnexpectedDigits()
        {
            string p = "$G";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "$g", "Unexpected digits"),
                new SyntaxError(1, 1, "$g", "Unknown keyword 'g'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestTooBigNumber()
        {
            string p = "$100";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "$100", "'0x100' is too big for one byte (max = 0xFF = 255)")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedEmptyBracket()
        {
            string p = "[ ";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[ ", "Unclosed brackets")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedBracketWithNumber()
        {
            string p = "[ $05";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[ $05", "Unclosed brackets")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedBracketWithUnexpectedChar()
        {
            string p = "[ .";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[ .", "Unexpected char '.'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestClosedBracketsWithUnexpectedChar()
        {
            string p = "[ .]";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[ .]", "Unexpected char '.'"),
                new SyntaxError(1, 3, "[ .]", "Unexpected char ']'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedBracketWithDollar()
        {
            string p = "[$";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[$", "Unexpected space after '$'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedBracketWithUnexpectedDigit()
        {
            string p = "[$G";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[$g", "Unexpected char 'g'")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }

        [TestMethod]
        public void TestUnclosedBracketWithTooBigNumber()
        {
            string p = "[$100";
            var lex = new Lexer(p);

            var expectedErrors = new List<SyntaxError>
            {
                new SyntaxError(1, 0, "[$100", "Unclosed brackets")
            };

            var tokens = lex.Tokenize();

            CollectionAssert.AreEqual(expectedErrors, lex.ErrorLogger.Errors);
            CollectionAssert.AreEqual(new List<Token>(), tokens);
        }
    }
}
