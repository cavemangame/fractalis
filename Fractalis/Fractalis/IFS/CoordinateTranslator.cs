using System;

namespace Fractalis.IFS
{
    /// <summary>
    /// Транслятор аффинных преобразований (из мировых в экранные, например)
    /// </summary>
    public static class CoordinateTranslator
    {
        /// <summary>
        /// Транслировать аффинное преобразование 
        /// </summary>
        /// <param name="map">аффинное преобразование</param>
        /// <param name="first">из координат (ограничивающий квадрат)</param>
        /// <param name="second">в координаты (ограничивающий квадрат)</param>
        /// <returns>Новое аффинное преобразование</returns>
        public static AffineMap Translate(AffineMap map, Matrix first, Matrix second)
        {
            if (first.Dim != 2 || second.Dim != 2)
            {
                throw new ArgumentException("Неверные размерности ограничивающих квадратов");
            }
            // TODO: check for invalid params
            double a, b, c, d, e, f;

            double M11 = (second[0, 1] - second[0, 0]) / (first[0, 1] - first[0, 0]);
            double M22 = (second[1, 1] - second[1, 0]) / (first[1, 1] - first[1, 0]);

            double w1 = second[0, 0] - M11 * first[0, 0];
            double w2 = second[1, 0] - M22 * first[1, 0];

            a = map.a;
            b = (M11 / M22) * map.b;
            c = (M22 / M11) * map.c;
            d = map.d;
            e = (1 - a) * w1 - b * w2 + M11 * map.e;
            f = -c * w1 + (1 - d) * w2 + M22 * map.f;

            return new AffineMap {a = a, b = b, c = c, d = d, e = e, f = f};
        } 
    }
}
