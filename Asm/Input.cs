using System;
using System.Collections.Generic;
using System.Text;

namespace Asm
{
    public class Input
    {
        private string source;
        private string[] lines;

        public Input(string source)
        {
            this.source = source.Replace("\r", "").ToLower();
            this.lines = this.source.Split("\n");
        }

        public int Length
        {
            get => this.source.Length;
        }

        public char GetChar(int index) => this.source[index];

        /// <summary>
        /// Returns a specified line between 1 and number of lines
        /// </summary>
        /// <param name="line">Number of the line</param>
        /// <returns>The specified line</returns>
        public string GetLine(int line) => this.lines[line - 1];
    }
}
