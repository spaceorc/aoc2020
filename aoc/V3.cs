using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public class V3 : IEquatable<V3>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public V3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(V3 other) => X == other.X && Y == other.Y && Z == other.Z;
        public override bool Equals(object obj) => obj is V3 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        public static bool operator ==(V3 a, V3 b) => a.Equals(b);
        public static bool operator !=(V3 a, V3 b) => !(a == b);
        public static V3 operator +(V3 a, V3 b) => new V3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static V3 operator -(V3 a, V3 b) => new V3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public int MLen() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        public static int DProd(V3 a, V3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public override string ToString() => $"{X} {Y} {Z}";

        public IEnumerable<V3> NearsAndSelf()
        {
            for (var z = -1; z <= 1; z++)
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++)
                yield return this + new V3(x, y, z);
        }

        public IEnumerable<V3> Nears() => NearsAndSelf().Where(x => x != this).ToArray();
    }
}