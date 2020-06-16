using Koshelek.TestTask.Clients.Base;
using Koshelek.TestTask.Domain.Model;
using Koshelek.TestTask.Interfaces.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Koshelek.TestTask.Clients.Message
{
    public class MessageClient : BaseClient, IMessageData
    {
        public MessageClient(IConfiguration config) : base(config, "api/Message") { }

        public List<Domain.Entities.Message> GetMessagesByDate(TimePeriod TimePeriod)
        {

            var responce = Post($"{_ServiceAddress}/GetByPeriod", TimePeriod);
            return responce.Content.ReadAsAsync<List<Domain.Entities.Message>>().Result;
        }

        public void PostMessage(Domain.Entities.Message message) => Post($"{_ServiceAddress}", message);
    }
}
