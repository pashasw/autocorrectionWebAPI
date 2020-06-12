using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    public class HashIndexer : IIndexer
	{
		public HashIndexer(Alphabet alphabet)
		{
			this.alphabet = alphabet;
			alphabetMap = MakeAlphabetMap(alphabet);
		}
		public IIndex CreateIndex(String[] dictionary)
		{
			int maxLength = 0;

			// Подсчет количества слов для каждой группы хеша (для уменьшения потребления памяти)
			//[хэш] = count of words in dictionary
			int[] hashCountTable = new int[1 << HASH_SIZE];
			foreach (var word in dictionary)
			{
				int hash = MakeHash(alphabet, alphabetMap, word);
				++hashCountTable[hash];
				if (word.Length > maxLength) maxLength = word.Length;
			}

			// Заполняем хеш-таблицу. Каждый элемент - массив индексов слов в словаре, соответствующих хешу.
			// hashTable[hash] - это хэши которые были сгенерены ранее, создаем для каждого элемента массив, в который кладем 
			// индекс слова в словаре
			int[][] hashTable = new int[1 << HASH_SIZE][];
			for (int i = 0; i < dictionary.Length; ++i)
			{
				int hash = MakeHash(alphabet, alphabetMap, dictionary[i]);
				if (hashTable[hash] == null) hashTable[hash] = new int[hashCountTable[hash]];
				hashTable[hash][--hashCountTable[hash]] = i;
			}

			return new HashIndex(dictionary, alphabet, hashTable, HASH_SIZE, alphabetMap, maxLength);
		}

		/**
		 * Вычисляет сигнатурный хеш для слова.
		 */
		public static int MakeHash(Alphabet alphabet, int[] alphabetMap, String word)
		{
			int result = 0;
			for (int i = 0; i < word.Length; ++i)
			{
				int group = alphabetMap[alphabet.MapChar(word.ToCharArray()[i])];
				result |= 1 << group;
			}
			return result;
		}

		/**
		 * Производит равномерное распределение символов алфавита по группам хеша.
		 */
		private static int[] MakeAlphabetMap(Alphabet alphabet)
		{
			int[] result = new int[alphabet.size];
			double sourceAspect = (double)result.Length / HASH_SIZE;
			double aspect = sourceAspect;
			int[] map = new int[HASH_SIZE];
			//создаем массив мап, в элементах которого указываем сколько букв храним,
			for (int i = 0; i < HASH_SIZE; ++i)
			{
				int step = (int)Math.Round(aspect);
				double diff = aspect - step;
				map[i] = step;
				aspect = sourceAspect + diff;
			}
			int resultIndex = 0; 
			// связываем буквы с массивом групп 
			for (int i = 0; i < map.Length; ++i)
				for (int j = 0; j < map[i]; ++j)
					if (resultIndex < result.Length) result[resultIndex++] = i;
			return result;
		}
		private static  int HASH_SIZE = 16;
		private  Alphabet alphabet;
		private  int[] alphabetMap;
	}
}
