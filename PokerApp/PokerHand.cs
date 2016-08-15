using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerApp
{
    public class PokerHand
    {

        public enum HandType : int
        {
            HighCard = 1, OnePair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush


        }

        private string playername = "";


        public string PlayerName
        {
            get { return playername; }

            set { playername = value; }
        }

        public List<Card> CardsInHand;

        public int HighestEvalRank;
        public int SecondaryEvalRank;
        public int Kicker1;
        public int Kicker2;
        public int Kicker3;
        public int Kicker4;
        public HandType HandEvalType;
        public int FlushSuit;

        public int FindHighestCard()
        {
            return (int)this.CardsInHand.OrderByDescending(c => c.cardRank).First().cardRank;
        }


        public HandType evalHand()
        {
            var rankGroupings = from card in this.CardsInHand
                                group card by card.cardRank into g
                                orderby g.Count() descending, g.Key descending
                                select g;

            int groupedCount1 = rankGroupings.First().Count();
            int groupedCount2 = rankGroupings.ElementAt(1).Count();
            int value1 = (int)rankGroupings.First().Key;
            int value2 = (int)rankGroupings.ElementAt(1).Key;

            SecondaryEvalRank = 0;
            Kicker1 = 0;
            Kicker2 = 0;
            Kicker3 = 0;
            Kicker4 = 0;


            switch (groupedCount1)
            {
                case 4:  //counts are 4 and 1, then the hand is quads.
                    {
                        this.HandEvalType = HandType.FourOfAKind;
                        this.HighestEvalRank = value1;
                        break;
                    }

                case 3:
                    {
                        if (groupedCount2 == 2)  //counts are 3 and 2, then the hand is full house.
                        {
                            this.HandEvalType = HandType.FullHouse;
                            this.HighestEvalRank = value1;
                        }
                        else  //counts are 3 and 1 and 1, then the hand is three of kind.
                        {
                            this.HandEvalType = HandType.ThreeOfAKind;
                            this.HighestEvalRank = value1;
                            this.Kicker1 = value2;
                            this.Kicker2 = (int)rankGroupings.ElementAt(2).Key;
                        }
                        break;
                    }
                case 2:
                    {
                        if (groupedCount2 == 2)  //counts are 2 and 2 and 1, then the hand is two pair.
                        {
                            this.HandEvalType = HandType.TwoPairs;
                            this.HighestEvalRank = value1;
                            this.SecondaryEvalRank = value2;
                            this.Kicker1 = (int)rankGroupings.ElementAt(2).Key;
                        }
                        else  //counts are 2 and 1 and 1 and 1, then the hand is a pair.
                        {
                            this.HandEvalType = HandType.OnePair;
                            this.HighestEvalRank = value1;
                            this.Kicker1 = (int)rankGroupings.ElementAt(1).Key;
                            this.Kicker2 = (int)rankGroupings.ElementAt(2).Key;
                            this.Kicker3 = (int)rankGroupings.ElementAt(3).Key;
                        }
                        break;
                    }
                default:
                    {

                        //All card have different rank, check flush and straight
                        bool isStraight = false;

                        int lowestRank = (int)this.CardsInHand.OrderBy(c => c.cardRank).First().cardRank;
                        int cardRankTotal = this.CardsInHand.Sum(r => (int)r.cardRank);

                        if (cardRankTotal == (lowestRank * Program.NumofCardsPerHand + 1 + 2 + 3 + 4))
                        {
                            isStraight = true;
                        }

                        var suitGroupings = from card in this.CardsInHand
                                            group card by card.cardSuit into g
                                            orderby g.Count() descending, g.Key descending
                                            select g;

                        int suitGroupCount = suitGroupings.Count();
                        int valueH = FindHighestCard();

                        if (suitGroupCount == 1)
                        {
                            if (isStraight)
                            {
                                if (valueH == (int)Card.Ranks.Ace)// This is a Royal Flush
                                {
                                    this.HandEvalType = HandType.RoyalFlush;

                                }
                                else // This hand is straight flush.
                                {
                                    this.HandEvalType = HandType.StraightFlush;
                                }
                            }
                            else  // This hand is flush.
                            {
                                this.HandEvalType = HandType.Flush;
                                this.Kicker1 = (int)rankGroupings.ElementAt(1).Key;
                                this.Kicker2 = (int)rankGroupings.ElementAt(2).Key;
                                this.Kicker3 = (int)rankGroupings.ElementAt(3).Key;
                                this.Kicker4 = (int)rankGroupings.ElementAt(4).Key;
                            }
                            this.HighestEvalRank = valueH;
                            this.FlushSuit = (int)this.CardsInHand.First().cardSuit;
                        }
                        else
                        {
                            if (isStraight)//This hand is straight
                            {
                                this.HandEvalType = HandType.Straight;
                            }
                            else //This hand is high card
                            {
                                this.HandEvalType = HandType.HighCard;
                                this.Kicker1 = (int)rankGroupings.ElementAt(1).Key;
                                this.Kicker2 = (int)rankGroupings.ElementAt(2).Key;
                                this.Kicker3 = (int)rankGroupings.ElementAt(3).Key;
                                this.Kicker4 = (int)rankGroupings.ElementAt(4).Key;
                            }
                            this.HighestEvalRank = valueH;
                        }
                        break;
                    }

            }

            return this.HandEvalType;

        }



    }
}
