﻿using Koshelek.TestTask.DAL.DataBase;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Koshelek.TestTask.Interfaces.Services
{
    public class PostgreSqlMessageData : IMessageData
    {
        private readonly PostgreSqlDbContext _db;

        public PostgreSqlMessageData(string ConnectionString)
        {
            _db = new PostgreSqlDbContext(ConnectionString);
        }

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End)
        {
            return _db.GetMessagesByTimeStamp(Start, End);
        }

        public void PostMessage(Message message)
        {
            _db.UpdateMessage(message);
        }
    }
}
