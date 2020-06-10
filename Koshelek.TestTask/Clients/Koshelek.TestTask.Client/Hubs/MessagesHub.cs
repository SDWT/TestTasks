using Koshelek.TestTask.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Koshelek.TestTask.Client.Hubs
{
    public class MessagesHub : Hub
    {
        public async Task Send(string message, int id)
        {
            var msg = new Message { Id = id, Text = message, ServerDateTime = DateTime.Now };
            await this.Clients.All.SendAsync("Send", msg.Text, msg.Id, msg.ServerDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
