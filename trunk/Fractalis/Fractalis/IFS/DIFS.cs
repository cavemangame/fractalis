using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractalis.IFS
{
    /// <summary>
    /// Реализация детерминированного СИФ
    /// </summary>
    public class DIFS
    {
        public List<AffineMap> Axioms { get; set; }

        public BitmapSource GetAttractor(ScreenMapper start, int screenSize, int depth)
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

    }
}
