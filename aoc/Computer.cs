using System;
using System.Linq;

namespace aoc
{
    public struct Computer
    {
        public long ip;
        public long acc;

        public Computer(Computer copy)
            : this(copy.ip, copy.acc)
        {
        }

        public Computer(long ip, long acc)
        {
            this.ip = ip;
            this.acc = acc;
        }

        public bool IsTerminated(Instruction[] program) => ip >= program.Length; 

        public Computer Next(Instruction[] program)
        {
            return Eval(program[ip]);
        }

        public Computer Eval(Instruction instruction)
        {
            switch (instruction.type)
            {
                case IT.jmp:
                    return new Computer(this) {ip = ip + instruction.arg};
                case IT.nop:
                    return new Computer(this) {ip = ip + 1};
                case IT.acc:
                    return new Computer(this)
                    {
                        ip = ip + 1,
                        acc = acc + instruction.arg
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Instruction[] Parse(string[] lines)
        {
            return lines.Select(Instruction.Parse).ToArray();
        }
    }
}