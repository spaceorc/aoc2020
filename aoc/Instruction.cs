using System;

namespace aoc
{
    public class Instruction
    {
        public IT type;
        public long arg;

        public Instruction(IT type, long arg)
        {
            this.type = type;
            this.arg = arg;
        }

        public static Instruction Parse(string lines)
        {
            var split = lines.Split();
            return new Instruction(Enum.Parse<IT>(split[0]), long.Parse(split[1]));
        }
    }
}