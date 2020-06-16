﻿using Koshelek.TestTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Koshelek.TestTask.Interfaces.Interfaces
{
    public interface IMessageData
    {
        public void PostMessage(Message message);

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End);
    }
}
