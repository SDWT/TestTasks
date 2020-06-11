using Koshelek.TestTask.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Text;

namespace Koshelek.TestTask.DAL.DataBase
{
    public class PostgreSqlDbContext
    {
        private const string _cs = "Host=localhost;Username=asp;Password=asp;Database=aspdb;Port=55432";
        private const string _DbName = "messages";
        
        private const string _CreateTableString = "CREATE TABLE IF NOT EXISTS messages(message_id integer PRIMARY KEY, text VARCHAR(128) NOT NULL, datetime TIMESTAMP NOT NULL);";

        private const string AddMessageString = "INSERT INTO messages VALUES(3, 'TextMessage', '2020-06-22 19:12:25')";
        private const string RowExistString = "SELECT EXISTS(SELECT 1 FROM messages WHERE message_id=3)";
        private const string GetMessagesByTimeStampString =
            "SELECT message_id, text, datetime FROM messages WHERE datetime BETWEEN '2020-06-02 23:55:00'::timestamp AND '2020-06-22 19:12:25'::timestamp;";

        private readonly string _ConnectionString;
        public PostgreSqlDbContext(string ConnectionString = _cs)
        {
            _ConnectionString = ConnectionString;
        }

        public void AddMessage()
        {

        }

        public void UpdateMessage()
        {

        }

        public List<Message> GetMessagesByTimeStamp(DateTime Start, DateTime End)
        {
            var messages = new List<Message>();
            var strB = new StringBuilder();

            string[] attributes = { "message_id", "text", "datetime" };

            strB.Append("SELECT message_id, text, datetime FROM messages WHERE datetime BETWEEN ");
            strB.Append($"'{Start.ToString("yyyy-MM-dd HH:mm:ss")}'::timestamp ");
            strB.Append("AND ");
            strB.Append($"'{End.ToString("yyyy-MM-dd HH:mm:ss")}'::timestamp;");

            string GetMessagesBetweenTimeStampCommand = strB.ToString();


            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {

                    //log Console.WriteLine($"\nINFO: Start reading\n");

                    command.CommandText = GetMessagesBetweenTimeStampCommand;
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Text = reader[1].ToString(),
                            ServerDateTime = DateTime.Parse(reader[2].ToString())
                        });
                        //log Console.WriteLine($"");
                    }
                    reader.Close();
                    //log Console.WriteLine($"\nINFO: End reading\n");
                }

                connection.Close();
            }


            return messages;
        }
    }
}
