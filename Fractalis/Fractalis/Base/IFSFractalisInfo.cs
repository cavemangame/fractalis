using System;
using System.IO;
using System.Text;

namespace Fractalis.Base
{
    /// <summary>
    /// Инфа о фрактале, описанная с помощью IFS
    /// </summary>
    public class IFSFractalisInfo : FractalisInfo
    {        
        public string Rules { get; set; }

        public int Depth { get; set; }

        #region Overrides of FractalisInfo

        public override void SaveItself(StreamWriter sw)
        {
            sw.WriteLine(Name);
            sw.WriteLine(Depth);
            sw.WriteLine(Rules);
        }

        public override void LoadItself(StreamReader sr)
        {
            Name = sr.ReadLine();
            Depth = Convert.ToInt32(sr.ReadLine());
            var sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                if (String.IsNullOrEmpty(s))
                    break;
                sb.Append(s + Environment.NewLine);
            }
            Rules = sb.ToString();
        }

        #endregion
    }
}