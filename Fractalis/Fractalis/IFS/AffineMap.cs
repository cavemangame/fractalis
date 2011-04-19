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
        }

        public Vector W
        {
            get
            {
                return new Vector(2, new[] { e, f });
            }
        }
       
    }
}
