using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PhysicianPortal.Web
{
    public class MessageHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendMessage()
        {
            Clients.All.BroadcastMessage();
        }
    }
}