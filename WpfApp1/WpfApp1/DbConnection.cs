using System.Data.SqlClient;

namespace WpfApp1
{
    public static class DbConnection
    {
        public const string ConnectionString =
            "Data Source=stud-mssql.sttec.yar.ru,38325;" +
            "Initial Catalog=user111_db;" +
            "User ID=user111_db;Password=user111;" +
            "TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}