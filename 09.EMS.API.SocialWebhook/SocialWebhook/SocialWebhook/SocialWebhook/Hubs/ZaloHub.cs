using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SocialWebhook.Models;
using Newtonsoft.Json;
namespace SocialWebhook.Hubs
{
    [HubName("ZaloHub")]
    public class ZaloHub : Hub
    {
        public static IHubContext ZaloContext = GlobalHost.ConnectionManager.GetHubContext<ZaloHub>();
        //public static List<InfoHubConnection> uList = new List<InfoHubConnection>();
        //public static void PushMessageToAllUsers(string data)
        //{
        //    ZaloContext.Clients.All.SendMessage(data);

        //    //var ConnectionID = _connections.GetConnections("Neko");
        //    //if (ConnectionID.Any())
        //    //{
        //    //    ZaloContext.Clients.Client(ConnectionID.First()).SendMessage(data);
        //    //} 

        //    //ZaloHubContext.Clients.All.SendAsync("ReceiveMessage", data);

        //    //var user = uList.Where(o => o.UserName == userName);
        //    //if (user.Any())
        //    //{
        //    //    ZaloContext.Clients.Client(user.First().ConnectionID).sendMessage(sendFromId, userId, sendFromName, userName, message);
        //    //}
        //}

        #region User
        public static void UserSendMessageToPage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.recipient.id.ToString());
            if (ConnectionID.Any())
            {
                //ZaloContext.Clients.Client(ConnectionID.First()).SendAsync("UserSendMessageToPage", data);
                //ZaloContext.Clients.Client(ConnectionID.First()).UserSendMessageToPage();
                foreach (var item in ConnectionID)
                {
                    ZaloContext.Clients.Client(item).UserSendMessageToPage(data);
                }
            }
        }
        public static void UserReceivedMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.sender.id.ToString());
            if (ConnectionID.Any())
            {
                //ZaloContext.Clients.Client(ConnectionID.First()).SendAsync("UserSendMessageToPage", data);
                //ZaloContext.Clients.Client(ConnectionID.First()).UserReceivedMessage();
                foreach (var item in ConnectionID)
                {
                    ZaloContext.Clients.Client(item).UserReceivedMessage(data);
                }
            }
        }

        public static void UserSeenMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.sender.id.ToString());
            if (ConnectionID.Any())
            {
                //ZaloContext.Clients.Client(ConnectionID.First()).SendAsync("UserSendMessageToPage", data);
                //ZaloContext.Clients.Client(ConnectionID.First()).UserSeenMessage();
                foreach (var item in ConnectionID)
                {
                    ZaloContext.Clients.Client(item).UserSeenMessage(data);
                }
            }
        }

        #endregion

        #region Page
        public static void PageSendMessageToUser(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            IEnumerable<string> ConnectionID = _connections.GetConnections(req.sender.id.ToString());
            if (ConnectionID.Any())
            {
                //ZaloContext.Clients.Client(ConnectionID.First()).PageSendMessageToUser(data);
                foreach (var item in ConnectionID)
                {
                    ZaloContext.Clients.Client(item).PageSendMessageToUser(data);
                }
            }
        }
        #endregion

        //public override Task OnConnected()
        //{
        //    var us = new InfoHubConnection();
        //    us.UserName = Context.QueryString["username"];
        //    us.ConnectionID = Context.ConnectionId;
        //    uList.Add(us);
        //    return base.OnConnected();
        //}
        //public override Task OnConnected()
        //{
        //    string name = Context.User.Identity.Name;
        //    Groups.Add(Context.ConnectionId, name);

        //    return base.OnConnected();
        //}

        //public override Task OnConnected()
        //{
        //    ConnectedUserModel.ConnectionID.Add(Context.ConnectionId);
        //    return base.OnConnected();
        //}
        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    ConnectedUserModel.ConnectionID.Remove(Context.ConnectionId);
        //    return base.OnDisconnected(stopCalled);
        //}

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

        //public void SendChatMessage(string who, string message)
        //{
        //    string name = Context.User.Identity.Name;

        //    foreach (var connectionId in _connections.GetConnections(who))
        //    {
        //        Clients.Client(connectionId).addChatMessage(name + ": " + message);
        //    }
        //}
    }
}