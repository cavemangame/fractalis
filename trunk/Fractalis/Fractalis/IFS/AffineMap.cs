using System;

namespace Fractalis.IFS
{
    /// <summary>
    /// Описание аффинного отображения вида T(x) = Mx + w
    /// </summary>
    public class AffineMap
    {
        // коэффициенты матрицы M
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double d { get; set; }

        // коэффициенты вектора w
        public double e { get; set; }
        public double f { get; set; }

        public Matrix M
        {
            get
            {
                return new Matrix(2, new[,] {{a, b}, {c, d}});
            }
            set
            {
                if (value.Dim != 2)
                {
                    throw new InvalidOperationException("Размерность матрицы M должна быть равна 2");
                }

                a = value[0, 0];
                b = value[0, 1];
                c = value[1, 0];
                d = value[1, 1];
            }
        }

        public Vector W
        {
            get
            {
                return new Vector(2, new[] { e, f });
            }
            set
            {
                if (value.Dim != 2)
                {
                    throw new InvalidOperationException("Размерность вектора W должна быть равна 2");
                }
                e = value[0];
                f = value[1];
            }
        }   

        public bool AlmostEqual(AffineMap other)
        {
            return Utils.AlmostEqual(a, other.a) && Utils.AlmostEqual(b, other.b) &&
                   Utils.AlmostEqual(c, other.c) && Utils.AlmostEqual(d, other.d) &&
                   Utils.AlmostEqual(e, other.e) && Utils.AlmostEqual(f, other.f);
        }
    }
}
