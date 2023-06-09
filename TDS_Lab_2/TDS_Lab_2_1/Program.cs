﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS_Lab_2_1
{
    class Program
    {

        private static int ITEM_COUNT = 10000;

        private static double MyFunction(int x)
        {
            return 9.2 * Math.Cos(Math.Pow(x, 2)) - Math.Abs(Math.Sin(x) / 1.1);
        }
        private static double MyFunctionPar(int x)
        {
            Task<double> thread1 = new Task<double>(() => Math.Pow(x, 2));
            Task<double> thread2 = new Task<double>(() => Math.Cos(thread1.Result));
            Task<double> thread3 = new Task<double>(() => 9.2 * thread2.Result);


            Task<double> thread4 = new Task<double>(() => Math.Sin(x));
            Task<double> thread5 = new Task<double>(() => thread4.Result / 1.1);
            Task<double> thread6 = new Task<double>(() => Math.Abs(thread5.Result));

            Task<double> thread7 = new Task<double>(() => thread3.Result - thread6.Result);




            return 9.2 * Math.Cos(Math.Pow(x, 2)) - Math.Abs(Math.Sin(x) / 1.1);
        }

        private static double GetA(int i)
        {
            double local = 0;
            int a = i;

            while (a < i + 8) {
                local += MyFunction(a);
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
                local *= MyFunction(a);
                a++;
            }
            return local;
        }
        private static double GetParA(int i)
        {
            double local = 0;
            int b = i;

            Parallel.For(b, i + 8, a =>
            {
                local += MyFunction(a);
            });
            return local;
        }

        private static double GetParB(int i)
        {
            double local = 1;
            int b = i;

            Parallel.For(b, i + 5, a =>
            {
                local *= MyFunction(a);
            });
            return local;
        }
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            double[] arr1 = new double[ITEM_COUNT];
            int i = 0;

            Parallel.For(i, ITEM_COUNT, a =>
            {
                Task<double> thread1 = new Task<double>(() => GetParA(a));
                Task<double> thread2 = new Task<double>(() => GetParB(a));
                thread1.Start();
                thread2.Start();

                arr1[a] = thread1.Result + thread2.Result;
            });

            Console.Write(arr1[0] + "\n");

            Console.WriteLine("1 algorithm: {0}", sw.ElapsedMilliseconds);

            sw.Restart();

            double[] arr2 = new double[ITEM_COUNT];

            for (int j = 0; j < ITEM_COUNT; j++) {
                arr2[j] = GetA(j) + GetB(j);
            }

            Console.Write(arr2[0] + "\n");
            Console.WriteLine("2 algorithm: {0}", sw.ElapsedMilliseconds);


            Console.ReadKey();
        }
    }

}
