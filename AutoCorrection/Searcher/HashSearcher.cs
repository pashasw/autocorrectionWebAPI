using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class HashSearcher : ISearcher
    {
		private IIndex index;
		private DLDistance metric;
		private static int DEFAULT_LENGTH = 255;

		public HashSearcher(IIndex index, int maxDistance, bool prefix)
		{
			this.index = (HashIndex)index;
			this.maxDistance = maxDistance;
			this.metric = new DLDistance(DEFAULT_LENGTH);
			dictionary = ((HashIndex)index).dictionary;
			alphabet = ((HashIndex)index).alphabet;
			hashTable = ((HashIndex)index).hashTable;
			hashSize = ((HashIndex)index).hashSize;
			alphabetMap = ((HashIndex)index).alphabetMap;
		}

		/**
		 * Производит поиск по всем словам из словаря, сигнатурный хеш которых отличается от исходного не более чем в
		 * maxDistance битах.
		 */
		public HashSet<Int32> Search(String str)
		{
			var result = new HashSet<Int32>();

			int stringHash = HashIndexer.MakeHash(alphabet, alphabetMap, str);
			Populate(str, stringHash, result);
			if (maxDistance > 0) HashPopulate(str, stringHash, 0, hashSize, result, maxDistance - 1);
			return result;
		}
		public HashSet<Int32> Search(String str, out List<string> output)
		{
			var result = new HashSet<Int32>();
			int stringHash = HashIndexer.MakeHash(alphabet, alphabetMap, str);
			Populate(str, stringHash, result, out output);
			if (maxDistance > 0) HashPopulate(str, stringHash, 0, hashSize, result, maxDistance - 1);
			return result;
		}
		/**
		 * Вносит перебирает все хеши, отличающиеся от исходного на 1 бит на какой-либо позиции. При добавлении или удалении
		 * символа из слова в сигнатурном хеше изменяются 0 или 1 бит, при замене символа - от 0 до 2 бит.
		 */
		private void HashPopulate(String query, int hash, int start, int hashSize, HashSet<Int32> set, int depth)
		{
			for (int i = start; i < hashSize; ++i)
			{
				int queryHash = hash ^ (1 << i);
				Populate(query, queryHash, set);
				if (depth > 0) HashPopulate(query, queryHash, i + 1, hashSize, set, depth - 1);
			}
		}

		/**
		 * Перебирает все слова в словаре с заданным хешем, записывая в результат только слова, удоволетворяющие ограничению
		 * при данной метрике.
		 */
		private void Populate(String query, int queryHash, HashSet<Int32> set)
		{
			int[] hashBucket = hashTable[queryHash];
			if (hashBucket != null) foreach (int dictionaryIndex in hashBucket)
				{
					String word = dictionary[dictionaryIndex];
					if (metric.GetDistance(query, word, maxDistance) <= maxDistance) set.Add(dictionaryIndex);
				}
		}
		private void Populate(String query, int queryHash, HashSet<Int32> set, out List<string> output)
		{
			output = new List<string>();
			int[] hashBucket = hashTable[queryHash];
			if (hashBucket != null) foreach (int dictionaryIndex in hashBucket)
				{
					String word = dictionary[dictionaryIndex];
					if (metric.GetDistance(query, word, maxDistance) <= maxDistance)
					{
						set.Add(dictionaryIndex);
						output.Add(word);
					}
				}
		}

		private  int maxDistance;
		private  String[] dictionary;
		private  Alphabet alphabet;
		private  int[][] hashTable;
		private  int hashSize;
		private  int[] alphabetMap;
	}
}
