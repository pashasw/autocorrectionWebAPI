using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection
{
    public class RussianAlphabet :Alphabet
    {
        public RussianAlphabet() : base('А', 'Я')
        { 
        }
        public override int MapChar(char ch)
        {
            if (ch == 'Ё') ch = 'Е';
            return base.MapChar(ch);
        }
    }
}
