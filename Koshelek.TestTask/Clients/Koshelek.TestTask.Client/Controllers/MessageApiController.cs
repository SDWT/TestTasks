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
        public Message PostMessage(int Id, string Text)
        {
            return new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now};
        }
        [HttpGet]
        public void Get()
        {
            DateTime Start, End = DateTime.Now;
            Start = End - TimeSpan.FromSeconds(60);

            //await _HubContext.Clients.Client(connectionId).SendAsync("Receive", GetMessagesByDate(Start, End));
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
            //await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages);
            await _HubContext.Clients.All.SendAsync("Receive", messages2);
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
            //await _HubContext.Clients.Client(connectionId).SendAsync("Receive", messages);
            await _HubContext.Clients.All.SendAsync("Receive", messages2);
        }

        public List<Message> GetMessagesByDate(DateTime Start, DateTime End = default(DateTime))
        {
            //var messages = new List<Message>();
            //if (End == default(DateTime))
            //{
            //    End = DateTime.Now;
            //}

            //messages.Add(new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now });
            //messages.Add(new Message { Text = "WIP", Id = -2, ServerDateTime = DateTime.Now });
            //messages.Add(new Message { Text = "WIP", Id = -3, ServerDateTime = DateTime.Now });

            return _MessageData.GetMessagesByDate(Start, End);
        }

        public void PostMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task PostMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetMessagesByDateAsync(DateTime Start, DateTime End)
        {
            throw new NotImplementedException();
        }
    }
}
