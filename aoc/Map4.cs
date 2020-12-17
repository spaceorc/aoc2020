using System;
using System.Collections.Generic;

namespace aoc
{
    public class Map4<T>
    {
        public readonly int size;
        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int sizeZ;
        public readonly int sizeW;
        public readonly int totalCount;
        public readonly T[] data;

        public Map4(int size)
            : this(size, size, size, size)
        {
            this.size = size;
        }

        public Map4(int sizeX, int sizeY, int sizeZ, int sizeW)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            this.sizeW = sizeW;
            totalCount = sizeX * sizeY * sizeZ * sizeW;
            data = new T[totalCount];
        }

        public T this[V4 v]
        {
            get => data[v.W * sizeX * sizeY * sizeZ + v.Z * sizeX * sizeY + v.Y * sizeX + v.X];
            set => data[v.W * sizeX * sizeY * sizeZ + v.Z * sizeX * sizeY + v.Y * sizeX + v.X] = value;
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

        public IEnumerable<V4> RangeNoBorders()
        {
            for (int w = 1; w < sizeW - 1; w++)
            for (int z = 1; z < sizeZ - 1; z++)
            for (int y = 1; y < sizeY - 1; y++)
            for (int x = 1; x < sizeX - 1; x++)
                yield return new V4(x, y, z, w);
        }

        public IEnumerable<V4> Range()
        {
            for (int w = 0; w < sizeW; w++)
            for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
            for (int x = 0; x < sizeX; x++)
                yield return new V4(x, y, z, w);
        }
    }
}