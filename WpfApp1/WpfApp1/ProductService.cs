using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace WpfApp1
{
    public static class ProductService
    {
        private const string BaseQuery = @"
            SELECT t.kod_tovara, t.naimenovanie,
                   ISNULL(t.ispolnitel, '') AS ispolnitel,
                   ISNULL(k.nazvanie, '')   AS kategoriya,
                   ISNULL(t.zhanr, '')      AS zhanr,
                   t.god_vypuska, t.tsena_roznitsa, t.tsena_opt, t.ostatki
            FROM myz_tovary t
            LEFT JOIN myz_katеgoriya k ON t.kod_kategorii = k.kod_kategorii";

        public static ObservableCollection<Product> GetAll()
        {
            return Load(BaseQuery + " ORDER BY t.naimenovanie", null);
        }

        public static ObservableCollection<Product> Search(string searchText)
        {
            const string where = @"
                WHERE t.naimenovanie LIKE @search OR t.ispolnitel LIKE @search
                ORDER BY t.naimenovanie";

            return Load(BaseQuery + where, delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
            });
        }

        private static ObservableCollection<Product> Load(string query, Action<SqlCommand> configure)
        {
            var list = new ObservableCollection<Product>();

            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (configure != null)
                            configure(cmd);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var p = new Product();
                                p.KodTovara = reader.GetInt32(0);
                                p.Naimenovanie = reader.GetString(1);
                                p.Ispolnitel = reader.GetString(2);
                                p.Kategoriya = reader.GetString(3);
                                p.Zhanr = reader.GetString(4);
                                p.GodVypuska = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                                p.TsenaRoznitsa = reader.GetDecimal(6);
                                p.TsenaOpt = reader.IsDBNull(7) ? 0m : reader.GetDecimal(7);
                                p.Ostatki = reader.GetInt32(8);
                                list.Add(p);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка загрузки товаров: " + ex.Message, ex);
            }

            return list;
        }
    }
}