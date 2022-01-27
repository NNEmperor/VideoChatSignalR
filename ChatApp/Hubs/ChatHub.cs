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
        private readonly string _botUser;

        public ChatHub()
        {
            _botUser = "MyChatBot";
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");
        }
        public async Task Disconnect()
        {
            await Clients.All.SendAsync("callended");
        }

        public async Task Calluser(string userToCall, string signalData, string from, string name)
        {
            await Clients.User(userToCall).SendAsync("calluser", signalData, from, name);
        }

        public async Task Answercall(string to, string signal)
        {
            await Clients.User(to).SendAsync("callaccepted", signal);
        }

    }
}
