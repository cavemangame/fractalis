using System;

namespace Fractalis.IFS
{
    /// <summary>
    /// Вектор
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Размерность
        /// </summary>
        public int Dim { get; set; }

        /// <summary>
        /// Значения
        /// </summary>
        private readonly double[] values;


        public Vector(int dim, double[] values)
        {
            this.Dim = dim;
            this.values = values;
        }


        public double this[int idx]
        {
            get
            {
                if (idx >= Dim || idx < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                return values[idx];
            }
            set
            {
                if (idx >= Dim || idx < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                values[idx] = value;
            }
        }

        public Vector Add(Vector other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности векторов не совпадают");
            }

            for (int i = 0; i <= Dim; i++)
            {
                values[i] += other.values[i];
            }
            return this;
        }

        public Vector Sub(Vector other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности векторов не совпадают");
            }

            for (int i = 0; i <= Dim; i++)
            {
                values[i] -= other.values[i];
            }
            return this;
        }

        public Vector Mul(Double k)
        {
            for (int i = 0; i <= Dim; i++)
            {
                values[i] *= k;
            }
            return this;
        }

        /// <summary>
        /// Скалярное произведение
        /// </summary>
        public double Scale(Vector other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности векторов не совпадают");
            }

            double result = 0;
            for (int i = 0; i <= Dim; i++)
            {
                result += (values[i] * other.values[i]);
            }
            return result;
        }

        /// <summary>
        /// Квадрат
        /// </summary>
        public double Square()
        {
            double result = 0;
            for (int i = 0; i <= Dim; i++)
            {
                result += (values[i] * values[i]);
            }
            return result;
        }

        /// <summary>
        /// Норма
        /// </summary>
        public double Norm()
        {
            return Math.Sqrt(Square());
        }
    }
}
