using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class NGramSearcher : ISearcher
	{
		private IIndex index;
        public NGramSearcher(IIndex index, int maxDistance, bool prefix)
        {
			d = new DLDistance(DEFAULT_LENGTH);
			this.index = index;
			this.maxDistance = maxDistance;
			this.prefix = prefix;

			dictionary = ((NGramIndex)index).dictionary;
			alphabet = ((NGramIndex)index).alphabet;
			ngramMap = ((NGramIndex)index).ngramMap;
			n = ((NGramIndex)index).n;
		}
		private  int maxDistance;
		private  bool prefix;
		private  String[] dictionary;
		private  Alphabet alphabet;
		private  int[][] ngramMap;
		private  int n;
		public HashSet<Int32> Search(String str)
		{
			var set = new HashSet<Int32>();

			for (int i = 0; i < str.Length - n + 1; ++i)
			{
				int ngram = NGramIndexer.GetNGram(alphabet, str, i, n);

				int[] dictIndexes = ngramMap[ngram];

				if (dictIndexes != null) 
					foreach (var k in dictIndexes)
					{
						DLDistance d = new DLDistance(DEFAULT_LENGTH);
						int distance = d.GetDistance(dictionary[k], str, maxDistance, prefix);
						if (distance <= maxDistance) set.Add(k);
					}
			}
			return set;
		}
		public HashSet<Int32> Search(String str, out List<string> output)
		{
			output = new List<string>();
			var set = new HashSet<Int32>();

			for (int i = 0; i < str.Length - n + 1; ++i)
			{
				int ngram = NGramIndexer.GetNGram(alphabet, str, i, n);

				int[] dictIndexes = ngramMap[ngram];

				if (dictIndexes != null)
					foreach (var k in dictIndexes)
					{
						int distance = d.GetDistance(dictionary[k], str, maxDistance, prefix);
						if (distance <= maxDistance)
						{
							set.Add(k);
							output.Add(dictionary[k]);
						}
					}
			}
			return set;
		}
		private DLDistance d;
		private static int DEFAULT_LENGTH = 255;

	}
}
