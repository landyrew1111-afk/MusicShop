using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class ProductEditService
    {
        public static ObservableCollection<string> GetCategories()
        {
            var list = new ObservableCollection<string>();
            string query = "SELECT nazvanie FROM myz_katеgoriya ORDER BY nazvanie";

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

        public static int GetCategoryId(string nazvanie)
        {
            string query = "SELECT kod_kategorii FROM myz_katеgoriya WHERE nazvanie = @n";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", nazvanie);
                    object result = cmd.ExecuteScalar();
                    return result == null ? 1 : (int)result;
                }
            }
        }

        public static void Insert(string naimenovanie, string ispolnitel, int kodKategorii,
                                  string zhanr, int god, decimal tsenaRozn, decimal tsenaOpt,
                                  int ostatok)
        {
            string query =
                "INSERT INTO myz_tovary " +
                "(naimenovanie, ispolnitel, kod_kategorii, zhanr, god_vypuska, " +
                " tsena_roznitsa, tsena_opt, ostatki) " +
                "VALUES (@n, @i, @k, @zh, @g, @tr, @to, @o)";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", naimenovanie);
                    cmd.Parameters.AddWithValue("@i", ispolnitel);
                    cmd.Parameters.AddWithValue("@k", kodKategorii);
                    cmd.Parameters.AddWithValue("@zh", zhanr);
                    cmd.Parameters.AddWithValue("@g", god);
                    cmd.Parameters.AddWithValue("@tr", tsenaRozn);
                    cmd.Parameters.AddWithValue("@to", tsenaOpt);
                    cmd.Parameters.AddWithValue("@o", ostatok);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Update(int kodTovara, string naimenovanie, string ispolnitel,
                                  int kodKategorii, string zhanr, int god,
                                  decimal tsenaRozn, decimal tsenaOpt, int ostatok)
        {
            string query =
                "UPDATE myz_tovary SET " +
                "  naimenovanie = @n, ispolnitel = @i, kod_kategorii = @k, " +
                "  zhanr = @zh, god_vypuska = @g, tsena_roznitsa = @tr, " +
                "  tsena_opt = @to, ostatki = @o " +
                "WHERE kod_tovara = @kod";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kod", kodTovara);
                    cmd.Parameters.AddWithValue("@n", naimenovanie);
                    cmd.Parameters.AddWithValue("@i", ispolnitel);
                    cmd.Parameters.AddWithValue("@k", kodKategorii);
                    cmd.Parameters.AddWithValue("@zh", zhanr);
                    cmd.Parameters.AddWithValue("@g", god);
                    cmd.Parameters.AddWithValue("@tr", tsenaRozn);
                    cmd.Parameters.AddWithValue("@to", tsenaOpt);
                    cmd.Parameters.AddWithValue("@o", ostatok);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int kodTovara)
        {
            string query = "DELETE FROM myz_tovary WHERE kod_tovara = @kod";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kod", kodTovara);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}