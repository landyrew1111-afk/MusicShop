namespace WpfApp1
{
    public class User
    {
        public int KodPolzovatelya { get; set; }
        public string Familiya { get; set; }
        public string Imya { get; set; }
        public string Otchestvo { get; set; }
        public string Login { get; set; }
        public string Parol { get; set; }
        public int KodRoli { get; set; }
        public string Rol { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }

        public User()
        {
            Familiya = string.Empty;
            Imya = string.Empty;
            Otchestvo = string.Empty;
            Login = string.Empty;
            Parol = string.Empty;
            Rol = string.Empty;
        }

        public string Fio
        {
            get { return (Familiya + " " + Imya + " " + Otchestvo).Trim(); }
        }
    }
}