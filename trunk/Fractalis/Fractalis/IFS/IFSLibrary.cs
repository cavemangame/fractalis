using System;
using System.Collections.Generic;
using System.IO;

namespace Fractalis.IFS
{
    /// <summary>
    /// Библиотека фракталов, записанных в виде аффинных преобразований
    /// </summary>
    public class IFSLibrary
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public List<IFSInfo> Fractals { get; set; }

        public void SaveLibrary(string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                // инфа о самой библиотеке
                sw.WriteLine(Name);
                sw.WriteLine(Author);
                sw.WriteLine();

                // список фракталов
                foreach (var info in Fractals)
                {
                    sw.WriteLine(info.Name);
                    sw.WriteLine(info.Rules);
                    sw.WriteLine(info.Depth);
                    sw.WriteLine();
                }
                sw.Flush();
            }
        }

        public void LoadLibrary(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    // инфа о самой библиотеке
                    Name = sr.ReadLine();
                    Author = sr.ReadLine();
                    sr.ReadLine();


                    // список фракталов
                    Fractals = new List<IFSInfo>();
                    while (!sr.EndOfStream)
                    {
                        Fractals.Add(new IFSInfo
                        {
                            Name = sr.ReadLine(),
                            Rules = sr.ReadLine(),
                            Depth = Convert.ToInt32(sr.ReadLine())
                        });
                        sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при загрузке библиотеки фракталов: " + e.Message);
            }
        }
    }
}
