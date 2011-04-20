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
            Dim = dim;
            this.values = values;
        }

        public Matrix(int dim)
        {
            Dim = dim;
            values = new double[dim,dim];
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

        public Matrix Add(Matrix other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности матриц не совпадают");
            }
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    values[i, j] += other[i, j];
                }
            }
            return this;
        }

        public Matrix Sub(Matrix other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности матриц не совпадают");
            }
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    values[i, j] -= other[i, j];
                }
            }
            return this;
        }

        public Matrix Mul(Double k)
        {
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    values[i, j] *= k;
                }
            }
            return this;
        }

        public Matrix Mul(Matrix other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности матриц не совпадают");
            }
            var C = new Matrix(Dim);
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < Dim; k++)
                    {
                        sum += values[i, k]*other[k, j];
                    }
                    C[i, j] = sum;
                }
            }

            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    values[i, j] = C[i, j];
                }
            }
            return this;

        }
    }
}
