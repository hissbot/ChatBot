using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class TimeTools :ChatModule 
    {
        public TimeTools()
        {
            initCommands = new string[] { "!time" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
            ChatBot.say(DateTime.Now.ToLongTimeString());
        }
    }
}
