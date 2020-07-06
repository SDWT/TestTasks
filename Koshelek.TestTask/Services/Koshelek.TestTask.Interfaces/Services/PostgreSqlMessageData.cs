using Koshelek.TestTask.DAL.DataBase;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Domain.Model;
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

        public PostgreSqlMessageData(string ConnectionString, ILogger<PostgreSqlDbContext> loggerDb)
        {
            _db = new PostgreSqlDbContext(ConnectionString, loggerDb);
        }

        public List<Message> GetMessagesByDate(TimePeriod TimePeriod)
        {
            return _db.GetMessagesByTimeStamp(TimePeriod.Start, TimePeriod.End);
        }

        public void PostMessage(Message message)
        {
            _db.AddOrUpdateMessage(message);
        }
    }
}
