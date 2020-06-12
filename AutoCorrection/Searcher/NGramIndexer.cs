using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class NGramIndexer : IIndexer
    {
		public NGramIndexer(Alphabet alphabet) : this(alphabet, DEFAULT_N)
		{ }
        public NGramIndexer(Alphabet alphabet, int n)
        {
            this.alphabet = alphabet;
            this.n = n;
        }
		//split on n grams, n =3
		public IIndex CreateIndex(String[] dictionary)
		{
			int mapLength = 1;
			for (int i = 0; i < n; ++i)
				mapLength *= alphabet.size;

			int[] ngramCountMap = new int[mapLength];

			int maxLength = 0;
			// подсчет колличества слов в каждой нграмме 
			foreach (var word in dictionary)
			{
				if (word.Length > maxLength) maxLength = word.Length;

				for (int k = 0; k < word.Length - n + 1; ++k)
				{
					int ngram = GetNGram(alphabet, word, k, n);
					++ngramCountMap[ngram];
				}
			}

			int[][] ngramMap = new int[mapLength][];
			// соотношение нграммы к слову в словаре, нграммМап(номернграммы) : 0 - 0вое слово в словаре, 1 - 5ое слово в словаре итд
			for (int i = 0; i < dictionary.Length; ++i)
			{
				String word = dictionary[i];
				for (int k = 0; k < word.Length - n + 1; ++k)
				{
					int ngram = GetNGram(alphabet, word, k, n);
					if (ngramMap[ngram] == null) ngramMap[ngram] = new int[ngramCountMap[ngram]];
					ngramMap[ngram][--ngramCountMap[ngram]] = i;
				}
			}
			var temp = ngramMap.ToList().Where(w => w != null);
			return new NGramIndex(dictionary, alphabet, ngramMap, n, maxLength);
		}

		public static int GetNGram(Alphabet alphabet, string str, int start, int n)
		{
			int ngram = 0;
			for (int i = start; i < start + n; ++i)
				ngram = ngram * alphabet.size + alphabet.MapChar(str.ToCharArray()[i]);
			return ngram;
		}


		private static  int DEFAULT_N = 3;
        private  Alphabet alphabet;
	    private  int n;
    }
}
