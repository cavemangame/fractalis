using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace Fractalis.IFS
{
    /// <summary>
    /// Реализация детерминированного СИФ
    /// TODO: вынести базовый для DIFS и RIFS класс
    /// </summary>
    public class DIFS
    {
        public List<AffineMap> Axioms { get; set; }

        private BitmapSource GetAttractor(ScreenMapper start, int screenSize, int depth)
        {
            var S = new ScreenMapper(screenSize);
            ScreenMapper T = start.Copy();

            for (int k = 0; k < depth; k++)
            {
                for (int i = 0; i < screenSize; i++)
                {
                    for (int j = 0; j < screenSize; j++)
                    {
                        if (T[i, j] == 1)
                        {
                            for (int l = 0; l < Axioms.Count; l++)
                            {
                                double ii = Axioms[l].a*i + Axioms[l].b*j + Axioms[l].e + 1;
                                if (1 <= ii && ii <= screenSize)
                                {
                                    double jj = Axioms[l].c * i + Axioms[l].d * j + Axioms[l].f + 1;
                                    if (1 <= jj && jj <= screenSize)
                                    {
                                        S[(int)ii, (int)jj] = 1;
                                    }
                                }
                            }
                        }
                    }

                }
                T = S.Copy();
                S = new ScreenMapper(screenSize);
            }
            return T.GetBitmap(Colors.Black);
        }

         public BitmapSource GetAttractor(int screenSize, int depth)
         {
             return GetAttractor(GetDefaultScreenMap(screenSize), screenSize, depth);
         }

         private ScreenMapper GetDefaultScreenMap(int n)
         {
             // начальная карта - весь квадрат
             ScreenMapper screenMapper = new ScreenMapper(n);
             for (int i = 0; i < n; i++)
             {
                 for (int j = 0; j < n; j++)
                 {
                     screenMapper[i, j] = 1;
                 }
             }
             return screenMapper;
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

        private AffineMap ParseRule(string rule)
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
    }
}
