using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Interfaces.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Koshelek.TestTask.Client.Hubs;

namespace Koshelek.TestTask.Client.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MessageApiController : Controller, IMessageData
    {
        /// <summary>Messages data provider</summary>
        private readonly IMessageData _MessageData;
        private readonly IHubContext<MessagesHub> _HubContext;

        /// <summary>
        /// Messaage controller constroctor
        /// </summary>
        /// <param name="MessageData">Messages data provider</param>
        public MessageApiController(IMessageData MessageData, IHubContext<MessagesHub> HubContext)
        {
            _MessageData = MessageData;
            _HubContext = HubContext;
        }

        [HttpPost, ActionName("Post")]
        public async Task Post(string connectionId, Message message)
        {
            message.ServerDateTime = DateTime.Now;

            await _HubContext.Clients.AllExcept(connectionId).SendAsync("Send", message.Text, message.Id, message.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            await _HubContext.Clients.AllExcept(connectionId).SendAsync("ReceiveMessage", new
            {
                message.Text,
                message.Id,
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
            messages.Add(new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now });
            messages.Add(new Message { Text = "WIP", Id = -2, ServerDateTime = DateTime.Now });
            var messages2 = messages.Select(el => new
            {
                el.Text,
                el.Id,
                ServerDateTime = el.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToArray();
            await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages2);
        }

        public async Task GetLast10MinMessages(string connectionId)
        {
            DateTime Start, End = DateTime.Now;
            Start = End - TimeSpan.FromSeconds(600);

            var messages = GetMessagesByDate(Start, End);
            messages.Add(new Message { Text = "WIP", Id = -3, ServerDateTime = DateTime.Now });
            messages.Add(new Message { Text = "WIP", Id = -4, ServerDateTime = DateTime.Now });
            var messages2 = messages.Select(el => new
            {
                el.Text,
                el.Id,
                ServerDateTime = el.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToArray();
            await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages2);
        }

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End = default(DateTime))
        {
            return _MessageData.GetMessagesByDate(Start, End);
        }

        public Task PostMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetMessagesByDateAsync(DateTime Start, DateTime End)
        {
            throw new NotImplementedException();
        }

        void IMessageData.PostMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
