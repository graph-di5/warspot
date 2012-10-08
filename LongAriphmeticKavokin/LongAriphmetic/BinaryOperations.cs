using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongAriphmetic
{
    enum Operation
    {
        Plus,
        Minus,
        Mult,
        Div
    }

    internal sealed class BinaryOperations
    {
        public int[] ConvertStringToArray(string str)
        {
            int[] result = new int[str.Length];

            for (int posInStr = 0; posInStr < str.Length; posInStr++)
            {
                result[posInStr] = int.Parse(str[posInStr].ToString());
            }

            return result;
        }

        // добавляет нолики к массиву
        private int[] ExpandTheArray(int dimension, int[] array)
        {
            int[] result = new int[dimension];

            int m = dimension-1;
            for (int k = array.Length - 1; k >= 0; k--)
            {
                result[m] = array[k];
                m--;
            }
                return result;
        }

        // оба массива должны иметь одинаковую размерность. к меньшему можно добавить нолики
        private int[] addTwoArrays(int[] first, int[] second)
        {
            int[] result = new int[first.Length + 1];
            int saver = 0;

            for (int pos = first.Length - 1; pos >= 0; pos--)
            {
                result[pos + 1] = (first[pos] + second[pos] + saver / 10) % 10;
                saver = first[pos] + second[pos];
            }
            result[0] = (first[0] + second[0] + saver / 10) / 10;
            return result;
        }

        public int[] Addition(int[] first, int[] second)
        {
            int max = Math.Max(first.Length, second.Length);
            int[] firstNumber = first;
            int[] secondNumber = second;

            /*if (first.Length == second.Length)
            {
                firstNumber = first;
                secondNumber = second;
            }*/
            if (max > second.Length)
            {
                firstNumber = first;
                secondNumber = ExpandTheArray(max, second);
            }
            if (max > first.Length)
            {
                firstNumber = ExpandTheArray(max, first);
                secondNumber = second;
            }

            int[] result = toShortenArray( addTwoArrays(firstNumber, secondNumber));
            //int[] result = addTwoArrays(firstNumber, secondNumber);
            return result;
        }
            
        private int[] toShortenArray(int[] array)
        {
            //int []res = (from x in array where x != 0 select x).ToArray();

            int i = 0;
            while (array[i] == 0) i++;

            int[] result = new int[array.Length - i];
            int j=0;
            if (i == 0)
            {
                return array;
            }
            for (int pos = i; pos <= result.Length; pos++)
            {
                result[j] = array[pos];
                j++;
            }
            return result;
        }

        /*public int[] minusTwoArrays(int[] first, int[] second)
        {
            int[] result = new int[first.Length + 1];
            bool sign;

            if (first.Length < second.Length)
            {
                sign = false;
            }
            for (int pos = first.Length - 1; pos >= 0; pos--)
            { 
                if ()
            }
        }*/
    }
}
