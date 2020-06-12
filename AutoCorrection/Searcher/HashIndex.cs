using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class HashIndex : IIndex
    {
		public String[] dictionary { get; private set; }
		public Alphabet alphabet { get; private set; }
		public int[][] hashTable { get; private set; }
		public int[] alphabetMap { get; private set; }
		public int hashSize { get; private set; }
		public int maxLength { get; private set; }
		public HashIndex(String[] dictionary, Alphabet alphabet, int[][] hashTable, int hashSize, int[] alphabetMap, int maxLength)
		{
			this.dictionary = dictionary;
			this.alphabet = alphabet;
			this.hashTable = hashTable;
			this.hashSize = hashSize;
			this.alphabetMap = alphabetMap;
			this.maxLength = maxLength;
		}
	}
}
