using Koshelek.TestTask.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace ConnectionSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestConnection();
            TestConnection2();
            TestConnection3();
            //DBHelper.Connection();
        }

        public static void TestConnection3()
        {
            var cs = "Host=localhost;Username=asp;Password=asp;Database=aspdb;Port=55432";
            string _CreateTable = "CREATE TABLE IF NOT EXISTS messages(message_id integer PRIMARY KEY, text VARCHAR(128) NOT NULL, datetime TIMESTAMP NOT NULL);";

            string AddMessage = "INSERT INTO messages VALUES(3, 'TextMessage', '2020-06-22 19:12:25')";
            string RowExist = "SELECT EXISTS(SELECT 1 FROM messages WHERE message_id=3)";
            string GetMessagesByTimeStamp =
                "SELECT message_id, text, datetime FROM messages WHERE datetime BETWEEN '2020-06-02 23:55:00'::timestamp AND '2020-06-22 19:12:25'::timestamp;";
            //"SELECT message_id, text, datetime FROM messages WHERE datetime BETWEEN '2020-06-02 23:55:00'::timestamp AND now()::timestamp;";
            //   WHERE  xtime BETWEEN now()::timestamp - (interval '1s') * $selectedtimeParm
            //   AND now()::timestamp;


            using (NpgsqlConnection connection = new NpgsqlConnection(cs))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //command.CommandText = TableExist;
                    command.CommandText = _CreateTable;
                    var tmp = command.ExecuteScalar()?.ToString();
                    
                    Console.WriteLine($"PostgreSQL answer: {tmp}");
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = RowExist;
                    var tmp = bool.Parse(command.ExecuteScalar()?.ToString());
                    if (!tmp)
                    {
                        using (var addCommand = connection.CreateCommand())
                        {
                            addCommand.CommandText = AddMessage;
                            addCommand.ExecuteScalar();
                        }
                    }

                }
                List<Message> messages = new List<Message>();
                using (var command = connection.CreateCommand())
                {

                    Console.WriteLine($"\nINFO: Start reading\n");
                    string val1, val2, val3;

                    command.CommandText = "SELECT message_id, text, datetime FROM messages";
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        val1 = reader[0].ToString();
                        val2 = reader[1].ToString();
                        val3 = reader[2].ToString();

                        messages.Add(new Message
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Text = reader[1].ToString(),
                            ServerDateTime = DateTime.Parse(reader[2].ToString())
                        });
                        Console.WriteLine($"PostgreSQL answer: {val1} {val2} {val3}");
                    }
                    reader.Close();
                    Console.WriteLine($"\nINFO: End reading\n");
                }

                Console.WriteLine($"\nINFO: Start List\n");
                foreach (var message in messages)
                {
                    Console.WriteLine($"{message.ServerDateTime}| {message.Id}: {message.Text}");
                }
                Console.WriteLine($"\nINFO: End List\n");

                using (var command = connection.CreateCommand())
                {

                    Console.WriteLine($"\nINFO: Start reading\n");
                    string val1, val2, val3;

                    command.CommandText = GetMessagesByTimeStamp;
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        val1 = reader[0].ToString();
                        val2 = reader[1].ToString();
                        val3 = reader[2].ToString();
                        Console.WriteLine($"PostgreSQL answer: {val1} {val2} {val3}");
                    }
                    reader.Close();
                    Console.WriteLine($"\nINFO: End reading\n");
                }
            }
        }

        public static void TestConnection2()
        {
            var cs = "Host=localhost;Username=asp;Password=asp;Database=aspdb;Port=55432";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
        }

        public static void TestConnection()
        {
            var cs = "Host=localhost;Username=asp;Password=asp;Database=aspdb;Port=55432";
            using (NpgsqlConnection connection = new NpgsqlConnection(cs))
            {
                connection.Open();
                Console.WriteLine("ConnectionOpen!");
            }
            Console.WriteLine("ConnectionClose!");
        }

    }
}
