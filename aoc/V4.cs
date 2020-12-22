using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public class V4 : IEquatable<V4>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;
        public readonly int W;

        public V4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public bool Equals(V4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        public override bool Equals(object obj) => obj is V4 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                hashCode = (hashCode * 397) ^ W;
                return hashCode;
            }
        }

        public static bool operator ==(V4 a, V4 b) => a.Equals(b);
        public static bool operator !=(V4 a, V4 b) => !(a == b);
        public static V4 operator +(V4 a, V4 b) => new V4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static V4 operator -(V4 a, V4 b) => new V4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

        public override string ToString() => $"{X} {Y} {Z} {W}";

        public IEnumerable<V4> NearsAndSelf()
        {
            for (var w = -1; w <= 1; w++)
            for (var z = -1; z <= 1; z++)
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++)
                yield return this + new V4(x, y, z, w);
        }

        public IEnumerable<V4> Nears() => NearsAndSelf().Where(x => x != this).ToArray();
    }
}