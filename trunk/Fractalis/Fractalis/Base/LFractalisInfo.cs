using System;
using System.IO;

namespace Fractalis.Base
{
    /// <summary>
    /// Инфо о фрактале на L-языке
    /// </summary>
    public class LFractalisInfo : FractalisInfo
    {
        public string Axiom { get; set; }

        public string FRule { get; set; }

        public string BRule { get; set; }

        public string AddRules { get; set; }
        
        public double Angle { get; set; }

        public double BeginAngle { get; set; }

        public int Depth { get; set; }

        #region Overrides of FractalisInfo

        public override void SaveItself(StreamWriter sw)
        {
            sw.WriteLine(Name);
            sw.WriteLine(Axiom);
            sw.WriteLine(FRule);
            sw.WriteLine(BRule);
            sw.WriteLine(AddRules);
            sw.WriteLine(Angle);
            sw.WriteLine(BeginAngle);
            sw.WriteLine(Depth);
            sw.WriteLine(); //TODO: kill it
        }

        public override void LoadItself(StreamReader sr)
        {
            Name = sr.ReadLine();
            Axiom = sr.ReadLine();
            FRule = sr.ReadLine();
            BRule = sr.ReadLine();
            AddRules = sr.ReadLine();
            Angle = Convert.ToDouble(sr.ReadLine());
            BeginAngle = Convert.ToDouble(sr.ReadLine());
            Depth = Convert.ToInt32(sr.ReadLine());
            sr.ReadLine();
        }

        #endregion
    }
}