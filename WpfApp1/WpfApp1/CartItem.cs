namespace WpfApp1
{
    public class CartItem
    {
        public int KodTovara { get; set; }
        public string Naimenovanie { get; set; }
        public decimal Tsena { get; set; }
        public int Kolichestvo { get; set; }

        public CartItem()
        {
            Naimenovanie = "";
            Kolichestvo = 1;
        }

        public decimal Summa { get { return Tsena * Kolichestvo; } }
        public string TsenaText { get { return Tsena.ToString("N2"); } }
        public string SummaText { get { return Summa.ToString("N2"); } }
    }
}