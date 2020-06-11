using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Interfaces.Interfaces;

namespace Koshelek.TestTask.Client.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessageController : Controller, IMessageData
    {
        /// <summary>Messages data provider</summary>
        private readonly IMessageData _MessageData;

        /// <summary>
        /// Messaage controller constroctor
        /// </summary>
        /// <param name="MessageData">Messages data provider</param>
        public MessageController(IMessageData MessageData) => _MessageData = MessageData;

        [HttpPost, ActionName("Post")]
        public Message PostMessage(int Id, string Text)
        {
            return new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now};
        }

        [HttpGet, ActionName("Get")]
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
