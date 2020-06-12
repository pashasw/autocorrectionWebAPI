using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrection
{
    public class Dictionary
    {
		public static String[] LoadDictionary(string filename) 
		{
			var lines = new List<String>();
			using (var reader = new StreamReader(filename))
			{
				String line;
				while ((line = reader.ReadLine()) != null)
					lines.Add(line);
				reader.Close();
			}
			return lines.ToArray();
		}
		public static void SaveDictionary(String[] dictionary, String filename) 
		{
			using (var writer = new StreamWriter(filename))
			{
				foreach (var line in dictionary)
				{
					writer.WriteLine(line);
				}
				writer.Close();
			}
		}
		public static String[] CreateDictionary(StreamReader reader, NormalizationForm normalizer)
		{
			var dictionarySet = new HashSet<String>();
			var stringBuilder = new StringBuilder();

			int sym;
			while ((sym = reader.Read()) >= 0)
			{
				char ch = (char)sym;
				if (Char.IsLetter(ch))
					stringBuilder.Append(ch);
				else 
				{
					if (stringBuilder.Length > 0)
					{
						string word = stringBuilder.ToString().Normalize(normalizer);
						if (word.Length > 0)
							dictionarySet.Add(word);
						stringBuilder.Length = 0;
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				String word = stringBuilder.ToString().Normalize(normalizer);
				if (word.Length > 0)
					dictionarySet.Add(word);
			}
			String[] dictionary = dictionarySet.ToArray();
			Array.Sort(dictionary);
			return dictionary;
		}
	}
}
