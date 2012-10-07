using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongArithmetics
{
    class Program
    {
  
        struct longNumber
        {
            private List<int> numerals;

            private int length;

            private Boolean sign;

            public void constructLongNumber()
            {
                
                numerals = new List<int>();
                length = 0;
                sign = true;
            }

            public void readNumeral(int a)
            {
                numerals.Insert(0, a);
                length++;
            }

            public void addNumeral(int a)
            {
                numerals.Add(a);
                length++;
            }

            public int getLength()
            {
                return length;
            }

            private void setLength(int value)
            {
                length = value;
            }

            public void setSign(Boolean isPositive)
            {
                sign = isPositive;
            }

            public void changeSign()
            {
                if (sign) sign = false;
                else sign = true;
            }

            public Boolean getSign()
            {
                return sign;
            }

            public void justLength()
            {
                setLength(numerals.Count());
            }

            public int getNumeralAtPosition(int i)
            {
                return numerals.ElementAt(i);
            }

            public void setNumeralAtPosition(int i,int value)
            {

                numerals.Insert(i, value);
                numerals.RemoveAt(i + 1);
            }

            public void decNumeralAtPosition(int i)
            {
                numerals.Insert(i, numerals.ElementAt(i) - 1);
                numerals.RemoveAt(i + 1);
            }

            public void borrowed(int i)
            {
                numerals.Insert(i, numerals.ElementAt(i) + 10);
                numerals.RemoveAt(i + 1);

            }

            public String toFile()
            {
                String s = "";
                if(!sign) s += '-';
                for (int i = length - 1; i >= 0; i--)
                    s += getNumeralAtPosition(i).ToString();
                return s;
            }

            public void removeLast()
            {
                numerals.RemoveAt(length - 1);
                length--;
            }

            public void reverse()
            {
                numerals.Reverse(0,length);
            }

            public void concat(longNumber n)
            {
                for (int i = n.getLength() - 1; i >= 0; i--)
                    numerals.Add(n.getNumeralAtPosition(i));
            }

        }

        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(@"in.txt");
            String s = file.ReadToEnd();
            longNumber a = new longNumber(),b = new longNumber();
            a.constructLongNumber();
            b.constructLongNumber();
            Boolean negative = false;
            Boolean sign = false;
            char operation1 = ' ';
            char operation2 = ' ';
            if (s.First() == '-')
            {
                s = s.Substring(1);
                negative = true;
            }
            foreach (char ii in s)
            {
                if ((int)ii >= 48 && (int)ii <= 57)
                {
                    sign = false;
                    b.readNumeral((int)ii - 48);
                }
                else
                {
                    if (negative)
                    {
                        b.changeSign();
                        negative = false;
                    }
                    if (sign)
                        negative = true;
                    else if(ii == '+' || ii == '-' || ii =='*' || ii =='/')
                    {
                        operation2 = ii;
                        switch (operation1)
                        {
                            case '+':
                                a = sumUp(a, b);
                                break;
                            case '-':
                                a = subtract(a, b);
                                break;
                            case '*':
                                a = multiple(a, b);
                                break;
                            case '/':
                                a = divide(a, b);
                                break;
                            case ' ':
                                a = b;
                                break;
                            default:
                                break;
                        }
                        sign = true;
                        operation1 = operation2;
                        b.constructLongNumber();
                    }
                } 
            }
            if (negative)
                b.changeSign();
            switch (operation1)
            {
                case '+':
                    a = sumUp(a, b);
                    break;
                case '-':
                    a = subtract(a, b);
                    break;
                case '*':
                    a = multiple(a, b);
                    break;
                case '/':
                    a = divide(a, b);
                    break;
                default:
                    break;
            }
            file.Close();
            using (System.IO.StreamWriter file2 = new System.IO.StreamWriter("out.txt", false))
            {
                file2.WriteLine(a.toFile());
                file2.Close();
            }
        }

        private static longNumber divide(longNumber c, longNumber d)
        {
            
            longNumber result = new longNumber();
            result.constructLongNumber();
            if (c.getSign() != d.getSign())
                result.setSign(false);
            else result.setSign(true);
            d.setSign(true);
            longNumber temp = new longNumber();
            temp.constructLongNumber();
            longNumber temp2 = new longNumber();
            temp2.constructLongNumber();
            int k = 0;
            int t;
            int cur = c.getLength() - d.getLength() - 1;
            
            if (cur < 0) cur = 0;
            
            for (int i = c.getLength() - 1; i >= cur; i--)
                temp.readNumeral(c.getNumeralAtPosition(i));
            cur--;
            
            while (cur >= 0)
            {
                k = 0;
                temp = subtract(temp, d);
                while (temp.getSign())
                {
                    temp.justLength();
                    k++;
                    temp = subtract(temp, d);
                    
                }
                temp = sumUp(temp, d);
                Console.WriteLine(temp.toFile());
                Console.ReadKey();
                while (k > 0)
                {
                    temp2.readNumeral(k % 10);
                    k = k / 10;
                }
                temp2.reverse();
                for (int i = temp2.getLength() - 1; i >= 0; i--)
                    result.addNumeral(temp2.getNumeralAtPosition(i));
                temp2.constructLongNumber();
                
                t = temp.getLength();
                for (int j = t - 1; j >= 0; j--)
                    if (temp.getNumeralAtPosition(j) == 0)
                        temp.removeLast();
                    else break;
                Boolean flag = false;
                while (!compare(temp, d) && cur >= 0)
                {
                    if (flag)
                        result.addNumeral(0);
                    flag = true;
                    temp.readNumeral(c.getNumeralAtPosition(cur));
                    cur--;
                }

                t = temp.getLength();
                for (int j = t - 1; j >= 0; j--)
                    if (temp.getNumeralAtPosition(j) == 0)
                        temp.removeLast();
                    else break;
            }
            k = 0;
            temp = subtract(temp, d);
            while (temp.getSign())
            {
                k++;
                temp = subtract(temp, d);
                
            }
            while (k > 0)
            {
                result.addNumeral(k % 10);
                k /= 10;
            }
            
            result.reverse();
            if (result.getLength() == 0)
                result.addNumeral(0);
            return result;
        }

        private static Boolean compare(longNumber c, longNumber d)
        {
            if (c.getLength() < d.getLength())
            {
                return false;
            }
            else if (c.getLength() > d.getLength())
            {
                return true;
            }
            else
            {
                Boolean equal = true;
                for (int i = c.getLength() - 1; i >= 0; i--)
                {
                    if (c.getNumeralAtPosition(i) > d.getNumeralAtPosition(i))
                    {
                        return true;
                    }
                    if (c.getNumeralAtPosition(i) < d.getNumeralAtPosition(i))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private static longNumber multiple(longNumber c, longNumber d)
        {
            longNumber result = new longNumber();
            result.constructLongNumber();
            if (c.getSign() != d.getSign())
                result.setSign(false);
            else result.setSign(true);
            for(int i = 0; i < c.getLength() + d.getLength(); i++)
                result.addNumeral(0);
            int carry = 0;
            for (int i = 0; i < c.getLength(); i++)
            {
                for (int j = 0; j < d.getLength(); j++)
                {
                    int k = result.getNumeralAtPosition(i + j);
                    result.setNumeralAtPosition(i+j, ((c.getNumeralAtPosition(i) * d.getNumeralAtPosition(j) + carry + result.getNumeralAtPosition(i + j)) % 10));
                    carry = (c.getNumeralAtPosition(i) * d.getNumeralAtPosition(j) + carry + k) / 10;
                    
                }
                
                if (carry > 0)
                    result.setNumeralAtPosition(i + d.getLength(), carry + result.getNumeralAtPosition(i + d.getLength()));
                carry = 0;
            }
            int temp = result.getLength();
            for (int i = temp - 1; i >= 0; i--)
                if (result.getNumeralAtPosition(i) == 0)
                    result.removeLast();
                else break;
            return result;
        }

        private static longNumber sumUp(longNumber c, longNumber d)
        {
            
            if (c.getSign() != d.getSign())
            {
                if (c.getSign() == false)
                {
                    c.changeSign();
                    return subtract(d, c);
                }
                else
                {
                    d.changeSign();
                    return subtract(c, d);
                }
            }
            longNumber min = new longNumber();
            min.constructLongNumber();
            longNumber max = new longNumber();
            max.constructLongNumber();
            longNumber result = new longNumber();
            result.constructLongNumber();
            result.setSign(c.getSign());
            if (c.getLength() < d.getLength())
            {
                min = c;
                max = d;
            }
            else
                {
                    min = d;
                    max = c;
                }
            int carry = 0;
            for (int i = 0; i < min.getLength(); i++)
            {
                result.addNumeral((min.getNumeralAtPosition(i) + max.getNumeralAtPosition(i) + carry) % 10);
                carry = (int)((min.getNumeralAtPosition(i) + max.getNumeralAtPosition(i) + carry) / 10);
            }
            for (int j = min.getLength(); j < max.getLength(); j++)
            {
                result.addNumeral((max.getNumeralAtPosition(j) + carry) % 10);
                carry = (int)((max.getNumeralAtPosition(j) + carry) / 10);
            }
            if (carry != 0) result.addNumeral(carry);
            return result;
        }

        private static longNumber subtract(longNumber c, longNumber d)
        {
            
            if (c.getSign() != d.getSign())
            {
                d.changeSign();
                return sumUp(c, d);
            }
            longNumber min = new longNumber();
            min.constructLongNumber();
            longNumber max = new longNumber();
            max.constructLongNumber();
            longNumber result = new longNumber();
            result.constructLongNumber();
            if (c.getLength() < d.getLength())
            {
                min = c;
                max = d;
                result.setSign(!d.getSign());
            }
            else if (c.getLength() > d.getLength())
            {
                min = d;
                max = c;
                result.setSign(d.getSign());
            }
            else
            {
                Boolean equal = true;
                for (int i = c.getLength() - 1; i >= 0; i--)
                {
                    if (c.getNumeralAtPosition(i) > d.getNumeralAtPosition(i))
                    {
                        min = d;
                        max = c;
                        result.setSign(d.getSign());
                        equal = false;
                        break;
                    }
                    if (c.getNumeralAtPosition(i) < d.getNumeralAtPosition(i))
                    {
                        min = c;
                        max = d;
                        result.setSign(!d.getSign());
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    result.addNumeral(0);
                    return result;
                }
            }
            for (int i = 0; i < min.getLength(); i++)
            {
                if (max.getNumeralAtPosition(i) < min.getNumeralAtPosition(i))
                {
                    max.borrowed(i);
                    int j = i + 1;
                    while (max.getNumeralAtPosition(j) == 0)
                    {
                        max.setNumeralAtPosition(j, 9);
                        j++;
                    }
                    max.decNumeralAtPosition(j);
                }
                result.addNumeral(max.getNumeralAtPosition(i) - min.getNumeralAtPosition(i));
            }
            for (int j = min.getLength(); j < max.getLength(); j++)
                result.addNumeral(max.getNumeralAtPosition(j));
            int temp = result.getLength();
            for (int i = temp - 1; i >= 0; i--)
                if (result.getNumeralAtPosition(i) == 0)
                    result.removeLast();
                else break;
           
            return result;
        }
    }
}
