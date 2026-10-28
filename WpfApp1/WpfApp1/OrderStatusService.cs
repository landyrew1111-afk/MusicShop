using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class OrderStatusService
    {
        public static ObservableCollection<string> GetAllStatuses()
        {
            var list = new ObservableCollection<string>();
            string query = "SELECT nazvanie FROM myz_status_zakaza ORDER BY kod_statusa";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(reader.GetString(0));
                }
            }

            return list;
        }

        public static string GetStatusName(int kodZakaza)
        {
            string query =
                "SELECT s.nazvanie FROM myz_zakazy z " +
                "INNER JOIN myz_status_zakaza s ON z.kod_statusa = s.kod_statusa " +
                "WHERE z.kod_zakaza = @kod";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kod", kodZakaza);
                    object r = cmd.ExecuteScalar();
                    return r == null ? "" : r.ToString();
                }
            }
        }

        public static void ChangeStatus(int kodZakaza, string statusName, int kodSotrudnika)
        {
            string query =
                "UPDATE myz_zakazy SET " +
                "  kod_statusa = (SELECT kod_statusa FROM myz_status_zakaza WHERE nazvanie = @st), " +
                "  kod_sotrudnika = @ks " +
                "WHERE kod_zakaza = @kz";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@st", statusName);
                    cmd.Parameters.AddWithValue("@ks", kodSotrudnika);
                    cmd.Parameters.AddWithValue("@kz", kodZakaza);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}