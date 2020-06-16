using Koshelek.TestTask.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Koshelek.TestTask.DAL.DataBase
{
    public class PostgreSqlDbContext
    {
        private const string _CreateTableString =
            "CREATE TABLE IF NOT EXISTS messages(message_id serial PRIMARY KEY, text VARCHAR(128) NOT NULL, datetime TIMESTAMP NOT NULL, message_order integer NOT NULL);";

        private readonly string _ConnectionString;
        private readonly ILogger<PostgreSqlDbContext> _logger;

        /// <summary>
        /// Working with PostgreSQL constructor
        /// </summary>
        /// <param name="ConnectionString">Database connection string</param>
        public PostgreSqlDbContext(string ConnectionString, ILogger<PostgreSqlDbContext> logger)
        {
            _ConnectionString = ConnectionString;
            _logger = logger;

            #region Create Table If not exists

            CreateTable();
            #endregion

        }

        private void CreateTable()
        {
            int session = new Random().Next();
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                _logger.LogDebug($"Try to open db connection session: {session}");
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
                    throw;
                }
                using (var command = connection.CreateCommand())
                {
                    _logger.LogDebug("Try create table");
                    command.CommandText = _CreateTableString;
                    command.ExecuteScalar();
                    //Console.WriteLine($"PostgreSQL answer: {tmp}");
                }
                connection.Close();
                _logger.LogDebug($"End db connection session: {session}");
            }
        }

        /// <summary>
        /// Add message
        /// </summary>
        /// <param name="message">New message</param>
        public void AddMessage(Message message)
        {
            int session = new Random().Next();
            string InsertMessage =
               $"INSERT INTO messages (text, datetime, message_order) VALUES('{message.Text}', '{message.ServerDateTime:yyyy-MM-dd HH:mm:ss}', {message.Order})";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                _logger.LogDebug($"Try to open db connection session: {session}");
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    _logger.LogDebug($"Try add message {message.Text}");
                    command.CommandText = InsertMessage;
                    command.ExecuteScalar();

                }
                _logger.LogDebug($"Message {message.Text} added");

                connection.Close();
                _logger.LogDebug($"End db connection session: {session}");
            }
        }

        /// <summary>
        /// Add or Update message row
        /// </summary>
        /// <param name="message">New message</param>
        public void AddOrUpdateMessage(Message message)
        {
            int session = new Random().Next();
            string RowExist = $"SELECT EXISTS(SELECT 1 FROM messages WHERE message_id={message.Id})";
            string InsertMessage =
               $"INSERT INTO messages (text, datetime, message_order) VALUES('{message.Text}', '{message.ServerDateTime:yyyy-MM-dd HH:mm:ss}', {message.Order})";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                _logger.LogDebug($"Try to open db connection session: {session}");
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    _logger.LogDebug($"Try add or update message {message.Id}: {message.Text}");
                    command.CommandText = RowExist;
                    var tmp = bool.Parse(command.ExecuteScalar()?.ToString());
                    if (!tmp)
                    {
                        _logger.LogDebug($"Message with id {message.Id} not exists, try add message {message.Text}");
                        using (var addCommand = connection.CreateCommand())
                        {
                            addCommand.CommandText = InsertMessage;
                            addCommand.ExecuteScalar();
                        }
                    }
                    else
                    {
                        _logger.LogError($"Message with id {message.Id} exists, message not added");
                        //throw new ArgumentException($"Message with id {message.Id} exists");
                    }
                }
                connection.Close();
                _logger.LogDebug($"End db connection session: {session}");
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
            int session = new Random().Next();
            var messages = new List<Message>();
            var strB = new StringBuilder();

            strB.Append("SELECT * FROM messages WHERE datetime BETWEEN ");
            strB.Append($"'{Start:yyyy-MM-dd HH:mm:ss}'::timestamp ");
            strB.Append("AND ");
            strB.Append($"'{End:yyyy-MM-dd HH:mm:ss}'::timestamp;");

            string GetMessagesBetweenTimeStampCommand = strB.ToString();

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                _logger.LogDebug($"Try to open db connection session: {session}");
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    _logger.LogDebug($"Try get messages between {Start:yyyy-MM-dd HH:mm:ss} and {End:yyyy-MM-dd HH:mm:ss}");
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
                    _logger.LogDebug($"Messages count: {messages.Count}");
                    reader.Close();
                    //log Console.WriteLine($"\nINFO: End reading\n");
                }
                connection.Close();
                _logger.LogDebug($"End db connection session: {session}");
            }


            return messages;
        }
    }
}
