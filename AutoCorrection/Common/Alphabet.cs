using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection
{
    public class Alphabet
    {
        private char min;
        private char max;
        public char[] chars {  get; private set; }
        public int size
        {
            get
            {
                return chars.Length;
            }
            private set
            { }
        }
        public Alphabet(char _min, char _max)
        {
            min = _min;
            max = _max;

            chars = new char[max-min +1];
            int index = 0;
            for (char ch = min; ch <= max; ch++)
                chars[index++] = ch;
        }
        public virtual int MapChar(char ch)
        {
            if (ch < min || ch > max) return -1;
            return ch - min;
        }
    }
}
