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
    /// <summary>
    /// Message api controller
    /// </summary>
    public class MessageApiController : Controller
    {
        /// <summary>Messages data provider</summary>
        private readonly IMessageData _MessageData;
        private readonly IHubContext<MessagesHub> _HubContext;
        private readonly ILogger<MessageApiController> _logger;

        /// <summary>Messaage controller constroctor</summary>
        /// <param name="MessageData">Messages data provider</param>
        public MessageApiController(IMessageData MessageData, IHubContext<MessagesHub> HubContext, ILogger<MessageApiController> logger)
        {
            _MessageData = MessageData;
            _HubContext = HubContext;
            _logger = logger;
        }

        /// <summary>Post message to recipient (second) clients and database</summary>
        /// <param name="connectionId">Client connection id</param>
        /// <param name="message">Client message</param>
        [HttpPost, ActionName("Post")]
        public async Task Post(string connectionId, Message message)
        {
            message.ServerDateTime = DateTime.Now;
            _logger.LogDebug($"{message.ServerDateTime}| Receive message {message.Text}, Order: {message.Order}");

            // send message to recipients
            await _HubContext.Clients.AllExcept(connectionId).SendAsync("ReceiveMessage", new
            {
                message.Text,
                message.Id,
                message.Order,
                ServerDateTime = message.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            // send message to database
            _MessageData.PostMessage(message);
        }

        /// <summary>
        /// Send message to database
        /// </summary>
        /// <param name="message">Message</param>
        public void PostMessage(Message message)
        {
            _MessageData.PostMessage(message);
        }

        /// <summary>Get messages from database for last 1 minute</summary>
        /// <param name="connectionId">Client connection id</param>
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

        /// <summary>Get messages from database for last 10 minute</summary>
        /// <param name="connectionId">Client connection id</param>
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
        /// <summary></summary>
        /// <param name="connectionId">Client connection id</param>
        /// <summary>
        /// Get messages from database in period
        /// </summary>
        /// <param name="Start">Begin of period</param>
        /// <param name="End">End of peri</param>
        /// <returns></returns>
        public List<Message> GetMessagesByDate(DateTime Start, DateTime End = default(DateTime))
        {
            return _MessageData.GetMessagesByDate(Start, End);
        }
    }
}
