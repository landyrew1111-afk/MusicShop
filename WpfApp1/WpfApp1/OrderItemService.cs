using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class OrderItemService
    {
        public static ObservableCollection<OrderItem> GetByOrder(int kodZakaza)
        {
            string query =
                "SELECT t.naimenovanie, s.kolichestvo, s.tsena, " +
                "       (s.kolichestvo * s.tsena) AS summa " +
                "FROM myz_sostav_zakaza s " +
                "INNER JOIN myz_tovary t ON s.kod_tovara = t.kod_tovara " +
                "WHERE s.kod_zakaza = @kod";

            var list = new ObservableCollection<OrderItem>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kod", kodZakaza);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var it = new OrderItem();
                            it.Naimenovanie = reader.GetString(0);
                            it.Kolichestvo = reader.GetInt32(1);
                            it.Tsena = reader.GetDecimal(2);
                            it.Summa = reader.GetDecimal(3);
                            list.Add(it);
                        }
                    }
                }
            }

            return list;
        }
    }
}