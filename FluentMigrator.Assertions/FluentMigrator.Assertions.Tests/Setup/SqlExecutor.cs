using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace FluentMigrator.Helpers.Tests.Setup
{
    public static class SqlExecutor
    {
        public static void RestoreDatabase()
        {
            var backupLocation = GetBackupLocation();
            var sql = "ALTER DATABASE [FluentMigrator.Helpers] SET OFFLINE WITH ROLLBACK IMMEDIATE;" +
                     $"RESTORE DATABASE [FluentMigrator.Helpers] FROM DISK ='{backupLocation}' WITH REPLACE;" +
                      "ALTER DATABASE [FluentMigrator.Helpers] SET ONLINE";

            using (var sqlConnection = CreateConnection(Constants.MasterDbConnectionString))
            using (var sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        private static string GetBackupLocation()
        {
            var dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring("File:///".Length);
            // ReSharper disable once PossibleNullReferenceException
            var directoryPath = new FileInfo(dllPath).Directory.FullName;
            var backupPath = Path.Combine(directoryPath, "Setup", "FluentMigratorHelpers.bak");
            return backupPath;
        }

        public static void ExecuteNonQuery(string sql)
        {
            using (var sqlConnection = CreateConnection())
            using (var sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public static T ExecuteScalar<T>(string sql)
        {
            using (var sqlConnection = CreateConnection())
            using (var sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlConnection.Open();
                var result = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                return (T) result;
            }
        }

        public static DataTable ExecuteQuery(string sql)
        {
            using (var sqlConnection = CreateConnection())
            using (var sqlCommand = new SqlCommand(sql, sqlConnection))
            using (var sqlDataAdpater = new SqlDataAdapter(sqlCommand))
            {
                var results = new DataTable();
                sqlConnection.Open();
                sqlDataAdpater.Fill(results);
                sqlConnection.Close();
                return results;
            }
        }

        public static bool TableExists(string table)
        {
            return 0 != ExecuteScalar<int>($"SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{table}'");
        }

        private static SqlConnection CreateConnection(string connectionString = Constants.FluentMigratorDbConnectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
