using Npgsql;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConnectionSamples
{

    public static class DBHelper
    {
        private const string _CreateTable = "CREATE TABLE messages(message_id integer PRIMARY KEY, text VARCHAR(128) NOT NULL, datetime TIMESTAMP NOT NULL);";

        public static void Connection()
        {
            var cs = "Host=localhost;Username=asp;Password=asp;Database=aspdb;Port=55432";
            using (NpgsqlConnection connection = new NpgsqlConnection(cs))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = _CreateTable;
                    var tmp = command.ExecuteScalar();
                }
            }

        }

    }
}
