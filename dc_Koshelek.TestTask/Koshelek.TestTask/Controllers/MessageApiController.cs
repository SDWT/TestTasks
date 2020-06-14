using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Interfaces.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Koshelek.TestTask.Hubs;
using Microsoft.Extensions.Logging;

namespace Koshelek.TestTask.Controllers
{
    public class MessageApiController : Controller
    {
        /// <summary>Messages data provider</summary>
        private readonly IMessageData _MessageData;
        private readonly IHubContext<MessagesHub> _HubContext;
        private readonly ILogger<MessageApiController> _logger;

        /// <summary>
        /// Messaage controller constroctor
        /// </summary>
        /// <param name="MessageData">Messages data provider</param>
        public MessageApiController(IMessageData MessageData, IHubContext<MessagesHub> HubContext, ILogger<MessageApiController> logger)
        {
            _MessageData = MessageData;
            _HubContext = HubContext;
            _logger = logger;
        }

        [HttpPost, ActionName("Post")]
        public async Task Post(string connectionId, Message message)
        {
            message.ServerDateTime = DateTime.Now;
            _logger.LogDebug($"{message.ServerDateTime}| Receive message {message.Text}, Order: {message.Order}");

            await _HubContext.Clients.AllExcept(connectionId).SendAsync("ReceiveMessage", new
            {
                message.Text,
                message.Id,
                message.Order,
                ServerDateTime = message.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            _MessageData.PostMessage(message);
        }

        public void PostMessage(Message message)
        {
            _MessageData.PostMessage(message);
        }

        [HttpPost]
        public async Task GetLast1MinMessages(string connectionId)
        {
            DateTime Start, End = DateTime.Now;
            Start = End - TimeSpan.FromSeconds(60);

            var messages = GetMessagesByDate(Start, End);

            var messages2 = messages.Select(el => new
            {
                el.Text,
                el.Id,
                el.Order,
                ServerDateTime = el.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToArray();
            await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages2);
        }

        public async Task GetLast10MinMessages(string connectionId)
        {
            DateTime Start, End = DateTime.Now;
            Start = End - TimeSpan.FromSeconds(600);

            var messages = GetMessagesByDate(Start, End);

            var messages2 = messages.Select(el => new
            {
                el.Text,
                el.Id,
                el.Order,
                ServerDateTime = el.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToArray();
            await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages2);
        }

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End = default(DateTime))
        {
            return _MessageData.GetMessagesByDate(Start, End);
        }
    }
}
