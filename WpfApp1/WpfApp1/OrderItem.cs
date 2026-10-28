namespace WpfApp1
{
    public class OrderItem
    {
        public string Naimenovanie { get; set; }
        public int Kolichestvo { get; set; }
        public decimal Tsena { get; set; }
        public decimal Summa { get; set; }

        public OrderItem()
        {
            Naimenovanie = "";
        }

        public string TsenaText { get { return Tsena.ToString("N2"); } }
        public string SummaText { get { return Summa.ToString("N2"); } }
    }
}