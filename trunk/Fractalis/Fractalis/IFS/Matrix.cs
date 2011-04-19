using System;

namespace Fractalis.IFS
{
    /// <summary>
    /// Матрица n*n
    /// </summary>
    public class Matrix
    {
        public int Dim { get; set; }
        private readonly double[,] values;

        public Matrix(int dim, double[,] values)
        {
            this.Dim = dim;
            this.values = values;
        }

        public double this[int x, int y]
        {
            get
            {
                if (x >= Dim || x < 0 || y >= Dim || y < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                return values[x, y];
            }
            set
            {
                if (x >= Dim || x < 0 || y >= Dim || y < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                values[x, y] = value;
            }
        }
    }
}
