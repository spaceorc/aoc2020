using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace aoc
{
    /// <summary>
    /// alternative solution for day 18 
    /// </summary>
    public class N
    {
        public readonly long n;
        public N(long n) => this.n = n;
        public static N operator *(N a, N b) => new N(a.n + b.n);
        public static N operator +(N a, N b) => new N(a.n + b.n);
        public static N operator -(N a, N b) => new N(a.n * b.n);
        public override string ToString() => n.ToString();

        public static void Solve1()
        {
            var lines = File.ReadAllLines("day18.txt");
            Console.Out.WriteLine(lines.Sum(Calc));

            long Calc(string expression)
            {
                expression = new Regex(@"\d+").Replace(expression.Replace("*", "-"), m => $"new N({m.Value})");
                var res = Eval(expression);
                return ((N) res).n;
            }
        }

        public static void Solve2()
        {
            var lines = File.ReadAllLines("day18.txt");
            Console.Out.WriteLine(lines.Sum(Calc));

            long Calc(string expression)
            {
                expression = new Regex(@"\d+").Replace(expression.Replace("*", "-").Replace("+", "*"),
                    m => $"new N({m.Value})");
                var res = Eval(expression);
                return ((N) res).n;
            }
        }

        private static object Eval(string expression)
        {
            return CSharpScript.EvaluateAsync(
                expression,
                ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()).WithImports("aoc")
            ).GetAwaiter().GetResult();
        }
    }
}