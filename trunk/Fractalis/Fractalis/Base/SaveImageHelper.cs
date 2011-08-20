using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Fractalis.Base
{
    public class SaveImageHelper
    {
        public static void SaveImage(BitmapSource bitmap)
        {
            var dlg = new SaveFileDialog
                          {
                              FileName = "Fractalis",
                              DefaultExt = ".png",
                              FilterIndex = 1,
                              RestoreDirectory = true,
                              Filter = "PNG (.png)|*.png|BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpeg)|*.jpeg"
                          };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    BitmapEncoder encoder = GetEncoder(dlg.FilterIndex);
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(fileStream);
                }
            }
        }

        private static BitmapEncoder GetEncoder(int index)
        {
            switch (index)
            {
                case 1:
                    return new PngBitmapEncoder();
                case 2:
                    return new BmpBitmapEncoder();
                case 3:
                    return new GifBitmapEncoder();
                case 4:
                    return new JpegBitmapEncoder();
                default :
                    throw new NotImplementedException("Этот формат не поддерживается");
            }
        }
    }
}