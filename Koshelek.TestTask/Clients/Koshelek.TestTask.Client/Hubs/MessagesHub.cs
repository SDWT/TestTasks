using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Koshelek.TestTask.Client.Hubs
{
    public class MessagesHub : Hub
    {
        public async Task Send(string message, int id)
        {
            await this.Clients.All.SendAsync("Send", message, id);
        }
    }
}
