using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using System.Diagnostics;

namespace TwitchChatBot
{
    class BlackjackModule : ChatModule
    {

        private List<PlayingCard> deck = new List<PlayingCard>();
        private List<PlayingCard> discarded = new List<PlayingCard>();
        private List<PlayingCard> playerHand = new List<PlayingCard>();
        private List<PlayingCard> dealerHand = new List<PlayingCard>();

        private string currentPlayer = String.Empty;

        private int CurrentState = States.NO_GAME;

        class States
        {
            public const int NO_GAME = 0;
            public const int CARDS_DEALT = 1;
            public const int WAITING_FOR_PLAYER = 2;
            public const int OTHER_STATE = 3;
        }

        public BlackjackModule()
        {
            initCommands = new string[] { "!blackjack", "!hit", "!stand", "!resetBlackjack","!jack" };
            GrabNewDeck();
            ShuffleDeck();
        }

        private void clearDeck()
        {
            deck = new List<PlayingCard>();
            discarded = new List<PlayingCard>();
            playerHand = new List<PlayingCard>();
            dealerHand = new List<PlayingCard>();


        }
        private void GrabNewDeck()
        {
            clearDeck();

            for (int i = 0; i < 52; i++)
            {
                PlayingCard temp = new PlayingCard();
                int remainder = i % 13;

                temp.Symbol = PlayingCard.SYMBOLS[remainder];
                temp.Suit = PlayingCard.SUITS[i / 13];
                if (remainder == 0)
                {
                    temp.Value = 11;
                }
                else if (remainder < 9)
                {
                    temp.Value = remainder + 1;
                }
                else
                {
                    temp.Value = 10;
                }

                deck.Add(temp);
            }
        }
        private void ShuffleDeck()
        {
            Random myRand = new Random();

            for (int i = deck.Count; i > 1; i--)
            {
                int pos = myRand.Next(i);

                PlayingCard temp = deck[i - 1];
                deck[i - 1] = deck[pos];
                deck[pos] = temp;
            }

        }
        public PlayingCard GrabCardFromTopOfDeck()
        {
            if (deck.Count == 0)
            {
                return new PlayingCard() { Suit = "None", Symbol = "None", Value = 0 };
            }

            PlayingCard temp = deck[0];
            deck.RemoveAt(0);
            return temp;
        }
        public void Deal()
        {
            playerHand.Add(GrabCardFromTopOfDeck());
            dealerHand.Add(GrabCardFromTopOfDeck());

            playerHand.Add(GrabCardFromTopOfDeck());
            dealerHand.Add(GrabCardFromTopOfDeck());
        }
        public void Reset()
        {
            GrabNewDeck();
            CurrentState = States.NO_GAME;
            ShuffleDeck();
        }

        public override void ProcessMessage(IrcEventArgs e)
        {
            if (e.Data.MessageArray[0] == "!resetBlackjack")
            {
                Reset();
                return;
            }


            if (CurrentState == States.NO_GAME && (e.Data.MessageArray[0]=="!jack" || e.Data.MessageArray[0] == "!blackjack"))
            {
                //start new game

                currentPlayer = e.Data.Nick;

                Deal();
                CurrentState = States.CARDS_DEALT;

                TellPlayerOfHands();

            }
            else if (CurrentState == States.CARDS_DEALT && e.Data.Nick == currentPlayer)
            {
                ParseCommand(e);
            }
        }
        private void ParseCommand(IrcEventArgs e)
        {
            string response = string.Empty;


            if (e.Data.MessageArray[0] == "!hit")
            {
                PlayingCard dealtCard = GrabCardFromTopOfDeck();
                //CheckPlayerBust();

                playerHand.Add(dealtCard);

                int playerScore = getScoreForHand(playerHand);

                if (playerScore > 21)
                {
                    response = string.Format("{0} dealt to {2}. ({1}). PLAYER BUSTS! with a score of {1}", dealtCard, playerScore,currentPlayer);
                    Reset();
                }
                else if (playerScore == 21)
                {
                    response = string.Format("{0} dealt to {1}. (21)! Kappa", dealtCard,currentPlayer);
                }
                else
                {
                    response = string.Format("{0} dealt to {2}. ({1})", dealtCard, playerScore,currentPlayer);
                }

                ChatBot.say(response);


            }
            else if (e.Data.MessageArray[0] == "!stand")
            {
                DealerTurn();
            }
        }
        private void DealerTurn()
        {
            int dealerScore = getScoreForHand(dealerHand);
            int playerScore = getScoreForHand(playerHand);
            StringBuilder response = new StringBuilder();

            response.Append(String.Format("My cards are ({0},{1}) = ({2})", dealerHand[0], dealerHand[1], dealerScore));

            
                while (dealerScore <= 16 && dealerScore < playerScore)
                {
                    PlayingCard card = GrabCardFromTopOfDeck();

                    dealerHand.Add(card);
                    dealerScore = getScoreForHand(dealerHand);

                    response.Append(String.Format("*-- Hit: {0} ({1}) ", card, dealerScore));

                }

                if (dealerScore>21)
                {
                    response.Append("I Bust :(");
                }
                else if (dealerScore==21)
                {
                    response.Append("BLACKJACK!");
                }
                else if (dealerScore>playerScore)
                {
                    response.Append(" I WIN! ");
                }
                else if (dealerScore==playerScore)
                {
                    response.Append(" PUSH! GG Kappa");
                }
                else
                {
                    response.Append(" I think i lost this one.");
                }


            response.Append(" ------  Type !jack or !blackjack to start a game!");

            Reset();
           
            ChatBot.say(response.ToString());

        }
        private int getScoreForHand(List<PlayingCard> hand)
        {
            int score = getScoreRaw(hand);

            while (score > 21 && hand.Any(c => c.Value == 11))
            {
                hand.First(c => c.Value == 11).Value = 1;
                score = getScoreRaw(hand);
            }

            return score;
        }
        private int getScoreRaw(List<PlayingCard> hand)
        {
            int score = 0;
            foreach (PlayingCard card in hand)
            {
                score += card.Value;
            }
            return score;
        }
        private bool CheckPlayerBust()
        {
            return false;
        }
        private void DealCardToPlayer()
        {
            playerHand.Add(GrabCardFromTopOfDeck());
        }
        private void TellPlayerOfHands()
        {
            string response = String.Format("Hissbot will be your dealer today! {3} recieved the cards {0} and {1} ({4}) Hissbot is showing {2} . !hit or !stand, ", playerHand[0].ToString(), playerHand[1].ToString(), dealerHand[0].ToString(), currentPlayer, getScoreForHand(playerHand));
            ChatBot.say(response);
        }
    }
    [DebuggerDisplay("{Symbol} of {Suit} ")]
    class PlayingCard
    {
        public static readonly string[] SYMBOLS = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        public static readonly string[] SUITS = { "♠", "♦", "♥", "♣" };

        public string Suit { get; set; }
        public string Symbol { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Symbol, Suit);
        }

    }

}
