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
        private const string _CreateTableString =
            "CREATE TABLE IF NOT EXISTS messages(message_id serial PRIMARY KEY, text VARCHAR(128) NOT NULL, datetime TIMESTAMP NOT NULL, message_order integer NOT NULL);";

        private readonly string _ConnectionString;

        /// <summary>
        /// Working with PostgreSQL constructor
        /// </summary>
        /// <param name="ConnectionString">Database connection string</param>
        public PostgreSqlDbContext(string ConnectionString)
        {
            _ConnectionString = ConnectionString;

            #region Create Table If not exists
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = _CreateTableString;
                    command.ExecuteScalar();
                    //Console.WriteLine($"PostgreSQL answer: {tmp}");
                }
            }
            #endregion
        }

        public void AddMessage(Message message)
        {
            string InsertMessage =
               $"INSERT INTO messages (text, datetime, message_order) VALUES('{message.Text}', '{message.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', {message.Order})";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {

                    //log Console.WriteLine($"\nINFO: Start reading\n");

                    command.CommandText = InsertMessage;
                    command.ExecuteScalar();
                    
                }

                connection.Close();
            }
        }

        public void UpdateMessage(Message message)
        {
            string RowExist = $"SELECT EXISTS(SELECT 1 FROM messages WHERE message_id={message.Id})";
            string InsertMessage =
               $"INSERT INTO messages (text, datetime, message_order) VALUES('{message.Text}', '{message.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', {message.Order})";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = RowExist;
                    var tmp = bool.Parse(command.ExecuteScalar()?.ToString());
                    if (!tmp)
                    {
                        using (var addCommand = connection.CreateCommand())
                        {
                            addCommand.CommandText = InsertMessage;
                            addCommand.ExecuteScalar();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get messages if time period
        /// </summary>
        /// <param name="Start">Begin Start period</param>
        /// <param name="End">End Start Period</param>
        /// <returns></returns>
        public List<Message> GetMessagesByTimeStamp(DateTime Start, DateTime End)
        {
            var messages = new List<Message>();
            var strB = new StringBuilder();

            strB.Append("SELECT * FROM messages WHERE datetime BETWEEN ");
            strB.Append($"'{Start:yyyy-MM-dd HH:mm:ss}'::timestamp ");
            strB.Append("AND ");
            strB.Append($"'{End:yyyy-MM-dd HH:mm:ss}'::timestamp;");

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
                            ServerDateTime = DateTime.Parse(reader[2].ToString()),
                            Order = int.Parse(reader[3].ToString())

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
