using Jfortnerd.Aod2k23.Dailies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aod2k23.Dailies
{
    internal class Day04 : Day
    {
        const bool SurpressConsoleOutput = true;

        const char CharColon = ':';
        const char CharSpace = ' ';
        const char CharPipe = '|';
        const int Spacer = 2;           // the amount of characters between the index of the current char plus one space
        const int WinningNumberMultipler = 2;

        public override void RunSolution()
        {
            // parse input
            StringReader sr = new StringReader(Sample);
            String currLine = new String("");
            String workingLine = new String("");
            int currCard = 0;
            int charColonIndex = 0;
            int charPipeIndex = 0;

            Dictionary<int, (List<int>, List<int>)> scratchTickets = new Dictionary<int, (List<int>, List<int>)>();
            Dictionary<int, int> copyCount = new Dictionary<int, int>();
            (List<int>, List<int>) gameNumbers;
            List<int> winningNumbers;
            List<int> ticketNumbers;
            
            int totalPoints = 0;
            int totalScratchCards = 0;

            // read first line from sample
            currLine = sr.ReadLine();

            // while there are lines to read from, keep iterating 
            while (currLine != null)
            {
                // retrieve card number
                // --   parse out colon
                charColonIndex = currLine.IndexOf(CharColon);
                workingLine = currLine.Substring(0, charColonIndex);

                // --   parse out space
                workingLine = workingLine.Substring(currLine.IndexOf(CharSpace));

                // --   retrieve integer from string representation
                currCard = Int32.Parse(workingLine);

                // retrieve winning numbers
                charPipeIndex = currLine.IndexOf(CharPipe);
                workingLine = currLine.Substring(charColonIndex + Spacer);
                workingLine = workingLine.Substring(0, charPipeIndex - charColonIndex - Spacer - 1);
                winningNumbers = RetrieveIntListFromArray(currCard, workingLine.Split(CharSpace));

                // retrieve ticket numbers
                workingLine = currLine.Substring(charPipeIndex + 1);
                ticketNumbers = RetrieveIntListFromArray(currCard, workingLine.Split(CharSpace));

                // retrieve game numbers
                gameNumbers = (winningNumbers, ticketNumbers);

                if (!SurpressConsoleOutput)
                {
                    // Console write current card
                    Console.WriteLine("Card " + currCard);

                    // Console write winning numbers
                    Console.Write("\tWinning Numbers: [");
                    winningNumbers.ForEach(number => Console.Write(number + " "));
                    Console.WriteLine("]");

                    // Console write ticket numbers
                    Console.Write("\tTicket Numbers: [");
                    ticketNumbers.ForEach(number => Console.Write(number + " "));
                    Console.WriteLine("]\n");
                }

                // add to dictionary of scratch tickets
                scratchTickets.Add(currCard, gameNumbers);

                // add to dictionary of copy counts
                copyCount.Add(currCard, 1);

                // read next line
                currLine = sr.ReadLine();
            }

            totalPoints = RetrieveTotalPointsFromScratchTicketPile(scratchTickets);

            Solution = "The scratchcard pile is worth a point total value of: " + totalPoints + "\n";

            totalScratchCards = RetrieveTotalScratchCardsFromCopyRule(scratchTickets, copyCount); 

            AppendSolution("The total number of scratch cards on the table is: " + totalScratchCards + "\n");
        }

        private int GetMatchingNumberCount(List<int> winningNumbers, List<int> ticketNumbers)
        {
            int count = 0;

            foreach (int number in ticketNumbers)
            {
                if (winningNumbers.Contains(number))
                {
                    count++;
                }
            }

            return count;
        }

        private int RetrieveTotalScratchCardsFromCopyRule(Dictionary<int, (List<int>, List<int>)> tickets,
                                                          Dictionary<int, int> copyCount)
        {
            int totalScratchCards = 0;
            int matchingNumberCount = 0;

            foreach (int ticketNum in tickets.Keys)
            {
                matchingNumberCount = GetMatchingNumberCount(tickets[ticketNum].Item1, tickets[ticketNum].Item2);

                Console.Write("Current Game: " + ticketNum + "\n");

                // Console write winning numbers
                Console.Write("\tWinning Numbers: [");
                tickets[ticketNum].Item1.ForEach(number => Console.Write(number + " "));
                Console.WriteLine("]");

                // Console write ticket numbers
                Console.Write("\tTicket Numbers: [");
                tickets[ticketNum].Item2.ForEach(number => Console.Write(number + " "));
                Console.WriteLine("]\n");

                // Console write matching number count
                Console.WriteLine("\tMatching number count: " + matchingNumberCount + "\n");

                // adjust matching number count for end of table
                if ((tickets.Count - ticketNum) < matchingNumberCount)
                {
                    matchingNumberCount = matchingNumberCount - (tickets.Count - ticketNum);
                }

                while (matchingNumberCount > 0)
                {
                    for (int currCopyCount = 0; currCopyCount < copyCount[ticketNum]; currCopyCount++)
                    {
                        copyCount[ticketNum + matchingNumberCount] = copyCount[ticketNum + matchingNumberCount] + 1;
                    }
                    
                    matchingNumberCount--;
                }

                Console.Write("\tTotal Scratchcards: " + copyCount[ticketNum] + "\n");

                totalScratchCards += copyCount[ticketNum];
            }

            return totalScratchCards;
        }

        private List<int> RetrieveIntListFromArray(int row, String[] tokens)
        {
            List<int> intList = new List<int>();
            int currInt = 0;

            foreach (String token in tokens)
            {
                // if empty string, continue
                if (token.Trim().Length == 0)
                {
                    continue;
                }

                if (!Int32.TryParse(token, out currInt))
                {
                    Console.Write("At row " + row + ", cannot process: " + token + " ...as an integer.");
                    System.Environment.Exit(1);
                } 
                else
                {
                    intList.Add(currInt);
                }
            }

            intList.Sort();

            return intList;
        }

        private int RetrieveTotalWinnings(List<int> winningNumbers, List<int> ticketNumbers)
        {
            int totalPoints = 0;

            foreach (int num in ticketNumbers)
            {
                if (winningNumbers.Contains(num))
                {
                    if (totalPoints == 0)
                    {
                        totalPoints = 1;
                    } 
                    else
                    {
                        totalPoints *= WinningNumberMultipler;
                    }
                }
            }

            return totalPoints;
        }

        private int RetrieveTotalPointsFromScratchTicketPile(Dictionary<int, (List<int>, List<int>)> tickets)
        {
            int totalPoints = 0;
            (List<int>, List<int>) currentGame;
            List<int> winningNumbers;
            List<int> ticketNumbers;

            foreach(int ticketIdent in tickets.Keys)
            {
                currentGame = tickets[ticketIdent];
                winningNumbers = currentGame.Item1;
                ticketNumbers = currentGame.Item2;

                totalPoints += RetrieveTotalWinnings(winningNumbers, ticketNumbers);

                if (!SurpressConsoleOutput)
                {
                    Console.Write("Current Game: " + ticketIdent + "\n");
                    Console.Write("\tTotal Points: " + totalPoints + "\n");
                }
              
            }
            return totalPoints;
        }
    }
}
