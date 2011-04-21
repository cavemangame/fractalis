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
            var res = new Matrix(Dim);
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    res[i, j] = values[i, j] + other[i, j];
                }
            }
            return res;
        }

        public Matrix Sub(Matrix other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности матриц не совпадают");
            }
            var res = new Matrix(Dim);
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    res[i, j] = values[i, j] - other[i, j];
                }
            }
            return res;
        }

        public Matrix Mul(Double k)
        {
            var res = new Matrix(Dim);
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                   res[i, j] = values[i, j] * k;
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
            var res = new Matrix(Dim);
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < Dim; k++)
                    {
                        sum += values[i, k]*other[k, j];
                    }
                    res[i, j] = sum;
                }
            }

            return res;
        }

        public Vector Mul(Vector vec)
        {
            if (vec.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности матриц не совпадают");
            }
            var w = new Vector(Dim);
            for (int i = 0; i < Dim; i++)
            {
                double sum = 0;

                for (int j = 0; j < Dim; j++)
                {
                    sum += values[i, j] * vec[j];
                }
                w[i] = sum;
            }
   
            return w;
        }

        public Matrix Copy()
        {
            return new Matrix(Dim, values);
        }

        #region Operator overrides

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return m1.Add(m2);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1.Sub(m2);
        }

        public static Matrix operator *(Matrix m1, double k)
        {
            return m1.Mul(k);
        }

        public static Matrix operator *(double k, Matrix m2)
        {
            return m2.Mul(k);
        }

        public static Vector operator *(Matrix m1, Vector v1)
        {
            return m1.Mul(v1);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return m1.Mul(m2);
        }

        #endregion
    }
}
