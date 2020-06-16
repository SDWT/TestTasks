using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koshelek.TestTask.Domain.Entities;
using Koshelek.TestTask.Domain.Model;
using Koshelek.TestTask.Interfaces.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Koshelek.TestTask.Api.Controllers
{
    /// <summary>Message api controller</summary>
    [Route("api/Message")]
    [ApiController]
    public class MessageApiController : ControllerBase, IMessageData
    {
        private readonly IMessageData _MessageData;
        private readonly ILogger<MessageApiController> _logger;

        /// <summary>Messaage controller constroctor</summary>
        /// <param name="MessageData">Messages data provider</param>
        /// <param name="logger">logger</param>
        public MessageApiController(IMessageData MessageData, ILogger<MessageApiController> logger)
        {
            _MessageData = MessageData;
            _logger = logger;
            _logger.LogInformation("Start Message");
        }


        /// <summary>
        /// Get messages from database in period
        /// </summary>
        /// <param name="TimePeriod">Period of time</param>
        /// <returns>List of messages with in period</returns>
        [HttpPost("GetByPeriod")]
        public List<Message> GetMessagesByDate(TimePeriod TimePeriod)
        {
            _logger.LogDebug($"Get messages by period: {TimePeriod.Start} || {TimePeriod.End}");
            var msgs = _MessageData.GetMessagesByDate(TimePeriod);
            _logger.LogDebug($"Count of messages by period {TimePeriod.Start} || {TimePeriod.End} is {msgs.Count}");
            return msgs;
        }

        /// <summary> Send message to database </summary>
        /// <param name="message">Message</param>
        [HttpPost]
        public void PostMessage(Message message)
        {
            _logger.LogDebug($"Save message {message.Text}");
            _MessageData.PostMessage(message);
            _logger.LogDebug($"Message saved");
        }
    }
}
