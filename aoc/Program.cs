using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Experiments;
using MoreLinq.Extensions;

namespace aoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt");
            var res = 0;
            
            Console.Out.WriteLine(res);
        }

        static void Main_2_2(string[] args)
        {
            var lines = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt");
            var res = 0;
            foreach (var line in lines)
            {
                var split = line.Split(new[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(split[0]);
                var max = int.Parse(split[1]);
                var ch = split[2][0];
                var psw = split[3];
                var c1 = psw[min - 1] == ch ? 1 : 0;
                var c2 = psw[max - 1] == ch ? 1 : 0;
                if (c1 + c2 == 1)
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_2_1(string[] args)
        {
            var lines = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt");
            var res = 0;
            foreach (var line in lines)
            {
                var split = line.Split(new[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(split[0]);
                var max = int.Parse(split[1]);
                var ch = split[2][0];
                var psw = split[3];
                var count = psw.Count(x => x == ch);
                if (count >= min && count <= max)
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_1_2(string[] args)
        {
            var numbers = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt").Select(long.Parse).ToArray();

            for (int i = 0; i < numbers.Length - 2; i++)
            for (int j = i + 1; j < numbers.Length - 1; j++)
            for (int k = j + 1; k < numbers.Length; k++)
            {
                if (numbers[i] + numbers[j] + numbers[k] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j] * numbers[k]);
            }
        }

        static void Main_1_1(string[] args)
        {
            var numbers = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt").Select(long.Parse).ToArray();

            for (int i = 0; i < numbers.Length - 1; i++)
            for (int j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[i] + numbers[j] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j]);
            }
        }
    }
}
