using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using System.Drawing;

namespace TwitchChatBot
{
    class ColorModule : ChatModule
    {
       
        public ColorModule()
        {
            name = "Color Module";
            initCommands = new string[] { "!color","!colour" };
        }

        public override void ProcessMessage(IrcEventArgs e)
        {
            parseColor(e.Data.MessageArray);
        }
        private void parseColor(string[] cmdArray)
        {
            if (cmdArray.Length < 2)
            {
             ChatBot.say("When doing !color use the format '!color red'");
            }
            else
            {
                Color returnColor = Color.FromName(cmdArray[1]);

                if (!returnColor.IsKnownColor)
                {
                    ChatBot.say(String.Format("{0} isnt a supported color... nerd", cmdArray[1]));
                }
                else
                {
                    ChatBot.say(String.Format("The hex for that color is #{0}{1}{2}", returnColor.R.ToString("X2"), returnColor.G.ToString("X2"), returnColor.B.ToString("X2")));
                }
            }

        }

    }
}
