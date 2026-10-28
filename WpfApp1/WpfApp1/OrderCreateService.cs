using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class OrderCreateService
    {
        public static int CreateOrder(int kodPolzovatelya, int kodSposobaDostavki,
                                      ObservableCollection<CartItem> items)
        {
            if (items.Count == 0)
                throw new Exception("Корзина пуста.");

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        decimal summa = 0;
                        foreach (var it in items) summa += it.Summa;

                        string insertOrder =
                            "INSERT INTO myz_zakazy " +
                            "(kod_polzovatelya, data, kod_statusa, kod_sposoba_dostavki, kod_sotrudnika, summa) " +
                            "VALUES (@kp, GETDATE(), 1, @kd, NULL, @s); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int kodZakaza;
                        using (var cmd = new SqlCommand(insertOrder, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@kp", kodPolzovatelya);
                            cmd.Parameters.AddWithValue("@kd", kodSposobaDostavki);
                            cmd.Parameters.AddWithValue("@s", summa);
                            kodZakaza = (int)cmd.ExecuteScalar();
                        }

                        foreach (var it in items)
                        {
                            string insertItem =
                                "INSERT INTO myz_sostav_zakaza " +
                                "(kod_zakaza, kod_tovara, kolichestvo, tsena) " +
                                "VALUES (@kz, @kt, @kol, @ts);";

                            using (var cmd = new SqlCommand(insertItem, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@kz", kodZakaza);
                                cmd.Parameters.AddWithValue("@kt", it.KodTovara);
                                cmd.Parameters.AddWithValue("@kol", it.Kolichestvo);
                                cmd.Parameters.AddWithValue("@ts", it.Tsena);
                                cmd.ExecuteNonQuery();
                            }

                            string updateStock =
                                "UPDATE myz_tovary SET ostatki = ostatki - @kol " +
                                "WHERE kod_tovara = @kt AND ostatki >= @kol;";

                            using (var cmd = new SqlCommand(updateStock, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@kol", it.Kolichestvo);
                                cmd.Parameters.AddWithValue("@kt", it.KodTovara);
                                int rows = cmd.ExecuteNonQuery();
                                if (rows == 0)
                                    throw new Exception("Товар \"" + it.Naimenovanie +
                                                        "\" закончился на складе.");
                            }
                        }

                        tran.Commit();
                        return kodZakaza;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}