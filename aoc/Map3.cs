using System;
using System.Collections.Generic;

namespace aoc
{
    public class Map3<T>
    {
        public readonly int size;
        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int sizeZ;
        public readonly int totalCount;
        public readonly T[] data;

        public Map3(int size)
            : this(size, size, size)
        {
            this.size = size;
        }

        public Map3(int sizeX, int sizeY, int sizeZ)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            totalCount = sizeX * sizeY * sizeZ;
            data = new T[totalCount];
        }

        public T this[V3 v]
        {
            get => data[v.Z * sizeX * sizeY + v.Y * sizeX + v.X];
            set => data[v.Z * sizeX * sizeY + v.Y * sizeX + v.X] = value;
        }

        public T this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public void Clear()
        {
            Array.Fill(data, default);
        }

        public void Fill(T value)
        {
            Array.Fill(data, value);
        }

        public IEnumerable<V3> RangeNoBorders()
        {
            for (var z = 1; z < sizeZ - 1; z++)
            for (var y = 1; y < sizeY - 1; y++)
            for (var x = 1; x < sizeX - 1; x++)
                yield return new V3(x, y, z);
        }

        public IEnumerable<V3> Range()
        {
            for (var z = 0; z < sizeZ; z++)
            for (var y = 0; y < sizeY; y++)
            for (var x = 0; x < sizeX; x++)
                yield return new V3(x, y, z);
        }
    }
}