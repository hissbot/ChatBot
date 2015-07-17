using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class RollerModule : ChatModule
    {
        public RollerModule()
        {
            initCommands = new string[] { "!roll", "!roller", "!rll", "!r" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
            parseRoll(e.Data.MessageArray);
        }
        private void parseRoll(String[] cmdArray)
        {

            Random myRand = new Random();
            int returnNumber;

            int lower = 1;
            int higher = 20;

            if (cmdArray.Length < 2)
            {
                returnNumber = myRand.Next(lower, higher + 1);
            }
            else
            {
                lower = Convert.ToInt32(cmdArray[1]);
                higher = Convert.ToInt32(cmdArray[2]);

                returnNumber = myRand.Next(lower, higher + 1);
            }
            ChatBot.say(String.Format("Roll from {0}-{1} resulted in: {2}", lower.ToString(), higher.ToString(), returnNumber.ToString()));

        }
    }
}
