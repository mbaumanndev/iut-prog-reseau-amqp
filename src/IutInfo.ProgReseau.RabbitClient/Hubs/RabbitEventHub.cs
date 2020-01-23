using System.Threading.Tasks;
using IutInfo.ProgReseau.BuildBlocks.RabbitMQ;
using Microsoft.AspNetCore.SignalR;

namespace IutInfo.ProgReseau.RabbitClient.Hubs
{
    public sealed class RabbitEventHub : Hub
    {
        private readonly IRabbitManager m_Manager;

        public RabbitEventHub(IRabbitManager p_Manager) {
            m_Manager = p_Manager;
        }

        public async Task SendMessage(string p_Message) {
            m_Manager.Publish(p_Message, "server.exchange", "topic", "server.queue.*");
        }

        public async Task RabbitCallback(string p_Message) {
            await Clients.Others.SendAsync("Rabbit", p_Message);
        }
    }
}