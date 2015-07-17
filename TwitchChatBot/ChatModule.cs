using System;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class ChatModule
    {
        public string name;
        public string[] initCommands;

        public virtual void ProcessMessage(IrcEventArgs e)
        {
            
        }
    }
}