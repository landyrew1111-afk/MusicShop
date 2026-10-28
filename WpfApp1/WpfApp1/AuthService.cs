using System;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class AuthService
    {
        public static User Login(string login, string password)
        {
            string query =
                "SELECT p.kod_polzovatelya, p.familiya, p.imya, p.otchestvo, " +
                "       p.login, p.parol, p.kod_roli, r.nazvanie AS rol, " +
                "       p.telefon, p.email, p.adres " +
                "FROM myz_polzovatel p " +
                "INNER JOIN myz_rol r ON p.kod_roli = r.kod_roli " +
                "WHERE p.login = @login AND p.parol = @password";

            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var u = new User();
                                u.KodPolzovatelya = reader.GetInt32(0);
                                u.Familiya = reader.GetString(1);
                                u.Imya = reader.GetString(2);
                                u.Otchestvo = reader.GetString(3);
                                u.Login = reader.GetString(4);
                                u.Parol = reader.GetString(5);
                                u.KodRoli = reader.GetInt32(6);
                                u.Rol = reader.GetString(7);
                                u.Telefon = reader.IsDBNull(8) ? "" : reader.GetString(8);
                                u.Email = reader.IsDBNull(9) ? "" : reader.GetString(9);
                                u.Adres = reader.IsDBNull(10) ? "" : reader.GetString(10);
                                return u;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка подключения к базе данных: " + ex.Message, ex);
            }

            return null;
        }

        public static bool LoginExists(string login)
        {
            string query = "SELECT COUNT(*) FROM myz_polzovatel WHERE login = @login";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public static void Register(string familiya, string imya, string otchestvo,
                                    string login, string parol, int kodRoli,
                                    string telefon, string email, string adres)
        {
            string query =
                "INSERT INTO myz_polzovatel " +
                "(familiya, imya, otchestvo, login, parol, kod_roli, telefon, email, adres) " +
                "VALUES (@f, @i, @o, @l, @p, @r, @t, @e, @a)";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@f", familiya);
                    cmd.Parameters.AddWithValue("@i", imya);
                    cmd.Parameters.AddWithValue("@o", otchestvo);
                    cmd.Parameters.AddWithValue("@l", login);
                    cmd.Parameters.AddWithValue("@p", parol);
                    cmd.Parameters.AddWithValue("@r", kodRoli);
                    cmd.Parameters.AddWithValue("@t", (object)telefon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@a", (object)adres ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}