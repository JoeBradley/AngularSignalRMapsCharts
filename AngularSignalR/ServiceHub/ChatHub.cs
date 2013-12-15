using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularSignalR.ServiceHub
{
    // see: http://blogs.msdn.com/b/webdev/archive/2013/05/21/datatable-using-signalr-angularjs-entityframework.aspx
    // See: http://www.tugberkugurlu.com/archive/mapping-asp-net-signalr-connections-to-real-application-users
    // see: http://www.asp.net/signalr/overview/signalr-20/hubs-api/mapping-users-to-connections
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            ChatMessages.Messages.Add(new ChatMessage() { Name = name, Message = message });

            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
        public void Join(string name)
        {
            ChatMembers.Members.Add(new ChatMember() { ConnectionId = Context.ConnectionId, Name = name });

            foreach (ChatMessage msg in ChatMessages.Messages)
            {                
                Clients.Caller.broadcastMessage(msg.Name, msg.Message);            
            }

            String message = "has joined the conversation";
            ChatMessages.Messages.Add(new ChatMessage() { Name = name, Message = message });
            
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public override Task OnDisconnected()
        {
            foreach (ChatMember m in ChatMembers.Members)
            {
                if (Context.ConnectionId == m.ConnectionId)
                { 
                    String message = "has left the conversation";
                    ChatMessages.Messages.Add(new ChatMessage() { Name = m.Name, Message = message });
                    Send(m.Name, message);
                }
            }
            return base.OnDisconnected();
        }
    }

    public class ChatMembers
    {
        public static List<ChatMember> Members = new List<ChatMember>();
    }
    public class ChatMember
    {
        public String ConnectionId;
        public String Name;
    }
    
    public class ChatMessages
    {
        public static List<ChatMessage> Messages = new List<ChatMessage>();
    }
    public class ChatMessage
    {
        public String Name;
        public String Message;
    }
}