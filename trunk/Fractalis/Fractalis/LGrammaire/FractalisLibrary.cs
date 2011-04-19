using System;
using System.Collections.Generic;
using System.IO;

namespace Fractalis.LGrammaire
{
    /// <summary>
    /// Описание библиотеки фракталов
    /// </summary>
    public class FractalisLibrary
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public List<FractalisInfo> Fractals { get; set; }


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
                    sw.WriteLine(info.Axiom);
                    sw.WriteLine(info.FRule);
                    sw.WriteLine(info.BRule);
                    sw.WriteLine(info.AddRules);
                    sw.WriteLine(info.Angle);
                    sw.WriteLine(info.BeginAngle);
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
                    Fractals = new List<FractalisInfo>();
                    while (!sr.EndOfStream)
                    {
                        Fractals.Add(new FractalisInfo
                                         {
                                             Name = sr.ReadLine(),
                                             Axiom = sr.ReadLine(),
                                             FRule = sr.ReadLine(),
                                             BRule = sr.ReadLine(),
                                             AddRules = sr.ReadLine(),
                                             Angle = Convert.ToDouble(sr.ReadLine()),
                                             BeginAngle = Convert.ToDouble(sr.ReadLine()),
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