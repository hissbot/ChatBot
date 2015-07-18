using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchChatBot
{
    class Roulette : ChatModule
    {

        private Boolean gameOver = true;
        private int[] bulletArray;
        private int count = 0;

        private string[] bulletNumbers = new string[] { "first", "second", "third", "fourth", "fifth", "final" };

        string responseString = "";

        public Roulette()
        {
            initCommands = new String[] { "!roulette","!rouletteDebug" };
        }
        public override void ProcessMessage(IrcEventArgs e)
        {
           

            if (gameOver)
            {
                clearChamber();
                reload();

                gameOver = false;
            }

            responseString = String.Format("{0} spins the chamber and puts the gun to their head, holding their breath as they pull the trigger for the {1} shot...", e.Data.Nick, bulletNumbers[count]);

            pullTrigger(e.Data.Nick);



            ChatBot.say(responseString);

        }
        private void pullTrigger(string nickname)
        {
            if (bulletArray[count]==1)
            {
                responseString += " the gun fires, sending brain matter across the way";
                gameOver = true;
            }
            else
            {
                count++;
                responseString += "  *click* the gun doesn't go off, you are safe for now";
            }
        }
        private void clearChamber()
        {
            bulletArray = new int[6];
        }
        private void reload()
        {
            Random myRand = new Random();
            int bulletPlacement = myRand.Next(0, 6);

            bulletArray[bulletPlacement] = 1;

            count = 0;
        }

    }

}
