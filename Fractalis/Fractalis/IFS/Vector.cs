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
            Dim = dim;
            this.values = values;
        }

        public Vector(int dim)
        {
            Dim = dim;
            values = new double[Dim];
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

            var res = new Vector(Dim);
            for (int i = 0; i < Dim; i++)
            {
                res[i] = values[i] + other.values[i];
            }
            return res;
        }

        public Vector Sub(Vector other)
        {
            if (other.Dim != Dim)
            {
                throw new InvalidOperationException("Размерности векторов не совпадают");
            }

            var res = new Vector(Dim);
            for (int i = 0; i < Dim; i++)
            {
                res[i] = values[i] - other.values[i];
            }
            return res;
        }

        public Vector Mul(Double k)
        {
            var res = new Vector(Dim);
            for (int i = 0; i < Dim; i++)
            {
                res[i] = values[i] * k;
            }
            return res;
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
            for (int i = 0; i < Dim; i++)
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
            for (int i = 0; i < Dim; i++)
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

        public Vector Copy()
        {
            return new Vector(Dim, values);
        }

        #region Operators overrides

        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1.Sub(v2);
        }

        public static Vector operator *(Vector v1, double k)
        {
            return v1.Mul(k);
        }

        public static Vector operator *(double k, Vector v2)
        {
            return v2.Mul(k);
        }

        #endregion
    }
}
