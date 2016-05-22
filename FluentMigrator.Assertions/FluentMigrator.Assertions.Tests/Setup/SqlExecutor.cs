using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace FluentMigrator.Assertions.Tests.Setup
{
    public static class SqlExecutor
    {
        private const string DatabaseName = "[FluentMigrator.Assertions]";
        private const string BackupFilename = "FluentMigratorAssertions.bak";

        public static void RestoreDatabase()
        {
            var backupLocation = GetBackupLocation();
            var sql = $"ALTER DATABASE {DatabaseName} SET OFFLINE WITH ROLLBACK IMMEDIATE;" +
                      $"RESTORE DATABASE {DatabaseName} FROM DISK ='{backupLocation}' WITH REPLACE;" +
                      $"ALTER DATABASE {DatabaseName} SET ONLINE";

            using (var sqlConnection = CreateConnection(ConnectionStrings.MasterDbConnectionString))
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
            var backupPath = Path.Combine(directoryPath, "Setup", BackupFilename);
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

        private static SqlConnection CreateConnection(string connectionString = ConnectionStrings.FluentMigratorDbConnectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
