using Koshelek.TestTask.DAL.DataBase;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Interfaces.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Koshelek.TestTask.Interfaces.Services
{
    public class PostgreSqlMessageData : IMessageData
    {
        private readonly PostgreSqlDbContext _db;
        private readonly ILogger<PostgreSqlMessageData> _logger;

        public PostgreSqlMessageData(string ConnectionString, ILogger<PostgreSqlMessageData> logger, ILogger<PostgreSqlDbContext> loggerDb)
        {
            _db = new PostgreSqlDbContext(ConnectionString, loggerDb);
            _logger = logger;
        }

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End)
        {
            return _db.GetMessagesByTimeStamp(Start, End);
        }

        public void PostMessage(Message message)
        {
            _db.AddOrUpdateMessage(message);
        }
    }
}
