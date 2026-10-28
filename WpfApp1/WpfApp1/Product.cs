namespace WpfApp1
{
    public class Product
    {
        public int KodTovara { get; set; }
        public string Naimenovanie { get; set; }
        public string Ispolnitel { get; set; }
        public string Kategoriya { get; set; }
        public string Zhanr { get; set; }
        public int GodVypuska { get; set; }
        public decimal TsenaRoznitsa { get; set; }
        public decimal TsenaOpt { get; set; }
        public int Ostatki { get; set; }

        public Product()
        {
            Naimenovanie = string.Empty;
            Ispolnitel = string.Empty;
            Kategoriya = string.Empty;
            Zhanr = string.Empty;
        }

        public string TsenaText
        {
            get { return TsenaRoznitsa.ToString("N2"); }
        }
    }
}