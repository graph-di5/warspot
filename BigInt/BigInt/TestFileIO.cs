using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigInt
{


    class TestFileIO
    {
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public List<int> parseInt(string input)
        {
            string res = "";
            int i = 0;
            while (input[i] != ' ')
            {
                res = res + input[i];
                //System.Console.WriteLine(res + " = res[" + i + "]");
                i++;
            }
            i = res.Length - 4;
            List<int> num = new List<int>();
            for (int j = 0; i > -4; i = i - 4, j++)
            {
                try
                {
                    num.Add(int.Parse(res.Substring(i, 4)));
                }
                catch (Exception e1)
                {
                    try
                    {
                        num.Add(int.Parse(res.Substring(i + 1, 3)));
                    }
                    catch (Exception e2)
                    {
                        try
                        {
                            num.Add(int.Parse(res.Substring(i + 2, 2)));
                        }
                        catch (Exception e3)
                        {
                            num.Add(int.Parse(res.Substring(i + 3, 1)));
                        }
                    }
                }


                //System.Console.WriteLine(num[j]+" = num["+j+"]");

            }
            return num;
        }

        public char parseChar(string input)
        {
            return input[input.Length - 1];
        }

        public List<int> sum(List<int> x, List<int> y)
        {
            List<int> res = new List<int>();
            int i = Math.Min(x.Count, y.Count);
            int j = 0;
            int flag = 0;
            for (; j < i; j++)
            {
                int temp = x[j] + y[j];
                if (temp < 10000)
                {
                    if (flag == 0)
                    {
                        res.Add(temp);
                    }
                    else
                    {
                        temp = temp + 1;
                        res.Add(temp);
                        flag = 0;
                    }
                }
                else
                {
                    if (flag == 0)
                    {
                        res.Add(temp % 10000);
                        flag = 1;
                    }
                    else
                    {
                        temp = temp + 1;
                        res.Add(temp % 10000 + 1);
                    }
                }
            }
            if (x.Count == y.Count)
            {
                if (flag == 1)
                {
                    res.Add(1);
                }
                else
                {
                    //do nothing
                }
            }
            else
            {
                if (i == x.Count)
                {
                    if (flag == 1)
                    {
                        res.Add(y[i] + 1);
                    }
                    else
                    {
                        res.Add(y[i]);
                    }
                    int k = i + 1;
                    while (k < y.Count)
                    {
                        res.Add(y[k]);
                        k++;
                    }
                }
                else
                {
                    if (flag == 1)
                    {
                        res.Add(x[i] + 1);
                    }
                    else
                    {
                        res.Add(x[i]);
                    }
                    int k = i + 1;
                    while (k < x.Count)
                    {
                        res.Add(x[k]);
                        k++;
                    }
                }
            }
            return res;
        }

        public string printInt(List<int> input)
        {
            string res = "";
            // System.Console.WriteLine("!!!" + res);
            int i = 0;

            for (; i < input.Count; i++)
            {
                if (input[i] == 0)
                {
                    res = "0000" + input[i] + res;
                }
                else
                {
                    if (input[i] < 10)
                    {
                        res = "000" + input[i] + res;
                    }
                    else
                    {
                        if (input[i] < 100)
                        {
                            res = "00" + input[i] + res;
                        }
                        else
                        {
                            if (input[i] < 1000)
                            {
                                res = "0" + input[i] + res;
                            }
                            else
                            {
                                res = input[i] + res;
                            }
                        }
                    }

                }

            }

            while (true)
            {
                if (res.StartsWith("0"))
                {
                    res = res.Substring(1);
                }
                else
                {
                    break;
                }
            }

            return res;

        }

        static void Main()
        {

            string fileName = "input.txt";
            List<int> op1 = new List<int>();
            List<int> op2 = new List<int>();

            using (System.IO.StreamReader sr = System.IO.File.OpenText(fileName))
            {
                string s = "";
                int i = 0;
                while ((s = sr.ReadLine()) != null)
                {
                    System.Console.WriteLine(s);
                    if (i == 0)
                    {
                        op1 = (new TestFileIO()).parseInt(s);
                        i++;
                    }
                    else
                    {
                        op2 = (new TestFileIO()).parseInt(s);
                        i++;
                    }

                }

            }

            List<int> op3 = (new TestFileIO()).sum(op1, op2);



            using (System.IO.FileStream fs = System.IO.File.Create("output.txt", 1024))
            {
                byte[] info = new System.Text.UTF8Encoding(true).GetBytes((new TestFileIO()).printInt(op3));
                fs.Write(info, 0, info.Length);
            }



            System.Console.ReadKey();
        }
    }

}
