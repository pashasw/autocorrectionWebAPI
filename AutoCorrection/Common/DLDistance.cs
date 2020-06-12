using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace AutoCorrection
{
    public class DLDistance
    {
        //double deleteCost = 1;
        //double insertCost = 1;
        //double replaceCost = 1;
        //double swapCost = 1;
        public DLDistance() { }
        //public DLDistance(double _delete, double _insert, double _replace, double _swap )
        //{
        //    deleteCost = _delete;
        //    insertCost = _insert;
        //    replaceCost = _replace;
        //    swapCost = _swap;
        //}
        private int[] currentRow;
        private int[] previousRow;
        private int[] transpositionRow;
        public DLDistance(int maxLength)
        {
            currentRow = new int[maxLength + 1];
            previousRow = new int[maxLength + 1];
            transpositionRow = new int[maxLength + 1];
        }


        public double Calculate(string source, string target)
        {
            if (source.Length == 0)
                return Convert.ToDouble(target.Length);
            //* insertCost;
            if (target.Length == 0)
                return Convert.ToDouble(source.Length);
            //* deleteCost;

            if (source.Length > target.Length)
            {
                var tmp = source;
                source = target;
                target = tmp;
            }
            var table = new double[source.Length + 1, target.Length + 1];
            for (int i = 0; i < source.Length + 1; i++)
                for (int j = 0; j < target.Length + 1; j++)
                    table[i, j] = new double();

            for (int i = 0; i < source.Length + 1; i++)
                table[i, 0] = i;

            for (int j = 0; j < target.Length + 1; j++)
                table[0, j] = j;

            var a = source.ToCharArray();
            var b = target.ToCharArray();
            int cost = 0;
            for (int i = 1; i < source.Length + 1; i++)
            {
                for (int j = 1; j < target.Length + 1; j++)
                {
                    if (a[i - 1] == b[j - 1])
                        cost = 0;
                    else
                        cost = 1;
                    table[i, j] = Math.Min(table[i - 1, j] + 1,
                        Math.Min(table[i - 1, j - 1] + cost, table[i, j - 1] + 1));
                    if (i > 1 && j > 1
                        && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                        table[i, j] = Math.Min(table[i, j], table[i - 2, j - 2] + 1);
                }
            }
            return table[source.Length, target.Length];


            //    var sourceIndexByCharacter = new char[2,source.Length];
            //    for (int i = 0; i < 2; i++)
            //        for (int j = 0; j < source.Length; j++)
            //            sourceIndexByCharacter[i, j] = new char();

            //    if (source.Substring(0, 1) != target.Substring(0, 1))
            //        table[0, 0] = Math.Min(replaceCost, (deleteCost + insertCost));

            //    sourceIndexByCharacter[0, 0] = (source.ToCharArray())[0];
            //    sourceIndexByCharacter[1,0] = '0';

            //    double deleteD;
            //    double insertD;
            //    double matchD;

            //    for (int i = 1; i < source.Length -1; i++)
            //    {
            //        deleteD = table[i - 1, 0] + deleteCost;
            //        insertD = ((i + 1) * deleteCost) + insertCost;

            //        if (source.Substring(i + 1, 1) == target.Substring(0, 1))
            //            matchD = (i * deleteCost) + 0;
            //        else
            //            matchD = (i * deleteCost) + replaceCost;
            //        table[i, 0] = Math.Min(deleteD, Math.Min(insertD, matchD));
            //    }

            //    for (int j = 1; j < target.Length -1; j++)
            //    {
            //        deleteD = table[0, j -1] + insertCost;
            //        insertD = ((j + 1) * insertCost) + deleteCost;

            //        if (source.Substring(0, 1) == target.Substring(j+1, 1))
            //            matchD = (j * insertCost) + 0;
            //        else
            //            matchD = (j * insertCost) + replaceCost;
            //        table[0, j] = Math.Min(deleteD, Math.Min(insertD, matchD));
            //    }

            //    for (int i = 1; i < source.Length -1 ; i++)
            //    {
            //        int maxSourceLetterMatchInder;
            //        if (source.Substring(i + 1, 1) == target.Substring(0, 1))
            //            maxSourceLetterMatchInder = 0;
            //        else
            //            maxSourceLetterMatchInder = -1;

            //        for (int j = 1; j < target.Length -1 ; j++)
            //        {
            //            int candidadeSwapIndex = -1;

            //            for (int k = 0; k < source.Length - 1; k++)
            //                if (sourceIndexByCharacter[0, k] == (target.ToCharArray())[j + 1])
            //                    candidadeSwapIndex = (int)Char.GetNumericValue(sourceIndexByCharacter[1, k]);
            //            int jSwap = maxSourceLetterMatchInder;

            //            deleteD = table[i - 1, j] + deleteCost;
            //            insertD = table[i, j - 1] + insertCost;
            //            matchD = table[i - 1, j - 1];

            //            if (source.Substring(i + 1, 1) != target.Substring(j + 1, 1))
            //                matchD = matchD + replaceCost;
            //            else
            //                maxSourceLetterMatchInder = j;

            //            double swapD;
            //            if ((candidadeSwapIndex != -1) && jSwap != -1)
            //            {
            //                var iSwap = candidadeSwapIndex;

            //                double preSwapCost;
            //                if (iSwap == 0 && jSwap == 0)
            //                    preSwapCost = 0;
            //                else
            //                    preSwapCost = table[Math.Max(0, iSwap - 1), Math.Max(0, jSwap - 1)];
            //                swapD = preSwapCost + ((i - iSwap - 1) * deleteCost) + ((j - jSwap - 1) * insertCost) + swapCost;

            //            }
            //            else
            //                swapD = 500;
            //            table[i, j] = Math.Min(Math.Min(Math.Min(deleteD, insertD), matchD), swapD);
            //        }

            //        sourceIndexByCharacter[0, i] = (source.ToCharArray())[i + 1];
            //        sourceIndexByCharacter[1, i] = (char)i;
            //    }
            //    return table[source.Length, target.Length];
            //}
        }

        public double GetDistance(string source, string target, int max)
        {
            int firstLength = source.Length;
            int secondLength = target.Length;

            if (firstLength == 0)
                return secondLength;
            else if (secondLength == 0) return firstLength;

            if (firstLength > secondLength)
            {
                var tmp = source;
                source = target;
                target = tmp;
                firstLength = secondLength;
                secondLength = target.Length;
            }

            if (max < 0) max = secondLength;
            if (secondLength - firstLength > max) return max + 1;

            if (firstLength > currentRow.Length)
            {
                currentRow = new int[firstLength + 1];
                previousRow = new int[firstLength + 1];
                transpositionRow = new int[firstLength + 1];
            }

            for (int i = 0; i <= firstLength; i++)
                previousRow[i] = i;

            char lastSecondCh = '0';
            for (int i = 1; i <= secondLength; i++)
            {
                char secondCh = target.ToCharArray()[i - 1];
                currentRow[0] = i;

                // Вычисляем только диагональную полосу шириной 2 * (max + 1)
                int from = Math.Max(i - max - 1, 1);
                int to = Math.Min(i + max + 1, firstLength);

                char lastFirstCh = '0';
                for (int j = from; j <= to; j++)
                {
                    char firstCh = source.ToCharArray()[j - 1];

                    // Вычисляем минимальную цену перехода в текущее состояние из предыдущих среди удаления, вставки и
                    // замены соответственно.
                    int cost = firstCh == secondCh ? 0 : 1;
                    int value = Math.Min(Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1), previousRow[j - 1] + cost);

                    // Если вдруг была транспозиция, надо также учесть и её стоимость.
                    if (firstCh == lastSecondCh && secondCh == lastFirstCh)
                        value = Math.Min(value, transpositionRow[j - 2] + cost);

                    currentRow[j] = value;
                    lastFirstCh = firstCh;
                }
                lastSecondCh = secondCh;

                int[] tempRow = transpositionRow;
                transpositionRow = previousRow;
                previousRow = currentRow;
                currentRow = tempRow;
            }
            return previousRow[firstLength];

        }
    

        public int GetPrefixDistance(string first, string second, int max, bool prefix)
        {
            return prefix ? GetPrefixDistance(first, second, max) : (int)GetDistance(first, second, max);
        }
        public int GetPrefixDistance(string source, string prefix, int max)
        {
            int prefixLength = prefix.Length;
            if (max < 0) max = prefixLength;
            int stringLength = Math.Min(source.Length, prefix.Length + max);

            if (prefixLength == 0)
                return 0;
            else if (stringLength == 0) return prefixLength;

            if (stringLength < prefixLength - max) return max + 1;

            if (prefixLength > currentRow.Length)
            {
                currentRow = new int[prefixLength + 1];
                previousRow = new int[prefixLength + 1];
                transpositionRow = new int[prefixLength + 1];
            }

            for (int i = 0; i <= prefixLength; i++)
                previousRow[i] = i;

            int distance = Int32.MaxValue;

            char lastStringCh = '0';
            for (int i = 1; i <= stringLength; i++)
            {
                char stringCh = source.ToCharArray()[i - 1];
                currentRow[0] = i;

                // Вычисляем только диагональную полосу шириной 2 * (max + 1)
                int from = Math.Max(i - max - 1, 1);
                int to = Math.Min(i + max + 1, prefixLength);

                char lastPrefixCh = '0';
                for (int j = from; j <= to; j++)
                {
                    char prefixCh = prefix.ToCharArray()[j - 1];

                    // Вычисляем минимальную цену перехода в текущее состояние из предыдущих среди удаления, вставки и
                    // замены соответственно.
                    int cost = prefixCh == stringCh ? 0 : 1;
                    int value = Math.Min(Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1), previousRow[j - 1] + cost);

                    // Если вдруг была транспозиция, надо также учесть и её стоимость.
                    if (prefixCh == lastStringCh && stringCh == lastPrefixCh)
                        value = Math.Min(value, transpositionRow[j - 2] + cost);

                    currentRow[j] = value;
                    lastPrefixCh = prefixCh;
                }
                lastStringCh = stringCh;

                // Вычисляем минимальное расстояние от заданного префикса ко всем префиксам строки, отличающимся от
                // заданного не более чем на max
                if (i >= prefixLength - max && i <= prefixLength + max && currentRow[prefixLength] < distance)
                    distance = currentRow[prefixLength];

                int[] tempRow = transpositionRow;
                transpositionRow = previousRow;
                previousRow = currentRow;
                currentRow = tempRow;
            }
            return distance;
        }
        public int GetDistance(string first, string second, int max, bool prefix)
        {
            return prefix ? GetPrefixDistance(first, second, max) : (int)GetDistance(first, second, max);
        }
    }
}
