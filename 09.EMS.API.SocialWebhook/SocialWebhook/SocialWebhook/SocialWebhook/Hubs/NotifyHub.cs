using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SocialWebhook.Models;

namespace SocialWebhook.Hubs
{
    [HubName("NotifyHub")]
    public class NotifyHub: Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public override Task OnConnected()
        {
            //string name = Context.User.Identity.Name;

            var name = Context.QueryString["name"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        public static IHubContext NotifyContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();

        public static void PushMessageToAllUsers(string data)
        {
            NotifyContext.Clients.All.SendMessage(data);

            //var ConnectionID = _connections.GetConnections("Neko");
            //if (ConnectionID.Any())
            //{
            //    ZaloContext.Clients.Client(ConnectionID.First()).SendMessage(data);
            //}

            //ZaloHubContext.Clients.All.SendAsync("ReceiveMessage", data);

            //var user = uList.Where(o => o.UserName == userName);
            //if (user.Any())
            //{
            //    ZaloContext.Clients.Client(user.First().ConnectionID).sendMessage(sendFromId, userId, sendFromName, userName, message);
            //}
        }
    }
}