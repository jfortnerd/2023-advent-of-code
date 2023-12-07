using Jfortnerd.Aod2k23;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aod2k23.Dailies
{
    internal class Day01 : Jfortnerd.Aod2k23.Dailies.Day
    {
        const int FIVE_CHARS_LONG = 5;
        const int FOUR_CHARS_LONG = 4;
        const int THREE_CHARS_LONG = 3;

        public override void RunSolution()
        {
            StringReader sr = new StringReader(Sample);
            String currLine = sr.ReadLine();
            int sum = 0;
            int currLineNum = 1;

            while (currLine != null)
            {
                Console.Write("Process Line : " + currLineNum + '\t');
                currLineNum++;

                int currVal = RetrieveParsedDoubleDigitInt(currLine);

                sum += currVal;

                currLine = sr.ReadLine();
            }

            Solution = "The sum of all the calibration values is: " + sum;


            Console.Write(Solution);
        }

        private int RetrieveNumberFromWord(String currLine)
        {
            currLine = currLine.ToLower();

            if (currLine.Contains("one"))
            {
                currLine = currLine.Replace("one", "1");
            }

            if (currLine.Contains("two"))
            {
                currLine = currLine.Replace("two", "2");
            }

            if (currLine.Contains("three"))
            {
                currLine = currLine.Replace("three", "3");
            }

            if (currLine.Contains("four"))
            {
                currLine = currLine.Replace("four", "4");
            }

            if (currLine.Contains("five"))
            {
                currLine = currLine.Replace("five", "5");
            }

            if (currLine.Contains("six"))
            {
                currLine = currLine.Replace("six", "6");
            }

            if (currLine.Contains("seven"))
            {
                currLine = currLine.Replace("seven", "7");
            }

            if (currLine.Contains("eight"))
            {
                currLine = currLine.Replace("eight", "8");
            }

            if (currLine.Contains("nine"))
            {
                currLine = currLine.Replace("nine", "9");
            }

            return Int32.Parse(currLine);
        }

        private String forwardLookWord(String currLine, int startIndex)
        {
           
            // check if there are enough chars for five char length numbers
            // three seven eight
            if ((startIndex + FIVE_CHARS_LONG) <= currLine.Length)
            {
                String comparison = new String(currLine.Substring(startIndex, FIVE_CHARS_LONG));

                if (String.Equals(comparison, "three"))
                {
                    return comparison;
                }
                
                else if (String.Equals(comparison, "seven"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "eight"))
                {
                    return comparison;
                }
            }

            // check if there are enough chars for four char length numbers
            // four five nine
            if ((startIndex + FOUR_CHARS_LONG) <= currLine.Length)
            {
                String comparison = new String(currLine.Substring(startIndex, FOUR_CHARS_LONG));

                if (String.Equals(comparison, "four"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "five"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "nine"))
                {
                    return comparison;
                }
            }

            // check if there are enough chars for three char length numbers
            // one two six ten
            if ((startIndex + THREE_CHARS_LONG) <= currLine.Length)
            {
                String comparison = new String(currLine.Substring(startIndex, THREE_CHARS_LONG));

                if (String.Equals(comparison, "one"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "two"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "six"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "ten"))
                {
                    return comparison;
                }
            }

            return null;
        }

        private String backwardLookWord(String currLine, int endIndex)
        {
            String currentWindow = new String(currLine.Substring(endIndex));

            // ignore check if the current substring is not long enough
            if (currLine.Length - endIndex < THREE_CHARS_LONG)
            {
                return null;
            }

            // check if there are enough chars for five char length numbers
            // three seven eight
            if (currLine.Length - endIndex >= FIVE_CHARS_LONG)
            {
                String comparison = new String(currLine.Substring(endIndex, FIVE_CHARS_LONG));

                if (String.Equals(comparison, "three"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "seven"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "eight"))
                {
                    return comparison;
                }
            }

            // check if there are enough chars for four char length numbers
            // four five nine
            if (currLine.Length - endIndex >= FOUR_CHARS_LONG)
            {
                String comparison = new String(currLine.Substring(endIndex, FOUR_CHARS_LONG));

                if (String.Equals(comparison, "four"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "five"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "nine"))
                {
                    return comparison;
                }
            }

            // check if there are enough chars for three char length numbers
            // one two six ten
            if (currLine.Length - endIndex >= THREE_CHARS_LONG)
            {
                String comparison = new String(currLine.Substring(endIndex, THREE_CHARS_LONG));

                if (String.Equals(comparison, "one"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "two"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "six"))
                {
                    return comparison;
                }

                else if (String.Equals(comparison, "ten"))
                {
                    return comparison;
                }
            }

            return null;
        }

        private int RetrieveParsedDoubleDigitInt(String currLine)
        {
            String workingLine = new String(currLine);

            // forward look parse string for first digit or "token" that represents a number.
            int currIndex = 0;
            int firstDigit = -1;
            int secondDigit = -1;

            while (currIndex < workingLine.Length)
            {
                // check if char at current index is a digit
                if (Char.IsDigit(workingLine[currIndex]))
                {
                    firstDigit = Int32.Parse(workingLine[currIndex].ToString());

                    // remove confirmed parse so it doesn't get read again
                    workingLine = workingLine.Substring(currIndex + 1);
                    break;
                }
                else
                {
                    // if not a digit, check if the current char and subsequent chars make a word
                    String foundWord = forwardLookWord(workingLine, currIndex);

                    if (foundWord != null) 
                    {
                        firstDigit = RetrieveNumberFromWord(foundWord);

                        // remove confirmed parse so it doesn't get read again
                        workingLine = workingLine.Substring(currIndex + foundWord.Length);
                        break;
                    }
                }

                currIndex++;
            }

            // reset values to look for
            currIndex = workingLine.Length - 1;

            // backward look parse string for second digit or "token" that represents a number.
            while (0 <= currIndex)
            {
                // check if char at current index is a digit
                if (Char.IsDigit(workingLine[currIndex]))
                {
                    secondDigit = int.Parse(workingLine[currIndex].ToString());
                    break;
                }
                else
                {
                    // if not a digit, check if the current char and subsequent chars make a word
                    String foundWord = backwardLookWord(workingLine, currIndex);

                    if (foundWord != null)
                    {
                        secondDigit = RetrieveNumberFromWord(foundWord);
                        break;
                    }
                }

                currIndex--;
            }

            Console.WriteLine("First: " + firstDigit + ", Second: " + secondDigit + '\t' + "for line: "  + currLine);

            if ((firstDigit >= 0) && (secondDigit < 0))
            {
                // first digit populated, so copy first digit to second
                secondDigit = firstDigit;
            }
            else if ((firstDigit < 0) && (secondDigit >= 0))
            {
                // second digit populated, so copy second digit to first
                firstDigit = secondDigit;
            }
            else if ((firstDigit < 0) && (secondDigit < 0))
            {
                // first and second digit not found - ERROR!
                Console.Write("ERROR - Can't find digits in: " + currLine);
                System.Environment.Exit(1);
            }

            return Int32.Parse(firstDigit.ToString() + secondDigit.ToString());
        }
    }
}
