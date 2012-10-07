using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace BigInteger
{
    class BigInteger
    {
        private static readonly int BASE = 10000;
        private static readonly int BASE_SIZE = 4;

        private List <int> value;
        private bool signe;

        public bool Signe 
        {
            get 
            {
                return signe;
            }
            private set 
            {
                signe = value; 
            }
        }
        public int Count
        {
            get 
            {
                return value.Count();
            }
        }

        public BigInteger ()
        {
            value = new List<int>();
        }
        public BigInteger (List<int> value, bool signe)
        {
            this.signe = signe;
            this.value = new List<int>(value);

            Process();
        }

        public BigInteger (string val) : this()
        {
            if (val[0] == '-')
                signe = true;

            foreach (char c in val) {
                if ('0' <= c && c <= '9')
                    value.Add(c - '0');
            }
            value.Reverse();
            Process();
        }
        public BigInteger (int val) : this()
        {
            if (val < 0) {
                signe = true;
                val = -val;
            }

            while (val > 0) {
                value.Add(val % BASE);
                val /= BASE;
            }
        }
        
        public BigInteger Clone ()
        {
            return new BigInteger(this.value, this.signe);
        }
        public int Last ()
        {
            return value[value.Count - 1];
        }
        public override string ToString ()
        {
            StringBuilder res = new StringBuilder();

            if (Count == 0)
                res.Append('0');
            else {
                if (signe)
                    res.Append('-');

                for (int i = Count - 1; i >= 0; i--) {
                    int val = value[i];
                    for (int j = 0; j < BASE_SIZE; j++) {
                        res.Append((char) (val % 10 + '0'));
                        val /= 10;
                    }
                    
                    for (int l = res.Length - BASE_SIZE, r = res.Length - 1; l < r; l++, r--) {
                        char tmp = res[l];
                        res[l] = res[r];
                        res[r] = tmp;
                    }
                }
            }   
         
            return res.ToString();
        }

        private void Process ()
        {
            while (Count > 0 && Last() == 0) {
                value.RemoveAt(Count - 1);
            }

            if (Count == 0)
                signe = false;
        }
        private static BigInteger Sum (BigInteger a, BigInteger b)
        {
            List <int> list = new List <int> ();
            
            for (int i = 0, d = 0; i < Math.Max(a.value.Count, b.value.Count) || d > 0; i++, d /= BASE) {
                if (a.value.Count > i)
                    d += a.value[i];
                if (b.value.Count > i)
                    d += b.value[i];

                list.Add(d % BASE);
            }

            return new BigInteger(list, false);
        }
        private static BigInteger Residual (BigInteger a, BigInteger b)
        {
            List <int> list = new List<int> ();

            for (int i = 0, d = 0; i < Math.Max(a.Count, b.Count) || d != 0; i++, d /= BASE) {
                if (a.Count > i)
                    d += a.value[i];
                if (b.Count > i)
                    d -= b.value[i];

                int cur = d % BASE;
                if (cur < 0)
                    cur += BASE;

                d -= cur;
                list.Add(cur);
            }

            return new BigInteger (list, false);
        }
        private static bool CompAbsValues (BigInteger a, BigInteger b)
        {
            if (a.Count < b.Count)
                return true;
            else if (a.Count == b.Count) {
                for (int i = a.Count - 1; i >= 0; i--)
                    if (a[i] < b[i])
                        return true;
                    else if (a[i] > b[i])
                        return false;
                return true;        
            }
            else {
                return false;
            }
        }

        public static BigInteger operator - (BigInteger a)
        {
            BigInteger res = a.Clone();
            res.signe = !res.signe;
            return res;
        }
        public int this[int i]
        {
            get
            {
                return value[i];
            }
            private set
            {
                this.value[i] = value; 
            }
        }

        public static bool operator < (BigInteger a, BigInteger b)
        {
            if (a.signe && !b.signe)
                return true;
            else if (!a.signe && b.signe)
                return false;
            else {
                if (a.Count < b.Count)
                    return true;
                else if (a.Count > b.Count)
                    return false;
                else {
                    for (int i = a.Count - 1; i >= 0; i--)
                        if (a[i] < b[i])
                            return true;
                        else if (a[i] > b[i])
                            return false;
                    return false;
                }
            }
        }
        public static bool operator <= (BigInteger a, BigInteger b)
        {
            if (a.signe && !b.signe) {
                return true;
            }
            else if (!a.signe && b.signe) {
                return false;
            }
            else {
                return a.signe ^ CompAbsValues(a, b);
            }
        }
        public static bool operator >= (BigInteger a, BigInteger b)
        {
            return b <= a;
        }
        public static bool operator > (BigInteger a, BigInteger b)
        {
            return b < a;
        }
        public static bool operator == (BigInteger a, BigInteger b)
        {
            if (a.signe == b.signe && a.Count == b.Count) {
                for (int i = 0; i < a.Count; i++)
                    if (a[i] != b[i])
                        return false;
                return true;
            }
            else {
                return false;
            }
        }
        public static bool operator != (BigInteger a, BigInteger b)
        {
            return !(a == b);
        }

        public static BigInteger operator + (BigInteger a, BigInteger b)
        {
            BigInteger res = null;
            
            if (a.signe == b.signe) {
                res = Sum(a, b);
                res.signe = a.signe;
            }
            else if (!CompAbsValues(a, b)) {
                res = Residual(a, b);
                res.signe = a.signe;
            }
            else {
                res = Residual(b, a);
                res.signe = b.signe;
            }

            res.Process();

            return res;
        }
        public static BigInteger operator - (BigInteger a, BigInteger b)
        {
            BigInteger res = null;

            if (a.signe != b.signe) {
                res = Sum(a, b);
                res.signe = a.signe;
            }
            else if (!CompAbsValues(a, b)) {
                res = Residual(a, b);
                res.signe = a.signe;
            }
            else {
                res = Residual(b, a);
                res.signe = !a.signe;
            }

            res.Process();

            return res;
        }
        public static BigInteger operator * (BigInteger a, BigInteger b)
        {
            List <int> list = new List<int>(a.Count + b.Count);
            for (int i = 0; i < a.Count + b.Count; i++)
                list.Add(0);

            for (int i = 0; i < a.Count; i++)
                for (int j = 0, d = 0; j < b.Count || d != 0; j++, d /= BASE) {
                    if (j < b.Count)
                        d += a[i] * b[j];

                    d += list[i + j];
                    list[i + j] = d % BASE;
                }

            BigInteger res = new BigInteger(list, a.signe ^ b.signe);
            res.Process();

            return res; 
        }
        public static BigInteger operator * (BigInteger a, int b)
        {
            List <int> list = new List<int>();
            
            long d = 0;
            for (int i = 0; i < a.Count || d != 0; i++, d /= BASE) {
                if (i < a.Count)
                    d += (long) a[i] * b;
                list.Add((int) (d % BASE));
            }

            BigInteger res = new BigInteger(list, a.signe);
            if (b < 0)
                res.Signe = !res.Signe;
            res.Process();

            return res;
        }
    }

    class Program
    {
        static readonly int ThreadCount = 10;
        static readonly int need = 40000;

        static int cur;
        static BigInteger result = new BigInteger(1);
        static Object nextIntLocker = new Object(), finalCountingLocker = new Object();
        static List <Thread> threadList = new List<Thread>();

        public static void calcFact ()
        {
            BigInteger res = new BigInteger(1);
            int current = 0;

            while (true) {
                lock (nextIntLocker) {
                    if (cur < need)
                        current = ++cur;
                    else
                        break;
                }

                res = res * current;
            }

            lock (finalCountingLocker) {
                result = result * res;
            }
        }

        public static void Main (string[] args)
        {
            long time = DateTime.Now.Ticks;

            for (int i = 0; i < ThreadCount; i++) {
                Thread thread = new Thread(calcFact);
                thread.Start();

                threadList.Add(thread);
            }

            for (int i = 0; i < threadList.Count(); i++)
                threadList[i].Join();
        
            Console.WriteLine(result);
        }
    }
}
