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

            double M11 = (second[0, 1] - second[0, 0]) / (first[0, 1] - first[0, 0]);
            double M22 = (second[1, 1] - second[1, 0]) / (first[1, 1] - first[1, 0]);

            double w1 = second[0, 0] - M11 * first[0, 0];
            double w2 = second[1, 0] - M22 * first[1, 0];

            var M = new Matrix(2, new[,] { { M11, 0 }, { 0, M22 } });
            var Mret = new Matrix(2, new[,] { { 1 / M11, 0 }, { 0, 1 / M22 } });
            var w = new Vector(2, new[] {w1, w2});
            var E = new Matrix(2, new double[,] {{1, 0}, {0, 1}});

            // T' = A'x+a', где 
            // A' = MAM'(M' - обратная)
            // a' = (E-A')w + Ma
            var translatedMap = new AffineMap {M = M * map.M * Mret};
            translatedMap.W = (E - translatedMap.M) * w + M * map.W; 
            return translatedMap;
        } 

        public static bool TestTranslator()
        {
            var testMap = new AffineMap {a = 2, b = -1, c = 3, d = 4, e = 1, f = -1};
            var world = new Matrix(2, new double[,] { { 0, 8 }, {-2, 14 } });
            var screen = new Matrix(2, new double[,] { { 0, 640 }, { 480, 0 } });

            var translateMap = Translate(testMap, world, screen);
            var expectedMap = new AffineMap {a = 2, b = 2.6667, c = -1.125, d = 4, e = -1040, f = -1230};

            return expectedMap.AlmostEqual(translateMap);
        }
    }
}
