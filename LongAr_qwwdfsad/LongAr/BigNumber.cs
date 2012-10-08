using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongAr
{
    class BigNumbers
    {
        public List<int> Numbers = new List<int>();

        #region functions

        public BigNumbers()
        {
            // Казалось бы, зачем я здесь?
        }

        public BigNumbers(string data)
        {
            // TODO: некорректно читается отрицательно число
            for (int i = data.Length - 1; i >= 0; i--)
            {
                Numbers.Add(data[i] - 48);
            }
        }

        public void Write()
        {
            for (int i = Numbers.Count - 1; i >= 0; i--)
            {
                Console.Write(Numbers[i]);
            }
        }
        // Для вычитания, в других случаях не трогать, падает при исключении
        private int firstNotNullElementIndex(int start)
        {
            for (int i = start; i < Numbers.Count; i++)
            {
                if (Numbers[i] != 0)
                    return i;
            }

            return -1;
        }

        private void deleteNulls()
        {
            if (this.Numbers.Count == 1 && this.Numbers[0] == 0)
                return;
            else
            {
                if (this.Numbers[this.Numbers.Count - 1] == 0)
                {
                    Numbers.RemoveAt(this.Numbers.Count -1);
                    deleteNulls();
                }
            }
        }
        #endregion functions

        #region operators

        public static BigNumbers operator +(BigNumbers first, BigNumbers second)
        {
            var result = new BigNumbers();
            result.Numbers.Capacity = first.Numbers.Count + second.Numbers.Count;                   
            bool checker = true;

            if (first.Numbers.Count >= second.Numbers.Count)
                result.Numbers.AddRange(first.Numbers);             
            else
            {
                result.Numbers.AddRange(second.Numbers);
                checker = false;
            }
            result.Numbers.Add(0);

            if (checker)
            {
                for (int i = 0; i < second.Numbers.Count; i++)
                {
                    result.Numbers[i] += second.Numbers[i];
                    if (result.Numbers[i] >= 10)
                    {
                        result.Numbers[i] %= 10;
                        result.Numbers[i + 1] += 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < first.Numbers.Count; i++)
                {
                    result.Numbers[i] += first.Numbers[i];
                    if (result.Numbers[i] >= 10)
                    {
                        result.Numbers[i] %= 10;
                        result.Numbers[i + 1] += 1;
                    }
                }
            }
            result.deleteNulls();
            return result;            
        }

        public static BigNumbers operator -(BigNumbers first, BigNumbers second)
        {
            var result = new BigNumbers();
            result.Numbers.AddRange(first.Numbers);
            if (first >= second)
            {
                for (int i = 0; i < second.Numbers.Count; i++)
                {
                    if (result.Numbers[i] < second.Numbers[i])
                    {
                        int j = result.firstNotNullElementIndex(i);
                        result.Numbers[j] -= 1;
                        for (int k = j - 1; k > i; k--)
                        {
                            result.Numbers[k] = 9;
                        }
                        result.Numbers[i] = 10 - second.Numbers[i];
                    }
                    else
                        result.Numbers[i] = result.Numbers[i] - second.Numbers[i];
                }
            }
            else
                return -(second - first);

            result.deleteNulls();
            return result;
        }

        public static BigNumbers operator *(BigNumbers first, BigNumbers second)
        {
            var result = new BigNumbers();
            result.Numbers.Add(0);
            for (long i = 0; i < second; i++)
            {
                result += first;
            }
            return result;
        }

        public static long operator /(BigNumbers first, BigNumbers second)
        {
            if (second == 0)
            {
                // НЕНАВИСТЬ
                throw new DivideByZeroException();
            }
            if (first < second)
                return 0;

            int counter = 0;
            while (first >= second)
            {
                first = first - second;
                ++counter;
            }
            return counter;
        }


        public static bool operator <(BigNumbers first, BigNumbers second)
        {
            // Никто ведь не будет вводить число 2 как 00000002?
            if (second.Numbers.Count > first.Numbers.Count)
                return true;
            if (second.Numbers.Count < first.Numbers.Count)
                return false;

            for (int i = first.Numbers.Count - 1; i >= 0; i--)
            {
                if (first.Numbers[i] < second.Numbers[i])
                    return true;
            }

            return false;
        }

        public static bool operator >(BigNumbers first, BigNumbers second)
        {
            return second < first;
        }


        public static BigNumbers operator -(BigNumbers number)
        {
            var result = new BigNumbers();
            result.Numbers.AddRange(number.Numbers);
            result.Numbers[result.Numbers.Count - 1] = -result.Numbers[result.Numbers.Count - 1];
            return result;
        }

        public static implicit operator long(BigNumbers number)
        {
            long result = 0;

            for (int i = 0; i < number.Numbers.Count; i++)
            {
                result += number.Numbers[i] * Convert.ToInt64(Math.Pow(10, i));
            }
            return result;
        }

        #endregion operators
    }
}
