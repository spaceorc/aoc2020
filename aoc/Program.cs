﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        static void Main_5_2()
        {
            var lines = File.ReadAllLines("day5.txt");

            var all = new HashSet<int>(lines.Select(Solve));

            const int min = 8;
            const int max = (1 << 10) - 8;
            for (var i = min; i <= max; i++)
            {
                if (!all.Contains(i) && all.Contains(i + 1) && all.Contains(i - 1))
                    Console.Out.WriteLine(i);
            }

            static int Solve(string line)
            {
                return Convert.ToInt32(
                    line.Replace('B', '1')
                        .Replace('R', '1')
                        .Replace('F', '0')
                        .Replace('L', '0'),
                    2);
            }
        }

        static void Main_5_1()
        {
            var lines = File.ReadAllLines("day5.txt");

            var res = lines.Select(Solve).Max();

            Console.Out.WriteLine(res);

            static int Solve(string line)
            {
                return Convert.ToInt32(
                    line.Replace('B', '1')
                        .Replace('R', '1')
                        .Replace('F', '0')
                        .Replace('L', '0'),
                    2);
            }
        }

        static void Main_4_2()
        {
            var passports = File.ReadAllText("day4.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split(new[] {'\n', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries));

            var res = 0;
            var req = new[] {"ecl", "pid", "eyr", "hcl", "byr", "iyr", /*"cid", */"hgt"};
            foreach (var passport in passports)
            {
                var dct = new Dictionary<string, string>();
                for (var i = 0; i < passport.Length; i += 2)
                {
                    var key = passport[i];
                    var value = passport[i + 1];
                    dct[key] = value;
                }

                if (req.All(r => dct.ContainsKey(r)))
                {
                    var invalid = false;
                    foreach (var (key, value) in dct)
                    {
                        if (invalid)
                            break;
                        switch (key)
                        {
                            case "byr":
                                if (!int.TryParse(value, out var byr) || byr < 1920 || byr > 2002)
                                    invalid = true;
                                break;
                            case "iyr":
                                if (!int.TryParse(value, out var iyr) || iyr < 2010 || iyr > 2020)
                                    invalid = true;
                                break;
                            case "eyr":
                                if (!int.TryParse(value, out var eyr) || eyr < 2020 || eyr > 2030)
                                    invalid = true;
                                break;
                            case "hgt":
                                if (value.EndsWith("cm") &&
                                    int.TryParse(value.Substring(0, value.Length - 2), out var cm) && cm >= 150 &&
                                    cm <= 193)
                                {
                                }
                                else if (value.EndsWith("in") &&
                                         int.TryParse(value.Substring(0, value.Length - 2), out var @in) && @in >= 59 &&
                                         @in <= 76)
                                {
                                }
                                else
                                    invalid = true;

                                break;
                            case "ecl":
                                if (!"amb blu brn gry grn hzl oth".Split().Contains(value))
                                    invalid = true;
                                break;
                            case "pid":
                                if (value.Length != 9 || !value.All(char.IsDigit))
                                    invalid = true;
                                break;
                            case "hcl":
                                if (!value.StartsWith("#") || !value.Substring(1).All(c =>
                                    char.IsDigit(c) || "abcdef".Contains(c)))
                                    invalid = true;
                                break;
                        }
                    }

                    if (!invalid)
                        res++;
                }
            }

            Console.Out.WriteLine(res);
        }

        static void Main_4_1()
        {
            var passports = File.ReadAllText("day4.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split(new[] {'\n', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries));

            var res = 0;
            var req = new[] {"ecl", "pid", "eyr", "hcl", "byr", "iyr", /*"cid", */"hgt"};
            foreach (var passport in passports)
            {
                var dct = new Dictionary<string, string>();
                for (var i = 0; i < passport.Length; i += 2)
                {
                    var key = passport[i];
                    var value = passport[i + 1];
                    dct[key] = value;
                }

                if (req.All(r => dct.ContainsKey(r)))
                    res++;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_3_2()
        {
            var lines = File.ReadAllLines("day3.txt");
            long res1 = Calc(1, 1);
            long res2 = Calc(3, 1);
            long res3 = Calc(5, 1);
            long res4 = Calc(7, 1);
            long res5 = Calc(1, 2);

            Console.Out.WriteLine(res1 * res2 * res3 * res4 * res5);

            int Calc(int dx, int dy)
            {
                var res = 0;

                var x = 0;
                var y = 0;
                while (y < lines.Length)
                {
                    if (lines[y][x] == '#')
                        res++;
                    y += dy;
                    x = (x + dx) % lines[0].Length;
                }

                return res;
            }
        }

        static void Main_3_1()
        {
            var lines = File.ReadAllLines("day3.txt");
            var res = 0;

            var x = 0;
            var y = 0;
            while (y < lines.Length)
            {
                if (lines[y][x] == '#')
                    res++;
                y++;
                x = (x + 3) % lines[0].Length;
            }

            Console.Out.WriteLine(res);
        }

        static void Main_2_2()
        {
            var lines = File.ReadAllLines("day2.txt");
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

        static void Main_2_1()
        {
            var lines = File.ReadAllLines("day2.txt");
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

        static void Main_1_2()
        {
            var numbers = File.ReadAllLines("day1.txt").Select(long.Parse).ToArray();

            for (var i = 0; i < numbers.Length - 2; i++)
            for (var j = i + 1; j < numbers.Length - 1; j++)
            for (var k = j + 1; k < numbers.Length; k++)
            {
                if (numbers[i] + numbers[j] + numbers[k] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j] * numbers[k]);
            }
        }

        static void Main_1_1()
        {
            var numbers = File.ReadAllLines("day1.txt").Select(long.Parse).ToArray();

            for (var i = 0; i < numbers.Length - 1; i++)
            for (var j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[i] + numbers[j] == 2020)
                    Console.Out.WriteLine(numbers[i] * numbers[j]);
            }
        }
    }
}