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
            var colors = new List<Color> { Colors.Red, Colors.Blue, Colors.Green };
            var myPalette = new BitmapPalette(colors);
            var rawData = new byte[N * N];
            int stride = N * 3 + (N * 3) % 4;


            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                   if (map[i , j] == 1)
                   {
                       SetPixel(ref rawData, i, j, stride, color);
                   }
                }
            }

           return  BitmapSource.Create(N, N, 96, 96, PixelFormats.Bgr32, myPalette, rawData, stride);
        }

         private void SetPixel(ref byte[] bits, int x, int y,int stride, Color c)
         {
             bits[x * 3 + y * stride] = c.R;
             bits[x * 3 + y * stride + 1] = c.G;
             bits[x * 3 + y * stride + 2] = c.B;
         }
    }
}
