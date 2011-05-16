using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractalis.IFS
{
    public class RIFS : IFS
    {
        private AffineRandomGenerator generator;

        #region Overrides of IFS

        protected override BitmapSource GetAttractor(ScreenMapper start, int screenSize, int depth, Color fractalColor)
        {
            generator = new AffineRandomGenerator(Axioms);
            ScreenMapper T = start.Copy();
            int x = screenSize / 2;
            int y = x;

            // за проход - точка, потому мы пока умножим на 10к
            for (int i = 0; i < depth*10000 ; i++)
            {
                int k = Pick();
                int ii = (int)(Axioms[k].a * x + Axioms[k].b * y + Axioms[k].e);
                int jj = (int)(Axioms[k].c * x + Axioms[k].d * y + Axioms[k].f);
                if (0 <= ii && ii < screenSize && 0 <= jj && jj < screenSize)
                {
                    T[ii, jj] = 1;
                }
                x = ii;
                y = jj;
            }
            return T.GetBitmap(fractalColor);
        }

        private int Pick()
        {
            return (int)generator.GetNewRandom();
        }

        protected override ScreenMapper GetDefaultScreenMap(int n)
        {
            // начальная карта - одна точка (пусть середина)
            var screenMapper = new ScreenMapper(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    screenMapper[i, j] = ((i == n / 2 && j == n / 2) ? (byte)1 : (byte)0);
                }
            }
            return screenMapper;
        }

        #endregion
    }
}
