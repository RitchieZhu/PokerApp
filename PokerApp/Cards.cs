using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerApp
{
    public class Card
    {

        public enum Ranks : byte
        {
            Two = 2, Three, Four, Five, Six,
            Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
        }

        static string[] allowedRanks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        public enum SuitType : int { Spade = 1, Heart, Diamond, Club }

        public Ranks cardRank;
        public SuitType cardSuit;
        public string error = "";

        public Card(string cardStr)
        {
            if (!string.IsNullOrEmpty(cardStr))
            {
                string suitStr = cardStr.Trim().Substring(cardStr.Length - 1, 1).ToLower();
                string rankStr = (cardStr.Trim().Substring(0, cardStr.Length - 1)).Trim().ToLower();

                switch (suitStr)
                {
                    //Helps to interprete the suit type from the input
                    case "s":
                        cardSuit = SuitType.Spade;
                        break;
                    case "h":
                        cardSuit = SuitType.Heart;
                        break;
                    case "d":
                        cardSuit = SuitType.Diamond;
                        break;
                    case "c":
                        cardSuit = SuitType.Club;
                        break;
                    default:
                        error = "Invalid Card. Can't read suit.";
                        break;

                }

                if (Array.Exists(allowedRanks, r => r.ToLower() == rankStr))
                {
                    switch (rankStr)
                    {
                            //Helps to interprete the face cards
                        case "j":
                            cardRank = Ranks.Jack;
                            break;
                        case "q":
                            cardRank = Ranks.Queen;
                            break;
                        case "k":
                            cardRank = Ranks.King;
                            break;
                        case "a":
                            cardRank = Ranks.Ace;
                            break;
                        default:
                            cardRank = (Ranks)Enum.Parse(typeof(Ranks), rankStr);
                            break;

                    }

                }
                else
                {
                    error = "Invalid Card. Can't read its rank.";
                }

            }
            else
            {
                this.error = "Can't read card. It's blank.";
            }

        }



    }
}
