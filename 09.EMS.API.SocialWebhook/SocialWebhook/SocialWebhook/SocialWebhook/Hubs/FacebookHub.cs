using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialWebhook.Hubs
{
    [HubName("FacebookHub")]
    public class FacebookHub: Hub
    {
        public static IHubContext FacebookContext = GlobalHost.ConnectionManager.GetHubContext<FacebookHub>();
        public static void PushMessageToAllUsers(string data)
        {
            FacebookContext.Clients.All.SendMessage(data);
        }

        #region User
        public static void UserSendMessageToPage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.entry[0].messaging[0].recipient.id.ToString());
            if (ConnectionID.Any())
            {
                //FacebookContext.Clients.Client(ConnectionID.First()).UserSendMessage(data);
                foreach (var item in ConnectionID)
                {
                    FacebookContext.Clients.Client(item).UserSendMessageToPage(data);
                }
            }
        }
        public static void UserReceivedMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.entry[0].messaging[0].sender.id.ToString());
            if (ConnectionID.Any())
            {
                //FacebookContext.Clients.Client(ConnectionID.First()).UserReceivedMessage();
                foreach (var item in ConnectionID)
                {
                    FacebookContext.Clients.Client(item).UserReceivedMessage(data);
                }
            }
        }
        public static void UserSeenMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.entry[0].messaging[0].sender.id.ToString());
            if (ConnectionID.Any())
            {
                //FacebookContext.Clients.Client(ConnectionID.First()).UserSeenMessage();
                foreach (var item in ConnectionID)
                {
                    FacebookContext.Clients.Client(item).UserSeenMessage(data);
                }
            }
        }
        #endregion

        #region Page
        public static void PageSendMessageToUser(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.entry[0].messaging[0].sender.id.ToString());
            if (ConnectionID.Any())
            {
                //FacebookContext.Clients.Client(ConnectionID.First()).PageSendMessageToUser(data);
                foreach (var item in ConnectionID)
                {
                    FacebookContext.Clients.Client(item).PageSendMessageToUser(data);
                }
            }
        }
        #endregion

        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public override Task OnConnected()
        {
            var name = Context.QueryString["name"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.QueryString["name"];

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.QueryString["name"];

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}