namespace Fractalis.Modules
{
    /// <summary>
    /// Инфо о фрактале
    /// </summary>
    public class FractalisInfo
    {
        public string Name { get; set; }

        public string Axiom { get; set; }

        public string FRule { get; set; }

        public string BRule { get; set; }

        public string AddRules { get; set; }
        
        public double Angle { get; set; }

        public double BeginAngle { get; set; }

        public int Depth { get; set; }
    }
}
