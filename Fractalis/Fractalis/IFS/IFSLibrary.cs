using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                    sw.WriteLine(info.Depth);
                    sw.WriteLine(info.Rules);
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
                        var info = new IFSInfo()
                                       {
                                           Name = sr.ReadLine(),
                                           Depth = Convert.ToInt32(sr.ReadLine())
                                       };

                        var sb = new StringBuilder();
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            if (String.IsNullOrEmpty(s))
                                break;
                            sb.Append(s + Environment.NewLine);
                        }
                        info.Rules = sb.ToString();
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
