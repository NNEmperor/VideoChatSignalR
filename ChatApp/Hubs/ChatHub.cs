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
        private readonly IDictionary<string, UserConnection> _connections;

        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            _connections = connections;
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, /*userConnection.Room */ "test");
        }

        public async Task StartChat(StartingChatData data)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                StartingChatResponseData response = new StartingChatResponseData() { From = Context.ConnectionId, Name = data.Name, Signal = data.Signal };
                await Clients.Group(/*userConnection.Room*/"test").SendAsync("callusers", userConnection.User, response);//for now whole group
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
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(/*userConnection.Room*/"test").SendAsync("callended");//for now whole group ends if one hangs up
            }
        }

        public async Task Calluser(CallUserC callUserC)
        {
            string userToCall = callUserC.UserToCall.ToString();
            CallUserS userDataSendingBack = new CallUserS { From = callUserC.From, Name = callUserC.Name, Signal = callUserC.Signal };
            await Clients.All.SendAsync("calluser", userDataSendingBack);
        }

        //public async Task Answercall(AnswerCallC answerCallC)
        //{
        //    await Clients.All.SendAsync("callaccepted", answerCallC.Signal);
        //}

    }
}
