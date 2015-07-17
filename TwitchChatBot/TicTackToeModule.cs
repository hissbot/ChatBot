using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class TicTackToeModule : ChatModule
    {

        public TicTackToeModule()
        {
            initCommands = new string[] { "!tic" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int i=0; i <40; i++)
            {
                sb.Append("#");
            }

            ChatBot.say(sb.ToString());

        }

    }
}
