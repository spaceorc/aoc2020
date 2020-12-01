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
            var numbers = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt").Select(long.Parse).ToArray();

            
        }

        static void Main1(string[] args)
        {
            var numbers = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt").Select(long.Parse).ToArray();

            for (int i = 0; i < numbers.Length - 2; i++)
            for (int k = i + 1; k < numbers.Length - 1; k++)
            for (int l = k + 1; l < numbers.Length; l++)
            {
                if (numbers[i] + numbers[k] + numbers[l] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[k] * numbers[l]) ;
            }
        }
    }
}