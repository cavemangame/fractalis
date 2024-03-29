﻿using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractalis.IFS
{
    /// <summary>
    /// Реализация детерминированного СИФ
    /// </summary>
    public class DIFS : IFS
    {
        /// <summary>
        /// Опеределяет начальное множество как линию
        /// </summary>
        public bool IsLine { get; set; }

        public DIFS(bool isLine)
        {
            IsLine = isLine;
        }

        protected override BitmapSource GetAttractor(ScreenMapper start, int screenSize, int depth, Color fractalColor)
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
                                double ii = Axioms[l].a * i + Axioms[l].b * j + Axioms[l].e + 1;
                                if (0 <= ii && ii < screenSize)
                                {
                                    double jj = Axioms[l].c * i + Axioms[l].d * j + Axioms[l].f + 1;
                                    if (0 <= jj && jj < screenSize)
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
            return T.GetBitmap(fractalColor);
        }

        protected override ScreenMapper GetDefaultScreenMap(int n)
        {
            if (!IsLine)
            {
                return GetPlaneDefaultScreen(n);
            }
            return GetLineDefaultScreen(n);
        }

        private ScreenMapper GetLineDefaultScreen(int n)
        {
            // начальная карта линия посредине
            var screenMapper = new ScreenMapper(n);
            for (int i = 0; i < n; i++)
            {
                screenMapper[i, n/2] = 1;
            }
            return screenMapper;
        }

        private ScreenMapper GetPlaneDefaultScreen(int n)
        {
            // начальная карта всеь квадрат
            var screenMapper = new ScreenMapper(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    screenMapper[i, j] = 1;
                }
            }
            return screenMapper;
        }
    }
}
