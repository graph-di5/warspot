using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigInt
{
    class Program
    {
        static void Main(string[] args)
        {

            long start = DateTime.Now.Ticks;
            BigInteger a = new BigInteger(200000);
            BigInteger b = new BigInteger(3);
            a = -a % b;
            Console.WriteLine(a.ToString());
            Console.WriteLine((DateTime.Now.Ticks - start) / 10000000d + " sec.");
        }
    }

    public class BigInteger
    {
        private int length = 0;             //Left after the array implementation.
        private List<int> num = new List<int>();
        private const int BASE = 10000;     //BASE = 10^N
        private const int DIGITS_IN_BASE = 4;
        private int sign = 0;

        public BigInteger(int n)
        {
            if (n == 0)
            {
                sign = 0;
                return;
            }
            if (n < 0)
            {
                sign = -1;
                n *= -1;
            }
            else
            {
                sign = 1;
            }
            while (n > 0)
            {
                num.Add(n % BASE);
                n /= BASE;
                ++length;
            }
        }

        public BigInteger(BigInteger a)
        {
            length = a.length;
            sign = a.sign;
            num = new List<int>(a.num);
        }

        override public string ToString()
        {
            if (sign == 0)
            {
                return "0";
            }
            StringBuilder ans = new StringBuilder();
            if (sign < 0)
            {
                ans.Append('-');
            }
            ans.Append(num[length - 1]);
            for (int i = length - 2; i >= 0; --i)
            {
                string t = "" + num[i];
                while (t.Length < DIGITS_IN_BASE)
                {
                    t = '0' + t;
                }
                ans.Append(t);
            }
            return ans.ToString();
        }

        public int ToInt()
        {
            int ans = 0;
            for (int i = length - 1; i >= 0; --i)
            {
                ans = ans * BASE + sign * num[i];
            }
            return ans;
        }

        public static bool operator ==(BigInteger a, BigInteger b)
        {
            return a.sign == b.sign && EqualUnsigned(a, b);
        }

        public static bool operator !=(BigInteger a, BigInteger b)
        {
            return !(a == b);
        }

        public static bool operator >(BigInteger a, BigInteger b)
        {
            return a.sign > b.sign || a.sign == b.sign && (a.sign == -1 && LessUnsigned(a, b) || a.sign == 1 && LessUnsigned(b, a));
        }

        public static bool operator >=(BigInteger a, BigInteger b)
        {
            return a.sign > b.sign || a.sign == b.sign && (a.sign == 0 || a.sign == -1 && LessOrEqualUnsigned(a, b) || a.sign == 1 && LessOrEqualUnsigned(b, a));
        }

        public static bool operator <(BigInteger a, BigInteger b)
        {
            return b > a;
        }

        public static bool operator <=(BigInteger a, BigInteger b)
        {
            return b >= a;
        }

        public static BigInteger operator +(BigInteger a, BigInteger b)
        {
            if (a.sign == 0)
            {
                return new BigInteger(b);
            }
            if (b.sign == 0)
            {
                return new BigInteger(a);
            }
            if (a.sign < 0)
            {
                if (b.sign < 0)
                {
                    BigInteger ans = AddUnsigned(a, b);
                    ans.sign = -1;
                    return ans;
                }
                else
                {
                    return SubUnsigned(b, a);
                }
            }
            else
            {
                if (b.sign < 0)
                {
                    return SubUnsigned(a, b);
                }
                else
                {
                    return AddUnsigned(a, b);
                }
            }
        }

        public static BigInteger operator -(BigInteger a)
        {
            BigInteger ans = new BigInteger(a);
            ans.sign *= -1;
            return ans;
        }

        public static BigInteger operator -(BigInteger a, BigInteger b)
        {
            if (a.sign == 0)
            {
                return -b;
            }
            if (b.sign == 0)
            {
                return new BigInteger(a);
            }
            if (a.sign < 0)
            {
                if (b.sign > 0)
                {
                    BigInteger ans = AddUnsigned(a, b);
                    ans.sign = -1;
                    return ans;
                }
                else
                {
                    return SubUnsigned(b, a);
                }
            }
            else
            {
                if (b.sign > 0)
                {
                    return SubUnsigned(a, b);
                }
                else
                {
                    return AddUnsigned(a, b);
                }
            }
        }

        public static BigInteger operator *(BigInteger a, BigInteger b)
        {
            BigInteger ans = MulUnsigned(a, b);
            ans.sign = a.sign * b.sign;
            return ans;
        }

        public BigInteger Power(uint n)
        {
            if (n == 0)
            {
                return new BigInteger(1);
            }
            else if (n == 1)
            {
                return new BigInteger(this);
            }
            else if (n == 2)
            {
                return this * this;
            }
            else if (n % 2 == 0)
            {
                return this.Power(n / 2).Power(2);
            }
            else
            {
                return this * this.Power(n - 1);
            }
        }

        public static BigInteger operator /(BigInteger a, BigInteger b)
        {
            if (b.sign == 0)
            {
                throw new DivideByZeroException();
            }
            BigInteger t = DivUnsigned(a, b);
            t.sign = a.sign * b.sign;
            return t;
        }

        public static BigInteger operator %(BigInteger a, BigInteger b)
        {
            if (b.sign == 0)
            {
                throw new DivideByZeroException();
            }
            BigInteger t = ModUnsigned(a, b);
            t.sign = a.sign;
            return t;
        }

        public BigInteger Factorial()
        {
            if (sign < 0)
            {
                throw new Exception("Negative number");
            }
            else if (sign == 0)
            {
                return new BigInteger(1);
            }
            else
            {
                BigInteger ans = new BigInteger(1);
                BigInteger counter = new BigInteger(1);
                while (counter != this)
                {
                    ans = ans * counter;
                    counter.Inc();
                }
                return ans;
            }
        }

        //----------------------------------------------------------------------

        private static bool LessUnsigned(BigInteger a, BigInteger b)
        {
            if (a.length > b.length)
            {
                return false;
            }
            else if (a.length < b.length)
            {
                return true;
            }
            else
            {
                for (int i = a.length - 1; i >= 0; --i)
                {
                    if (a.num[i] > b.num[i])
                    {
                        return false;
                    }
                    else if (a.num[i] < b.num[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool LessOrEqualUnsigned(BigInteger a, BigInteger b)
        {
            if (a.length > b.length)
            {
                return false;
            }
            else if (a.length < b.length)
            {
                return true;
            }
            else
            {
                for (int i = a.length - 1; i >= 0; --i)
                {
                    if (a.num[i] > b.num[i])
                    {
                        return false;
                    }
                    else if (a.num[i] < b.num[i])
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        private static bool EqualUnsigned(BigInteger a, BigInteger b)
        {
            if (a.length > b.length)
            {
                return false;
            }
            else if (a.length < b.length)
            {
                return false;
            }
            else
            {
                for (int i = a.length - 1; i >= 0; --i)
                {
                    if (a.num[i] > b.num[i])
                    {
                        return false;
                    }
                    else if (a.num[i] < b.num[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static BigInteger AddUnsigned(BigInteger a, BigInteger b)
        {
            BigInteger ans = new BigInteger(0);
            ans.sign = 1;
            int next = 0;
            int l = Math.Max(a.length, b.length);
            for (int i = 0; i < l; ++i)
            {
                int t = a.num[i] + b.num[i] + next;
                next = t / BASE;
                t %= BASE;
                ans.num.Add(t);
            }
            ans.length = l;
            if (next > 0)
            {
                ans.num.Add(next);
                ans.length++;
            }
            return ans;
        }

        private static BigInteger SubUnsigned(BigInteger a, BigInteger b)
        {
            BigInteger ans = new BigInteger(0);
            if (a == b)
            {
                return ans;
            }
            if (LessUnsigned(a, b))
            {
                BigInteger t = a;
                a = b;
                b = t;
                ans.sign = -1;
            }
            else
            {
                ans.sign = 1;
            }
            int l = a.length;
            int back = 0;
            for (int i = 0; i < l; ++i)
            {
                int t = a.num[i] - b.num[i] - back;
                if (t < 0)
                {
                    t += BASE;
                    back = 1;
                }
                else
                {
                    back = 0;
                }
                ans.num.Add(t);
            }
            while (ans.num.Count > 0 && ans.num[ans.num.Count - 1] == 0)
            {
                ans.num.RemoveAt(ans.num.Count - 1);
            }
            ans.length = ans.num.Count;
            return ans;
        }

        /**
         * Not used.
         * 0 <= n < BASE
         */
        private static BigInteger MulUnsigned(BigInteger a, int n)
        {
            BigInteger ans = new BigInteger(0);
            ans.sign = 1;
            int next = 0;
            for (int i = 0; i < a.length || next != 0; ++i)
            {
                int t = (i < a.length ? a.num[i] * n : 0) + next;
                next = t / BASE;
                t %= BASE;
                ans.num.Add(t);
                if (t > 0)
                {
                    ans.length = i + 1;
                }
            }
            return ans;
        }

        private static BigInteger MulUnsigned(BigInteger a, BigInteger b)
        {
            BigInteger ans = new BigInteger(0);
            ans.sign = 1;
            int next = 0;
            for (int i = 0; i < a.length + b.length - 1; ++i)
            {
                int t = next;
                //i = j + k
                for (int j = (i < b.length ? 0 : i + 1 - b.length); j < a.length && i - j >= 0; ++j)
                {
                    int k = i - j;
                    t += a.num[j] * b.num[k];
                }
                next = t / BASE;
                t %= BASE;
                ans.num.Add(t);
            }
            while (next > 0)
            {
                ans.num.Add(next % BASE);
                next /= BASE;
            }
            while (ans.num.Count > 0 && ans.num[ans.num.Count - 1] == 0)
            {
                ans.num.RemoveAt(ans.num.Count - 1);
            }
            ans.length = ans.num.Count;
            return ans;
        }

        private void Inc()
        {
            for (int i = 0; i < length; i++)
            {
                ++num[i];
                if (num[i] == BASE)
                {
                    num[i] = 0;
                    if (i == length - 1)
                    {
                        num.Add(1);
                        ++length;
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private static BigInteger DivUnsigned(BigInteger a, BigInteger b)
        {
            BigInteger ans = new BigInteger(0);
            BigInteger cur = new BigInteger(0);
            BigInteger _base = new BigInteger(BASE);
            for (int i = a.length - 1; i >= 0; --i)
            {
                cur.num.Insert(0, a.num[i]);
                ++cur.length;
                if (LessUnsigned(cur, b))
                {
                    if (ans.num.Count > 0)
                    {
                        ans.num.Insert(0, 0);
                    }
                    continue;
                }
                int l = 0, r = BASE;
                while (r - l > 1)
                {
                    int m = (l + r) / 2;
                    BigInteger t = MulUnsigned(b, m);
                    if (LessOrEqualUnsigned(t, cur))
                    {
                        l = m;
                    }
                    else
                    {
                        r = m;
                    }
                }
                cur = SubUnsigned(cur, MulUnsigned(b, l));
                ans.num.Insert(0, l);
            }
            ans.sign = 1;
            ans.length = ans.num.Count;
            return ans;
        }

        private static BigInteger ModUnsigned(BigInteger a, BigInteger b)
        {
            BigInteger cur = new BigInteger(0);
            BigInteger _base = new BigInteger(BASE);
            for (int i = a.length - 1; i >= 0; --i)
            {
                cur.num.Insert(0, a.num[i]);
                ++cur.length;
                cur.sign = 1;
                int l = 0, r = BASE;
                while (r - l > 1)
                {
                    int m = (l + r) / 2;
                    BigInteger t = MulUnsigned(b, m);
                    if (LessOrEqualUnsigned(t, cur))
                    {
                        l = m;
                    }
                    else
                    {
                        r = m;
                    }
                }
                cur = SubUnsigned(cur, MulUnsigned(b, l));
            }
            return cur;
        }
    }
}
