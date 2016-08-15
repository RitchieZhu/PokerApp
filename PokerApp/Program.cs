using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace PokerApp
{
    class Program
    {
        public static readonly int NumofCardsPerHand = 5;
        public static readonly int NumofHands = 5;



        static void Main(string[] args)
        {
            ArrayList allhands = new ArrayList();
            //PokerHand winnerHand = new PokerHand();
           

            //Read in the hands from user's input
            allhands = ReadInput();

            //Evaluate the winning hands
            List<PokerHand> winnerHands = EvaluateHands(allhands);

            //Wrtie out the results
            Console.WriteLine("Winners:");
            if (winnerHands.Count > 0)
            {
                DisplayWinners(winnerHands);
            }
            else
            {
                Console.Write( "Please check your cards and restart.");
            }
            //DisplayResults(allhands);

            Console.Read();
        }



        private static ArrayList ReadInput()
        {
            //Set the prompt
            string instruct = " Please type in the  " + NumofHands + " player names and hands of cards in the format of Joe, 4D, 5H, 6S, QC, AD.";
            Console.WriteLine(instruct);


            string[] cardStrArr = null;
            Card[] cardsInHand = new Card[5];
            ArrayList hands = new ArrayList();

            //repeating the input process until the number of hands is reached.
            for (int i = 1; i <= NumofHands; i++)
            {
                Console.WriteLine("Hand  " + i.ToString() + ": ");
                string cInput = Console.ReadLine();

                if (cInput.Length > 0)
                {
                    //Parse the input into the card and pokerhand objects
                    string player = (cInput.Substring(0, cInput.IndexOf(',') + 1)).Trim(',');
                    string cardStrings = cInput.Replace(player, "");
                    cardStrArr = Array.ConvertAll(cardStrings.Trim(',').TrimEnd('.').Split(','), c => c.Trim().Replace(" ", ""));

                    PokerHand currentHand = new PokerHand();
                    List<Card> CurCardsInHand = new List<Card>();

                    foreach (string cardInput in cardStrArr)
                    {
                        Card currentCard = new Card(cardInput);
                        if (currentCard != null)
                        {
                            if (string.IsNullOrEmpty(currentCard.error))
                            {
                                CurCardsInHand.Add(currentCard);
                            }
                            else
                            {
                                //Validation and prompt reenter
                                Console.WriteLine(currentCard.error + " Please check and re-enter.");
                                cInput = Console.ReadLine();
                            }

                        }
                        else
                        {
                            Console.WriteLine("Sorry, can't read the card in this hand. please check and re-enter.");
                            cInput = Console.ReadLine();
                        }
                    }

                    if (CurCardsInHand.Count == NumofCardsPerHand)
                    {
                        currentHand.PlayerName = player;
                        currentHand.CardsInHand = CurCardsInHand;
                        hands.Add(currentHand);
                    }
                    else
                    {
                        Console.WriteLine("Sorry, this hand is missing some card. please check and re-enter.");
                        cInput = Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Sorry, No cards wer dealt. please check and re-enter.");
                    cInput = Console.ReadLine();
                }
            }


            return hands;
        }


        private static List<PokerHand> EvaluateHands(ArrayList allhands)
        {

            //evaluate each hand and compare it against the best hand and update the winner list
            List<PokerHand> winHands = new List<PokerHand>();
            PokerHand BestHand = new PokerHand();
            if (allhands.Count > 0)
            {
                //evaluate the first hand and initialize the best hand
                BestHand = (PokerHand)allhands[0];
                BestHand.evalHand();
                winHands.Add(BestHand);

                //Console.WriteLine(BestHand.HandEvalType.ToString() + " with " +(BestHand.HighestEvalRank).ToString() + " high. ");
                for (int h = 1; h < allhands.Count; h++)
                {

                    //evaluate the next hand 
                    PokerHand curHand = (PokerHand)allhands[h];
                    curHand.evalHand();


                    //Make the comparisons and apply the tie breaking rules
                    if (curHand.HandEvalType > BestHand.HandEvalType)
                    {
                        BestHand = curHand;
                        winHands.Clear();
                        winHands.Add(curHand);
                    }
                    else if (curHand.HandEvalType == BestHand.HandEvalType)
                    {
                        if (curHand.HighestEvalRank > BestHand.HighestEvalRank)
                        {
                            BestHand = curHand;
                            winHands.Clear();
                            winHands.Add(curHand);
                        }
                        else if (curHand.HighestEvalRank == BestHand.HighestEvalRank)
                        {
                            if (curHand.SecondaryEvalRank > BestHand.SecondaryEvalRank)
                            {
                                BestHand = curHand;
                                winHands.Clear();
                                winHands.Add(curHand);
                            }
                            else if (curHand.SecondaryEvalRank == BestHand.SecondaryEvalRank)
                            {
                                if (curHand.Kicker1 > BestHand.Kicker1)  //Tie breaker
                                {
                                    BestHand = curHand;
                                    winHands.Clear();
                                    winHands.Add(curHand);
                                }
                                else if (curHand.Kicker1 == BestHand.Kicker1)
                                {
                                    if (curHand.Kicker2 > BestHand.Kicker2)  //Tie breaker
                                    {
                                        BestHand = curHand;
                                        winHands.Clear();
                                        winHands.Add(curHand);
                                    }
                                    else if (curHand.Kicker2 == BestHand.Kicker2)
                                    {

                                        if (curHand.Kicker3 > BestHand.Kicker3)  //Tie breaker
                                        {
                                            BestHand = curHand;
                                            winHands.Clear();
                                            winHands.Add(curHand);
                                        }
                                        else if (curHand.Kicker3 == BestHand.Kicker3)
                                        {
                                            if (curHand.Kicker4 > BestHand.Kicker4)  //Tie breaker
                                            {
                                                BestHand = curHand;
                                                winHands.Clear();
                                                winHands.Add(curHand);
                                            }
                                            else if (curHand.Kicker4 == BestHand.Kicker4)
                                            {
                                                winHands.Add(curHand);
                                            }

                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }

            return winHands;
        }


        private static void DisplayResults(ArrayList AllHands)
        {

            string output = "";
            foreach (PokerHand hand in AllHands)
            {
                output += hand.PlayerName + ": ";
                DisplayHand(hand);
            }
            Console.Write(output);

        }

        private static void DisplayWinners(List<PokerHand> winnerHands)
        {
            string outputWinner = "";
            
            // write out the winners names
            foreach (PokerHand hand in winnerHands)
            {
                outputWinner += hand.PlayerName + ", ";
                outputWinner += DisplayHand(hand);
                outputWinner += ",";
            }
            outputWinner = outputWinner.Trim(',');
            Console.WriteLine(outputWinner);
        }

        private static string DisplayHand(PokerHand Onehand)
        {
            string outputHand = "";
            foreach (Card eachCard in Onehand.CardsInHand)
            {
                //outputHand += (eachCard.cardRank).ToString() + (eachCard.cardSuit).ToString() + ", ";

            }

            //wrtie out the hand type 
            outputHand += Onehand.HandEvalType + ", ";
            outputHand = outputHand.Trim(',');

            return outputHand;
        }

    }
}
