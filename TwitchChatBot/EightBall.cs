using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{

    class EightBall : ChatModule
    {
        public static String[] RESPONSES = new String[] { "It is certain", "It is decidedly so", "Without a doubt", "Yes definitely", "You may rely on it", "As I see it, yes",
                        "Most likely", "Outlook good","Yes","Signs point to yes","Reply Hazy, try again","Ask again later","Better not tell you now", "Cannot predict now","Concetrate and ask again",
        "Don't count on it", "My reply is no","My sources say no","Outlook not so good", "Very doubtful", "No", "NO"};
        
        
        public EightBall()
        {
            initCommands = new string[] {"!8ball","!8bal" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
            shake8ball();
        }
        private void shake8ball()
        {
            Random myRand = new Random();

            int response = myRand.Next(0, RESPONSES.Length);
            ChatBot.say(RESPONSES[response]);
        }
       

    }
}
