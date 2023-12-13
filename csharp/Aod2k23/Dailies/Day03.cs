using Jfortnerd.Aod2k23.Dailies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aod2k23.Dailies
{
    internal class Day03 : Day 
    {
        const bool SurpressConsoleProgressOutput = true;
        const int SquareWidth = 3;

        public override void RunSolution()
        {
            // Store engine schematic
            Dictionary<int, int> engineNumbers = new Dictionary<int, int>();
            Dictionary<int, char> engineSymbols = new Dictionary<int, char>();

            // parse input
            StringReader sr = new StringReader(Sample);
            String currLine = sr.ReadLine();

            int maxLineCharNum = -1;
            int maxRowNum = -1;
            int currRow = 0;
            int currCol = 0;
            int startEngineNumIndex = -1;
            int endEngineNumIndex = -1;
            int enginePartsSum = -1;
            int gearRatioSum = -1;
            


            // while file has more lines to read
            while (currLine != null)
            {
                if (!SurpressConsoleProgressOutput)
                {
                    Console.WriteLine("Start Parse at Line: " + (currRow + 1));
                }
                

                // get max length of lines from first line
                if (maxLineCharNum < 0)
                {
                    maxLineCharNum = currLine.Length;
                } else
                {
                    // also sanity check if next lines are the same length, if not ERROR!
                    if (currLine.Length != maxLineCharNum)
                    {
                        Console.WriteLine("Current line doesn't max char length, at Line: " + (currRow + 1));
                        System.Environment.Exit(1);
                    }
                }


                // while we still have more chars to parse
                while (currCol < maxLineCharNum)
                {
                    // check if current char is a digit
                    if (Char.IsDigit(currLine[currCol]))
                    {
                        // check if we did not start tracking the engine number
                        // if not, start
                        if (startEngineNumIndex < 0)
                        {
                            startEngineNumIndex = currCol;
                        }

                        // check if we reached the end of the line
                        // if so, end the tracking
                        if ((currCol + 1) == maxLineCharNum)
                        {
                            endEngineNumIndex = currCol;

                            // add engine number to Dictionary
                            TrackEngineNumber(engineNumbers, currLine, currRow, startEngineNumIndex, endEngineNumIndex, maxLineCharNum);

                            // reset tracking indices 
                            startEngineNumIndex = -1;
                            endEngineNumIndex = -1;
                        }
                    }
                    else
                    {
                        // if curr char not a digit, check if we have started tracking engine number
                        // if so, end
                        if (startEngineNumIndex > -1)
                        {
                            endEngineNumIndex = currCol - 1;

                            // add engine number to Dictionary
                            TrackEngineNumber(engineNumbers, currLine, currRow, startEngineNumIndex, endEngineNumIndex, maxLineCharNum);

                            // reset tracking indices 
                            startEngineNumIndex = -1;
                            endEngineNumIndex = -1;
                        }

                        // check if current char is not a period
                        // if not, track symbol
                        if (currLine[currCol] != '.')
                        {
                            TrackEngineSymbol(engineSymbols, currLine[currCol], currRow, currCol, maxLineCharNum);
                        }
                    }

                    currCol++;
                }

                // read next line in file
                currLine = sr.ReadLine();
                currCol = 0;
                currRow++;
            }

            maxRowNum = currRow;

            enginePartsSum = RetrieveSumOfEnginePartNumbers(engineNumbers, engineSymbols, maxLineCharNum, maxRowNum);

            Solution = "The sum of all the part numbers in the engine schematic is: " + enginePartsSum;

            // Calculate the sum of the gear ratios
            gearRatioSum = RetrieveSumOfGearRatios(engineNumbers, engineSymbols, maxLineCharNum, maxRowNum);

            AppendSolution("\nThe sum of all of the gear ratios is : " + gearRatioSum + "\n");
        }

        private List<int> RetrieveValidEngineParts(Dictionary<int, int> engineNumbers, List<int> validSpaces, int engineSymbolKey)
        {
            List<int> validPartsNearGear = new List<int>();
            int prevIdent = -1;
            int prevEngineNumber = -1;
            int currEngineNumber;
            bool partAlreadyAdded = false;

            foreach(int currIdent in validSpaces)
            {
                if (engineNumbers.ContainsKey(currIdent))
                {
                    if (!engineNumbers.TryGetValue(currIdent, out currEngineNumber))
                    {
                        Console.WriteLine("Error: Can't parse value at engine number key: " + currIdent);
                        System.Environment.Exit(1);
                    }

                    // check if part was already added 
                    if (prevIdent < 0)
                    {
                        prevIdent = currIdent;
                        prevEngineNumber = currEngineNumber;
                    }
                    else
                    {
                        if ((currIdent - prevIdent) < SquareWidth) 
                        {
                            if ((currIdent + 1) == engineSymbolKey)
                            {
                                // do nothing, since it's directly left of the symbol
                            }
                            else if ((currIdent - 1) == engineSymbolKey)
                            {
                                // do nothing, since it's directly right of the symbol
                            }
                            else
                            {
                                if (currEngineNumber == prevEngineNumber)
                                {
                                    partAlreadyAdded = true;
                                }
                            }
                        }

                        prevIdent = currIdent;
                        prevEngineNumber = currEngineNumber;
                    }

                    if (!partAlreadyAdded)
                    {
                        validPartsNearGear.Add(currEngineNumber);
                    }

                    partAlreadyAdded = false;
                }
            }

            return validPartsNearGear;
        }

        private int RetrieveSumOfGearRatios(Dictionary<int, int> engineNumbers, Dictionary<int, char> engineSymbols, int maxCol, int maxRow)
        {
            List<int> validEngineParts;
            List<int> validSpaces;
            int gearRatioSum = 0;
            char currEngineSymbol;

            foreach (int engineSymbolKey in engineSymbols.Keys)
            {
                if (!engineSymbols.TryGetValue(engineSymbolKey, out currEngineSymbol))
                {
                    Console.WriteLine("Error: Can't parse value at engine symbol key: " + engineSymbolKey);
                    System.Environment.Exit(1);
                }

                // skip this engine symbol if it is not an asterisk
                // * - this symbol represents a gear in the schematic
                if (currEngineSymbol != '*')
                {
                    continue;
                }

                // retrieve valid spaces to verify parts to check near engine symbol
                validSpaces = RetrieveValidSpaces(engineSymbolKey, 1, maxCol, maxRow);

                // determine from valid spaces and valid engine numbers...
                // ...whether engine symbol is a valid gear
                // symbol is valid when there are only two engine parts nearby
                validEngineParts = RetrieveValidEngineParts(engineNumbers, validSpaces, engineSymbolKey); 

                if (validEngineParts.Count == 2)
                {
                    int currProduct = 1;

                    foreach (int partNumber in validEngineParts)
                    {
                        currProduct *= partNumber;
                    }

                    gearRatioSum += currProduct;
                }
            }

            return gearRatioSum;
        }

        private List<int> RetrieveValidSpaces(int ident, int engineNumber, int maxLineCharNum, int maxRowNum)
        {
             List<int> validSpaces = new List<int>();
            int currIndex = -1;

            // using cardinal directions...
            // check Northwest of ident is valid
            if (ident % maxLineCharNum != 1)
            {
                currIndex = ident - maxLineCharNum - 1;

                if ((currIndex > 0))
                {
                    validSpaces.Add(currIndex);
                }
            }

            // check if Southwest of ident is valid
            if (ident % maxLineCharNum != 1)
            {
                currIndex = ident + maxLineCharNum - 1;

                if ((currIndex <= (maxRowNum * maxLineCharNum)))
                {
                    validSpaces.Add(currIndex);
                }
            }

            // check North of ident is valid, especially if engine number has more than one digit
            currIndex = ident - maxLineCharNum;

            if (currIndex > 0)
            {
                for (int i = currIndex; i < currIndex + engineNumber.ToString().Length; i++) {
                    validSpaces.Add(i);
                }
            }

            // check if South of ident is valid, especially if engine number has more than one digit
            currIndex = ident + maxLineCharNum;

            if (currIndex <= (maxRowNum * maxLineCharNum))
            {
                for (int i = currIndex; i < currIndex + engineNumber.ToString().Length; i++)
                {
                    validSpaces.Add(i);
                }
            }

            // check Northeast of ident is valid
            currIndex = ident - maxLineCharNum + engineNumber.ToString().Length;

            if (ident % maxLineCharNum != 0)
            {
                if ((currIndex > 0) && (((ident % maxLineCharNum) + engineNumber.ToString().Length) <= maxLineCharNum))
                {
                    validSpaces.Add(currIndex);
                }
            }


            // check Southeast of ident is valid
            if (ident % maxLineCharNum != 0)
            {
                currIndex = ident + maxLineCharNum + engineNumber.ToString().Length;

                if ((currIndex <= (maxRowNum * maxLineCharNum)) && (((ident % maxLineCharNum) + engineNumber.ToString().Length) <= maxLineCharNum))
                {
                    validSpaces.Add(currIndex);
                }
            }
 

            // check if West of ident is valid
            currIndex = ident - 1;

            if (ident % maxLineCharNum != 1)
            {
                if (currIndex > 0)
                {
                    validSpaces.Add(currIndex);
                }
            }

            // check if East of ident is valid
            if (ident % maxLineCharNum != 0)
            {
                currIndex = ident + engineNumber.ToString().Length;

                if (((ident % maxLineCharNum) + engineNumber.ToString().Length) <= maxLineCharNum)
                {
                    validSpaces.Add(currIndex);
                }
            }

            validSpaces.Sort();

            return validSpaces;
        }

        private bool CheckIfValidPart(List<int> validSpaces, Dictionary<int, char> engineSymbols)
        {
            bool validPart = false;

            foreach (int space in validSpaces)
            {
                if (engineSymbols.ContainsKey(space))
                {
                    validPart = true;
                }
            }

            return validPart;
        }

        private int RetrieveSumOfEnginePartNumbers(Dictionary<int, int> engineNumbers, Dictionary<int, char> engineSymbols, int maxLineCharNum, int maxRowNum)
        {
            List<int> validSpaces;
            int enginePartSum = 0;
            bool symbolFound = false;

            int prevKey = -1;
            int prevValue = -1;
            bool skipEngineNumber = false;

            foreach (KeyValuePair<int, int> engineNumber in engineNumbers)
            {
                // check if engine number is still the current one
                // i.e. engine numbers can represent more than one digit
                if (engineNumber.Key == prevKey + 1)
                {
                    if (engineNumber.Value == prevValue)
                    {
                        // sanity check for word wrap
                        if (engineNumber.Key % maxLineCharNum != 1)
                        {
                            skipEngineNumber = true;
                        }
                    }
                }

                // set current key/value as the previous...done with check
                prevKey = engineNumber.Key;
                prevValue = engineNumber.Value;

                if (skipEngineNumber)
                {
                    skipEngineNumber = false;
                    continue;
                }

                // retrieve all valid spaces 
                validSpaces = RetrieveValidSpaces(engineNumber.Key, engineNumber.Value, maxLineCharNum, maxRowNum);

                // iterate through valid spaces
                if (validSpaces.Count > 0)
                {
                    symbolFound = CheckIfValidPart(validSpaces, engineSymbols);
                }

                // if symbol found, add to sum
                if (symbolFound)
                {
                    if (!SurpressConsoleProgressOutput)
                    {
                        Console.WriteLine("Engine part valid: " + engineNumber.Value);
                    }
                    
                    enginePartSum += engineNumber.Value;
                }
            }

            return enginePartSum;
        }

        private void TrackEngineSymbol(Dictionary<int, char> engineSymbols, Char symbol, int row, int col, int maxLength)
        {
            int ident = (col + 1) + (row * maxLength);
            engineSymbols.Add(ident, symbol);

            if (!SurpressConsoleProgressOutput)
            {
                Console.WriteLine("Adding (" + ident + ", " + symbol + ")");
            }
        }

        private void TrackEngineNumber(Dictionary<int, int> engineNumbers,
                                       String currLine,        
                                       int currRow,
                                       int startIndex,
                                       int endIndex,
                                       int maxLength)
        {
            int ident = -1;
            int engineNumber = -1;

            // retrieve engine number
            engineNumber = Int32.Parse(currLine.Substring(startIndex, endIndex - startIndex + 1));

            for (int i = startIndex; i <= endIndex; i++)
            {
                // create identifier for engine number
                //  formula: (current column + 1) + (current row * max length)
                ident = (i + 1) + (currRow * maxLength);
                engineNumbers.Add(ident, engineNumber);

                if (!SurpressConsoleProgressOutput)
                {
                    Console.WriteLine("Adding (" + ident + ", " + engineNumber + ")");
                }
            }

            
        }
    }
}
