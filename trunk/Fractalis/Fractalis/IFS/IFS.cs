using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractalis.IFS
{
    /// <summary>
    /// База для алгоритмов построения аттракторов
    /// </summary>
    public class IFS
    {
        /// <summary>
        /// Список аксиом
        /// </summary>
        public List<AffineMap> Axioms { get; set; }

        /// <summary>
        /// Метод, выдающий битмап с аттрактором
        /// переопределен в конкретных алгоритмах
        /// </summary>
        /// <param name="start">Начальный битмап</param>
        /// <param name="screenSize">Размер квадратного экрана</param>
        /// <param name="depth">глубина прорисовки (число итераций алгоритма)</param>
        /// <param name="fractalColor">Цвет</param>
        /// <returns></returns>
        protected virtual BitmapSource GetAttractor(ScreenMapper start, int screenSize, int depth, Color fractalColor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Выдает начальную карту для алгоритма
        /// </summary>
        /// <param name="screenSize">Размер квадратного экрана</param>
        /// <returns></returns>
        protected virtual ScreenMapper GetDefaultScreenMap(int screenSize)
        {
            throw new NotImplementedException();
        }

        #region для общего пользования

        /// <summary>
        /// Получение аттрактора
        /// </summary>
        /// <param name="screenSize">Размер квадратного экрана</param>
        /// <param name="depth">глубина прорисовки (число итераций алгоритма)</param>
        /// <param name="fractalColor">Цвет</param>
        /// <returns></returns>
        public BitmapSource GetAttractor(int screenSize, int depth, Color fractalColor)
        {
            return GetAttractor(GetDefaultScreenMap(screenSize), screenSize, depth, fractalColor);
        }

        /// <summary>
        /// Парсим строку входных преобразований, и транслируем их на экран [0, size][0, size]
        /// </summary>
        /// <param name="text">правила</param>
        /// <param name="size">размеры квадратного экрана</param>
        public void ParseAndTranslateFunctions(string text, double size)
        {
            if (String.IsNullOrEmpty(text))
                return;
            // TODO: fix, split by newline
            string[] rules = text.Split();
            if (rules.Length == 0)
                return;

            var world = new Matrix(2, new double[,] { { 0, 1 }, { 0, 1 } });
            var screen = new Matrix(2, new[,] { { 0, size }, { 0, size } });
            Axioms = new List<AffineMap>(rules.Length);

            foreach (var rule in rules)
            {
                AffineMap affineMap = ParseRule(rule);
                if (affineMap != null)
                {
                    Axioms.Add(CoordinateTranslator.Translate(affineMap, world, screen));
                }
            }
        }

        private static AffineMap ParseRule(string rule)
        {
            string[] koef = rule.Split(';');
            if (koef.Length != 6)
                return null;
            // просто аффинные коэффициенты через ";"
            var map = new AffineMap
            {
                a = Convert.ToDouble(koef[0]),
                b = Convert.ToDouble(koef[1]),
                c = Convert.ToDouble(koef[2]),
                d = Convert.ToDouble(koef[3]),
                e = Convert.ToDouble(koef[4]),
                f = Convert.ToDouble(koef[5])
            };

            return map;
        }

        #endregion
    }
}
