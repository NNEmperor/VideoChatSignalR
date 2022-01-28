using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, int> _connections;
        private static int count = 0;

        public ChatHub(IDictionary<string, int> connections)
        {
            _connections = connections;
        }

        public async Task JoinRoom(/*UserConnection userConnection*/)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, /*userConnection.Room */ "test");
            _connections[Context.ConnectionId] = ++count;
        }

        public async Task StartChat(StartingChatData data)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out int id))
            {
                List<string> me = new List<string>() { Context.ConnectionId };
                StartingChatResponseData response = new StartingChatResponseData() { From = Context.ConnectionId, Name = data.Name, Signal = data.Signal };
                await Clients.GroupExcept(/*userConnection.Room*/"test", me).SendAsync("calluser", response);//for now whole group
            }
        }

        public async Task AnswerCall(AnswerCallData data)
        {
            //if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            //{
            string temp = Context.ConnectionId;
                await Clients.All/*Group(userConnection.Room)*/.SendAsync("callaccepted", data.Signal);//for now whole group
            //}
        }

        public async Task Disconnect()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out int id))
            {
                await Clients.Group(/*userConnection.Room*/"test").SendAsync("callended");//for now whole group ends if one hangs up
            }
        }

    }
}
