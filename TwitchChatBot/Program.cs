using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;


namespace TwitchChatBot
{



    class ChatBot
    {

        public static string channel = "#corra1310";

        public static IrcClient irc = new IrcClient();

        public static String[] wakeupResponses = {"(≖_≖ )","💪(◡̀_◡́҂)", "I wasn't sleeping", "Onii-chan!!", "You call?", "What's up?", "Somebody call for me??", "Hola", "Hey!", "✺◟(∗❛ัᴗ❛ั∗)◞✺" };

        public static List<ChatModule> runningModules = new List<ChatModule>();


        public static void ConnectTOIRC()
        {
            irc.Encoding = System.Text.Encoding.UTF8;
            irc.SendDelay = 200;
            irc.ActiveChannelSyncing = true;
            irc.OnRawMessage += new IrcEventHandler(onRawMessage);

            string[] serverlist;
            // the server we want to connect to, could be also a simple string
            serverlist = new string[] { "irc.twitch.tv" };
            int port = 6667;

            try
            {
                // here we try to connect to the server and exceptions get handled
                irc.Connect(serverlist, port);
            }
            catch (ConnectionException e)
            {
                // something went wrong, the reason will be shown
                System.Console.WriteLine("couldn't connect! Reason: " + e.Message);
            }

            try
            {
                // here we logon and register our nickname and so on 
                irc.Login("hissbot", "hissbot", 0, "hissbot", "oauth:qt8vz48gacp0fhh92jgwfweux66qbi");
                // join the channel
                irc.RfcJoin(channel);
                irc.Listen();
                irc.Disconnect();
            }
            catch (ConnectionException)
            {
                // this exception is handled because Disconnect() can throw a not
                // connected exception
                //Exit();
            }
            catch (Exception e)
            {
                // this should not happen by just in case we handle it nicely
                System.Console.WriteLine("Error occurred! Message: " + e.Message);
                System.Console.WriteLine("Exception: " + e.StackTrace);
                //Exit();
            }
        }
        public static void Main(string[] args)
        {
            initStuff();
            ConnectTOIRC();
        }
        private static void initStuff()
        {
            runningModules.Add(new EightBall());
            runningModules.Add(new ColorModule());
            runningModules.Add(new VotingSystem());
            runningModules.Add(new RollerModule());
            runningModules.Add(new TicTackToeModule());
            runningModules.Add(new Roulette());
            runningModules.Add(new DefaultChatCommands());
            
        }
        private static void onRawMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("Raw--" + e.Data.RawMessage);
            Console.WriteLine(String.Format("{0}: {1}", e.Data.Nick, e.Data.Message));
            if (e.Data.Nick != null && e.Data.Type != ReceiveType.Join)
            {
                  parseMessage(e);
            }
        }
        private static void parseMessage(IrcEventArgs e)
        {
            try
            {
                String[] messageArray = e.Data.MessageArray;

                if (messageArray.Length < 1)
                {
                    return;
                }

                if (messageArray[0].StartsWith("!"))
                {
                    parseMessageArray(e);
                }else if (Regex.IsMatch(e.Data.Message.ToLower(), "(hissbot|hiss)"))
                {
                    pickRandomResponse();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void pickRandomResponse()
        {

            Random rand = new Random();

            int value = rand.Next(0, wakeupResponses.Length);

            say(wakeupResponses[value]);
        }

        private static void parseMessageArray(IrcEventArgs e)
        {
            //switch (e.Data.MessageArray[0])
            //{

            //    case "!dance":
            //        irc.SendMessage(SendType.Message, channel, "(~‾▿‾)~");
            //        break;
            //    case "!color":
            //        getModuleByType(typeof(EightBall)).parseMessage(e);
            //        break;
            //    case "!dummy":
            //        irc.SendMessage(SendType.Message, channel, "are you asking about corra1310?");
            //        break;
            //    case "!showVote":
            //        showVoteResults();
            //        break;
            //    case "!clearVote":
            //        initStuff();
            //        break;
            //    case "!vote":
            //        //parseVote(e.Data.MessageArray);
            //        break;
            //    case "!lenny":
            //        say("┌༼▀̿̿Ĺ̯̿̿▀̿༽┘");
            //        break;
            //    case "!time":
            //        say(DateTime.Now.ToShortTimeString() + " " + TimeZone.CurrentTimeZone.StandardName);
            //        break;
            //    case "!roll":
            //        parseRoll(e.Data.MessageArray);
            //        break;
            //    case "!about":
            //        say("corra1310 is a guy who plays games and stuff");
            //        break;
            //    case "!store":
            //        parseStore(e.Data.MessageArray);
            //        break;
            //    case "!get":
            //        //parseGet(e.Data.MessageArray);
            //        break;
            //    case "!8ball":
            //        getModuleByType(typeof(EightBall)).parseMessage(e); 
            //        break;
            //    default:
            //        irc.SendMessage(SendType.Message, channel, "I dunno what to do.");
            //        break;
            //}

            List<ChatModule> moduleList = findModuleByInitStrings(e.Data.MessageArray[0]);

            if (moduleList.Count==0)
            {
                // say("Command not being picked up by any of the modules.");

                getModuleByType(typeof(DefaultChatCommands)).ProcessMessage(e);

            }
            else
            {
                moduleList.ForEach(m => m.ProcessMessage(e));
            }
        }
        

        private static List<ChatModule> findModuleByInitStrings(string commandString)
        {
            List<ChatModule> returnList = new List<ChatModule>();

            returnList= (from m in runningModules
                         where m.initCommands.Contains(commandString)
                         select m).ToList<ChatModule>();

            return returnList;
        }

     
       private static ChatModule getModuleByType(Type moduleType)
        {
            ChatModule module = (from m in runningModules
                                 where m.GetType() == moduleType
                                 select m).FirstOrDefault();
            return module;
        }
        public static void say(String inputString)
        {
            irc.SendMessage(SendType.Message, channel, inputString);
        }
    }
}


