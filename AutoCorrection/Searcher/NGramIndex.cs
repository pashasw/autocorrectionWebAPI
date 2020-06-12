using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class NGramIndex :IIndex
    {
		public String[] dictionary { get; private set; }
		public Alphabet alphabet { get; private set; }
		public int[][] ngramMap { get; private set; }
		public int n { get; private set; }
		public int maxLength { get; private set; }
		public NGramIndex(String[] dictionary, Alphabet alphabet, int[][] ngramMap, int n, int maxLength)
		{
			this.dictionary = dictionary;
			this.alphabet = alphabet;
			this.ngramMap = ngramMap;
			this.n = n;
			this.maxLength = maxLength;
		}
	}
}
