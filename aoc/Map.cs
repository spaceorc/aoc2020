using System;

namespace aoc
{
    public class Map<T>
    {
        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int totalCount;
        private readonly T[] data;

        public Map(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            totalCount = sizeX * sizeY;
            data = new T[totalCount];
        }

        public T this[V v]
        {
            get => data[v.Y * sizeX + v.X];
            set => data[v.Y * sizeX + v.X] = value;
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
    }
}