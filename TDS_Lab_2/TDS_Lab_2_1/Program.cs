using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS_Lab_2_1
{
    class Program
    {
        private static double MyFunction(int x)
        {
            return 9.2 * Math.Cos(Math.Pow(x, 2)) - Math.Abs(Math.Sin(x) / 1.1);
        }
        private static double GetA(int i)
        {
            double local = 0;
            int a = i;

            while (a < i + 8) {
                local += MyFunction(i);
                a++;
            }
            return local;
        }
        private static double GetB(int i)
        {
            double local = 1;
            int a = i;

            while (a < i + 5)
            {
                local *= MyFunction(i);
                a++;
            }
            return local;
        }
        private static double GetParA(int i)
        {
            double local = 0;
            Parallel.For(i, i + 8, a =>
            {
                local += MyFunction(a);
            });
            return local;
        }

        private static double GetParB(int i)
        {
            double local = 1;
            Parallel.For(i, i + 5, a =>
            {
                local *= MyFunction(a);
            });
            return local;
        }
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            double[] arr1 = new double[10000];
            int i = 0;

            Parallel.For(i, i + 10000, a =>
            {
                Task<double> thread1 = new Task<double>(() => GetParA(a));
                Task<double> thread2 = new Task<double>(() => GetParB(a));
                thread1.Start();
                thread2.Start();

                arr1[i] = thread1.Result + thread2.Result;

                //Console.Write(arr1[i] + "\n");

            });

            Console.WriteLine("1 algorithm: {0}", sw.ElapsedMilliseconds);

            sw.Restart();

            double[] arr2 = new double[10000];

            for (int j = 0; j < 10000; j++) {
                arr2[j] = GetA(j) + GetB(j);
            }

            Console.WriteLine("2 algorithm: {0}", sw.ElapsedMilliseconds);


            Console.ReadKey();
        }
    }

}
