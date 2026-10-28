using System;

namespace WpfApp1
{
    public class Order
    {
        public int KodZakaza { get; set; }
        public string Klient { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string SposobDostavki { get; set; }
        public string Sotrudnik { get; set; }
        public decimal Summa { get; set; }

        public Order()
        {
            Klient = "";
            Status = "";
            SposobDostavki = "";
            Sotrudnik = "";
        }

        public string DataText { get { return Data.ToString("dd.MM.yyyy HH:mm"); } }
        public string SummaText { get { return Summa.ToString("N2") + " руб."; } }
    }
}