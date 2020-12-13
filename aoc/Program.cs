using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    class Program
    {
        static void Main()
        {
            var lines = File.ReadAllLines("/Users/spaceorc/Downloads/input.txt")
                .ToArray();

            long res = 0;
            Console.Out.WriteLine(res);
        }

        static void Main_13_2()
        {
            var lines = File.ReadAllLines("day13.txt");

            // x*N = b mod K
            var nums = lines[1].Split(',');

            var eqs = new List<(long x, long b, long K)>();

            var first = long.Parse(nums[0]);
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i] == "x")
                    continue;

                var next = long.Parse(nums[i]);
                long delta = i;

                var b = (next - delta % next) % next;
                eqs.Add((first % next, b, next));
            }

            var results = new List<long>();
            var results2 = new List<long>();

            while (eqs.Count > 1)
            {
                var nextEqs = new List<(long x, long b, long K)>();
                for (int i = 0; i < eqs.Count; i++)
                {
                    var (x, b, K) = eqs[i];

                    // N = x^-1 * b mod K
                    var N = Inv(x, K) * b % K;

                    if (i == 0)
                    {
                        results.Add(N);
                        results2.Add(K);
                    }
                    else
                        nextEqs.Add((results2.Last() % K, (N % K + K - results.Last() % K) % K, K));
                }

                eqs = nextEqs;    
            }

            var (xx, bb, KK) = eqs.Last();
            var NN = Inv(xx, KK) * bb % KK;
            for (int i = results.Count - 1; i >= 0; i--)
                NN = NN * results2[i] + results[i];

            Console.Out.WriteLine(first * NN);
            
            static long Bp(long a, long n, long mod)
            {
                long res = 1;
                while (n != 0)
                {
                    if ((n & 1) != 0)
                        res = res * a % mod;
                    a = a * a % mod;
                    n >>= 1;
                }

                return res;
            }

            static long Inv(long x, long mod)
            {
                return Bp(x, mod - 2, mod);
            }
        }

        static void Main_13_1()
        {
            var lines = File.ReadAllLines("day13.txt");

            var ts = long.Parse(lines[0]);
            var nums = lines[1].Split(',').Where(x => x != "x").Select(long.Parse).ToArray();

            var array = nums.Select(x => new {r = ts % x == 0 ? ts % x : x - ts % x, x}).ToArray();
            var min = array.OrderBy(x => x.r).First();
            Console.Out.WriteLine(min.r * min.x);
        }

        static void Main_12_2()
        {
            var lines = File.ReadAllLines("day12.txt")
                .Select(x => (cmd: x[0], n: int.Parse(x.Substring(1))))
                .ToArray();

            var dv = new[] {new V(0, -1), new V(1, 0), new V(0, 1), new V(-1, 0)};
            var pos = new V(0, 0);
            var vp = new V(10, -1);

            foreach (var (cmd, n) in lines)
            {
                switch (cmd)
                {
                    case 'F':
                        pos += vp * n;
                        break;
                    case 'N':
                        vp += dv[0] * n;
                        break;
                    case 'E':
                        vp += dv[1] * n;
                        break;
                    case 'S':
                        vp += dv[2] * n;
                        break;
                    case 'W':
                        vp += dv[3] * n;
                        break;
                    case 'L':
                        switch (n / 90 % 4)
                        {
                            case 0:
                                break;
                            case 2:
                                vp = -vp;
                                break;
                            case 1:
                                vp = new V(vp.Y, -vp.X);
                                break;
                            case 3:
                                vp = new V(-vp.Y, vp.X);
                                break;
                        }

                        break;
                    case 'R':
                        switch (n / 90 % 4)
                        {
                            case 0:
                                break;
                            case 2:
                                vp = -vp;
                                break;
                            case 3:
                                vp = new V(vp.Y, -vp.X);
                                break;
                            case 1:
                                vp = new V(-vp.Y, vp.X);
                                break;
                        }

                        break;
                }
            }

            Console.Out.WriteLine(pos.MLen());
        }

        static void Main_12_1()
        {
            var lines = File.ReadAllLines("day12.txt")
                .Select(x => (cmd: x[0], n: int.Parse(x.Substring(1))))
                .ToArray();

            var delta = new[] {new V(0, -1), new V(1, 0), new V(0, 1), new V(-1, 0)};
            var dir = 1;
            var pos = new V(0, 0);

            foreach (var (cmd, n) in lines)
            {
                switch (cmd)
                {
                    case 'F':
                        pos += delta[dir] * n;
                        break;
                    case 'N':
                        pos += delta[0] * n;
                        break;
                    case 'E':
                        pos += delta[1] * n;
                        break;
                    case 'S':
                        pos += delta[2] * n;
                        break;
                    case 'W':
                        pos += delta[3] * n;
                        break;
                    case 'L':
                        dir = (dir + 4 - n / 90 % 4) % 4;
                        break;
                    case 'R':
                        dir = (dir + n / 90) % 4;
                        break;
                }
            }

            Console.Out.WriteLine(pos.MLen());
        }

        static void Main_11_2()
        {
            var lines = File.ReadAllLines("day11.txt")
                .Select(x => x.ToCharArray()).ToArray();

            var nears = new List<V>[lines.Length, lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[0].Length; x++)
                nears[y, x] = CalcNears(x, y);

            var hash = CalcHash();
            while (true)
            {
                lines = Sim();
                var nextHash = CalcHash();
                if (nextHash == hash)
                    break;

                hash = nextHash;
            }

            Console.Out.WriteLine(lines.SelectMany(x => x).Count(c => c == '#'));

            int CalcHash()
            {
                var res = 0;
                foreach (var l in lines)
                    res = HashCode.Combine(res, new string(l).GetHashCode());

                return res;
            }

            char[][] Sim()
            {
                var next = lines.Select(x => x.ToArray()).ToArray();
                for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[0].Length; x++)
                    next[y][x] = Calc(x, y);

                return next;
            }

            char Calc(int x, int y)
            {
                if (lines[y][x] == '.')
                    return '.';

                int cnt = 0;
                foreach (var near in nears[y, x])
                {
                    if (lines[near.Y][near.X] == '#')
                        cnt++;
                }

                if (lines[y][x] == 'L' && cnt == 0)
                    return '#';

                if (lines[y][x] == '#' && cnt >= 5)
                    return 'L';

                return lines[y][x];
            }

            List<V> CalcNears(int x, int y)
            {
                var dvs = new[]
                {
                    new V(-1, -1), new V(-1, 0), new V(-1, 1), new V(0, 1), new V(1, 1), new V(1, 0), new V(1, -1),
                    new V(0, -1)
                };

                var res = new List<V>();

                foreach (var dv in dvs)
                {
                    var v = new V(x, y);
                    while (true)
                    {
                        v += dv;

                        if (v.X < 0)
                            break;
                        if (v.X >= lines[0].Length)
                            break;
                        if (v.Y < 0)
                            break;
                        if (v.Y >= lines.Length)
                            break;

                        if (lines[v.Y][v.X] == '.')
                            continue;

                        res.Add(v);
                        break;
                    }
                }

                return res;
            }
        }

        static void Main_11_1()
        {
            var lines = File.ReadAllLines("day11.txt")
                .Select(x => x.ToCharArray()).ToArray();

            var hash = CalcHash();
            while (true)
            {
                lines = Sim();
                var nextHash = CalcHash();
                if (nextHash == hash)
                    break;

                hash = nextHash;
            }

            Console.Out.WriteLine(lines.SelectMany(x => x).Count(c => c == '#'));

            int CalcHash()
            {
                var res = 0;
                foreach (var l in lines)
                    res = HashCode.Combine(res, new string(l).GetHashCode());

                return res;
            }

            char[][] Sim()
            {
                var next = lines.Select(x => x.ToArray()).ToArray();
                for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[0].Length; x++)
                    next[y][x] = Calc(x, y);

                return next;
            }

            char Calc(int x, int y)
            {
                if (lines[y][x] == '.')
                    return '.';

                int cnt = 0;
                for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (x + dx < 0)
                        continue;
                    if (x + dx >= lines[0].Length)
                        continue;
                    if (y + dy < 0)
                        continue;
                    if (y + dy >= lines.Length)
                        continue;

                    if (lines[y + dy][x + dx] == '#')
                        cnt++;
                }

                if (lines[y][x] == 'L' && cnt == 0)
                    return '#';

                if (lines[y][x] == '#' && cnt >= 4)
                    return 'L';

                return lines[y][x];
            }
        }

        static void Main_10_2()
        {
            var nums = File.ReadAllLines("day10.txt")
                .Select(long.Parse)
                .OrderBy(x => x)
                .ToList();
            nums.Insert(0, 0);
            nums.Add(nums.Last() + 3);

            var res = new Dictionary<int, long>();

            Console.Out.WriteLine(Calc(nums.Count - 1));

            long Calc(int last)
            {
                if (last == 0)
                    return 1;

                if (res.TryGetValue(last, out var result))
                    return result;

                var count = 0L;
                for (var i = last - 1; i >= 0; i--)
                {
                    if (nums[i] < nums[last] - 3)
                        break;

                    count += Calc(i);
                }

                return res[last] = count;
            }
        }

        static void Main_10_1()
        {
            var nums = File.ReadAllLines("day10.txt")
                .Select(long.Parse)
                .OrderBy(x => x)
                .ToList();
            nums.Insert(0, 0);
            nums.Add(nums.Last() + 3);

            var d1 = 0;
            var d3 = 0;
            for (var i = 0; i < nums.Count - 1; i++)
            {
                if (nums[i + 1] - nums[i] == 1)
                    d1++;
                if (nums[i + 1] - nums[i] == 3)
                    d3++;
            }

            Console.Out.WriteLine(d1 * d3);
        }

        static void Main_9_2()
        {
            var nums = File.ReadAllLines("day9.txt")
                .Select(long.Parse)
                .ToArray();

            var res = Find();

            for (var i = 0; i < nums.Length; i++)
            {
                long sum = 0;
                for (var j = i; j < nums.Length; j++)
                {
                    sum += nums[j];
                    if (sum == res)
                    {
                        var range = nums.Skip(i).Take(j - i + 1).ToArray();
                        Console.Out.WriteLine(range.Min() + range.Max());
                        return;
                    }

                    if (sum > res)
                        break;
                }
            }

            long Find()
            {
                for (var i = 25; i < nums.Length; i++)
                {
                    var range = nums.Skip(i - 25).Take(25).ToArray();
                    var ok = false;
                    for (var j = 0; j < range.Length - 1; j++)
                    for (var k = j + 1; k < range.Length; k++)
                    {
                        if (range[j] + range[k] == nums[i])
                        {
                            ok = true;
                            break;
                        }
                    }

                    if (!ok)
                        return nums[i];
                }

                throw new Exception("WTF???");
            }
        }

        static void Main_9_1()
        {
            var nums = File.ReadAllLines("day9.txt")
                .Select(long.Parse)
                .ToArray();

            for (var i = 25; i < nums.Length; i++)
            {
                var range = nums.Skip(i - 25).Take(25).ToArray();
                var ok = false;
                for (var j = 0; j < range.Length - 1; j++)
                for (var k = j + 1; k < range.Length; k++)
                {
                    if (range[j] + range[k] == nums[i])
                    {
                        ok = true;
                        break;
                    }
                }

                if (!ok)
                    Console.Out.WriteLine(nums[i]);
            }
        }

        static void Main_8_2()
        {
            var program = Computer.Parse(File.ReadAllLines("day8.txt"));

            foreach (var line in program)
            {
                if (line.type == IT.jmp)
                {
                    line.type = IT.nop;
                    var x = Eval();
                    if (x != long.MinValue)
                    {
                        Console.Out.WriteLine(x);
                        return;
                    }

                    line.type = IT.jmp;
                }
                else if (line.type == IT.nop)
                {
                    line.type = IT.jmp;
                    var x = Eval();
                    if (x != long.MinValue)
                    {
                        Console.Out.WriteLine(x);
                        return;
                    }

                    line.type = IT.nop;
                }
            }

            long Eval()
            {
                var computer = new Computer();
                var used = new HashSet<long>();
                while (!computer.IsTerminated(program))
                {
                    if (computer.ip < 0)
                        return long.MinValue;

                    if (!used.Add(computer.ip))
                        return long.MinValue;

                    computer = computer.Next(program);
                }

                return computer.acc;
            }
        }

        static void Main_8_1()
        {
            var program = Computer.Parse(File.ReadAllLines("day8.txt"));

            var computer = new Computer();
            var used = new HashSet<long>();
            while (!computer.IsTerminated(program))
            {
                if (!used.Add(computer.ip))
                {
                    Console.Out.WriteLine(computer.acc);
                    return;
                }

                computer = computer.Next(program);
            }
        }

        static void Main_7_2()
        {
            var rules = File.ReadAllLines("day7.txt")
                .Select(r => r.Split(" bags contain "))
                .Select(r => new
                {
                    outer = r[0],
                    inner = r[1]
                        .Replace(" bags", "")
                        .Replace(" bag", "")
                        .Split(new[] {',', '.'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim())
                        .Where(v => v != "no other")
                        .Select(v => v.Split(' ', 2))
                        .Select(v => (count: int.Parse(v[0]), color: v[1]))
                        .ToList()
                })
                .ToDictionary(x => x.outer, x => x.inner);


            Console.Out.WriteLine(Calc("shiny gold"));

            long Calc(string outer)
            {
                var res = 0L;
                foreach (var (count, color) in rules[outer])
                    res += count + count * Calc(color);

                return res;
            }
        }

        static void Main_7_1()
        {
            var rules = File.ReadAllLines("day7.txt")
                .Select(r => r.Split(" bags contain "))
                .Select(r => new {outer = r[0], inner = r[1]})
                .ToArray();


            var queue = new Queue<string>();
            queue.Enqueue("shiny gold");
            var used = new HashSet<string>();
            used.Add("shiny gold");
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var next in rules.Where(x => x.inner.Contains(cur)))
                {
                    if (used.Add(next.outer))
                        queue.Enqueue(next.outer);
                }
            }


            Console.Out.WriteLine(used.Count - 1);
        }

        static void Main_6_2()
        {
            var groups = File.ReadAllText("day6.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            var res = groups.Select(Solve).Sum();
            Console.Out.WriteLine(res);

            static int Solve(string[] gr)
            {
                var r = gr[0].ToHashSet();
                foreach (var item in gr)
                    r.IntersectWith(item);

                return r.Count;
            }
        }

        static void Main_6_1()
        {
            var groups = File.ReadAllText("day6.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace("\n", "").Trim());

            var res = groups.Select(x => x.Distinct().Count()).Sum();
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