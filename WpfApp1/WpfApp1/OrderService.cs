using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class OrderService
    {
        private const string BaseQuery =
            "SELECT z.kod_zakaza, " +
            "       (p.familiya + ' ' + p.imya) AS klient, " +
            "       z.data, " +
            "       s.nazvanie AS status, " +
            "       d.nazvanie AS dostavka, " +
            "       ISNULL((so.familiya + ' ' + so.imya), '—') AS sotrudnik, " +
            "       z.summa " +
            "FROM myz_zakazy z " +
            "INNER JOIN myz_polzovatel p  ON z.kod_polzovatelya = p.kod_polzovatelya " +
            "INNER JOIN myz_status_zakaza s ON z.kod_statusa = s.kod_statusa " +
            "INNER JOIN myz_sposob_dostavki d ON z.kod_sposoba_dostavki = d.kod_sposoba_dostavki " +
            "LEFT JOIN myz_polzovatel so ON z.kod_sotrudnika = so.kod_polzovatelya";

        public static ObservableCollection<Order> GetAll()
        {
            return Load(BaseQuery + " ORDER BY z.data DESC", null);
        }

        public static ObservableCollection<Order> GetByUser(int kodPolzovatelya)
        {
            string where = " WHERE z.kod_polzovatelya = @kod ORDER BY z.data DESC";
            return Load(BaseQuery + where, delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@kod", kodPolzovatelya);
            });
        }

        public static ObservableCollection<Order> FilterByStatus(string statusName)
        {
            string where = " WHERE s.nazvanie = @st ORDER BY z.data DESC";
            return Load(BaseQuery + where, delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@st", statusName);
            });
        }

        private static ObservableCollection<Order> Load(string query, Action<SqlCommand> configure)
        {
            var list = new ObservableCollection<Order>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (configure != null) configure(cmd);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var o = new Order();
                            o.KodZakaza = reader.GetInt32(0);
                            o.Klient = reader.GetString(1);
                            o.Data = reader.GetDateTime(2);
                            o.Status = reader.GetString(3);
                            o.SposobDostavki = reader.GetString(4);
                            o.Sotrudnik = reader.GetString(5);
                            o.Summa = reader.GetDecimal(6);
                            list.Add(o);
                        }
                    }
                }
            }

            return list;
        }
    }
}