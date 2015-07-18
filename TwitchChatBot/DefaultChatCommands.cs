using Meebey.SmartIrc4net;
using System.Collections.Generic;
using System.Linq;

namespace TwitchChatBot
{
    class DefaultChatCommands : ChatModule
    {

       public  Dictionary<string, string> chatStrings = new Dictionary<string, string>()
        {
            {"!help","Help would go here!" },
            {"!dummy","Corra1310" },
            {"!version","HissBot V0.2" },
            {"!lenny","(͡° ͜ʖ ͡°)" },
            {"!eat","(っ＾▿＾)۶🍸🌟🍺٩(˘◡˘ )" },
            {"!hi","(っ＾▿＾)💨" },
           {"!github","https://github.com/hissbot/ChatBot" },
           {"!shrug",@"¯\_| ಠ ∧ ಠ |_/¯" },
           {"!test",@"Testing" }
           
        };


        public DefaultChatCommands()
        {
            initCommands = new string[] { "!chat" ,"!c" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
            int commandCount = e.Data.MessageArray.Length;

            if (e.Data.Message.StartsWith("!") && commandCount>0)
            {
                parseCommand(e);
            }      
        }
        private void parseCommand(IrcEventArgs e)
        {
            string initCommand = e.Data.MessageArray[0];
           
            if (initCommands.Any(cmd => initCommand ==cmd))
            {
                ChatBot.say("Found chat command");
            }
            else
            {
                KeyValuePair<string,string> command = (from cs in chatStrings
                            where cs.Key == initCommand
                            select cs).FirstOrDefault();

                ChatBot.say(command.Value);

            }
        }
    }
}