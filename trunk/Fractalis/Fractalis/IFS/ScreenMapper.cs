using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractalis.IFS
{
    /// <summary>
    /// Класс - отображение битовой карты на экран
    /// </summary>
    public class ScreenMapper
    {
        /// <summary>
        /// Размер экрана
        /// </summary>
        public int N { get; set; }

        private byte[,] map;

        public ScreenMapper(int n)
        {
            N = n;
            map = new byte[n, n];
        }

        public byte this[int x, int y]
        {
            get
            {
                if (x >= N || x < 0 || y >= N || y < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                return map[x, y];
            }
            set
            {
                if (x >= N || x < 0 || y >= N || y < 0)
                {
                    throw new ArgumentException("Индекс за пределами массива");
                }
                map[x, y] = value;
            }
        }

        public ScreenMapper Copy()
        {
            return new ScreenMapper(N) {map = map};
        }

        public BitmapSource GetBitmap(Color color)
        {
            PixelFormat pf = PixelFormats.Rgb24;
            int stride = (N * pf.BitsPerPixel + 7) / 8;

            var rawData = new byte[stride * N];

            // set all to white
            for (int i = 0; i < stride * N; i++)
            {
                rawData[i] = 255;
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                   if (map[i , j] == 1)
                   {
                       SetPixel(rawData, i, j, stride, color);
                   }
                }
            }

            return BitmapSource.Create(N, N, 96, 96, pf, null, rawData, stride);
        }

         private void SetPixel(byte[] bits, int x, int y, int stride, Color c)
         {
             bits[x * 3 + y * stride] = c.R;
             bits[x * 3 + y * stride + 1] = c.G;
             bits[x * 3 + y * stride + 2] = c.B;
         }
    }
}
