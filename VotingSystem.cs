using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class VotingSystem :ChatModule
    {
        public override void ProcessMessage(IrcEventArgs e)
        {
            ChatBot.say("Still working on inplementing this!");
        }

        public VotingSystem()
        {
            initCommands = new string[] { "!vote", "!voteNo", "!voteYes" };
        }
    }
}
