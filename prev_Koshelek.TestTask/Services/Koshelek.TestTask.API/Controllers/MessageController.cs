﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Koshelek.TestTask.Domain.Entities;

namespace Koshelek.TestTask.API.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessageController : Controller
    {
        [HttpPost]
        public Message Post(int Id, string Text)
        {
            return new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now};
        }

        [HttpPost("GetByDate")]
        public List<Message> GetByDate(DateTime Start, DateTime End = default(DateTime))
        {
            var messages = new List<Message>();
            if (End == default(DateTime))
            {
                End = DateTime.Now;
            }

            //messages.Add(new Message { Text = "WIP", Id = -1, ServerDateTime = DateTime.Now });
            //messages.Add(new Message { Text = "WIP", Id = -2, ServerDateTime = DateTime.Now });
            //messages.Add(new Message { Text = "WIP", Id = -3, ServerDateTime = DateTime.Now });

            return messages;
        }

    }
}
