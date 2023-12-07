using Jfortnerd.Aod2k23.Dailies;

namespace Aod2k23.Dailies
{
    internal class Day02 : Day
    {
        private const char CHAR_COLON = ':';
        private const char CHAR_SPACE = ' ';
        private const char CHAR_SEMICOLON = ';';
        private const char CHAR_COMMA = ',';
        private const int SPACER = 2;                   // to account for removing the char and the space after the char

        private const int RED_CUBES = 0;
        private const int GREEN_CUBES = 1;
        private const int BLUE_CUBES = 2;
        private const int NUM_OF_CUBE_COLORS = 3;

        public override void RunSolution()
        {
            // set up reader to parse input file
            StringReader sr = new StringReader(Sample);
            List<int[]> cubeSubsetList = null;
            String currLine = new String("");
            String currGame = new String("");
            String currSubset = new String("");
            int[] maxCubesAllowed = new int[NUM_OF_CUBE_COLORS];

            int solutionSum = 0;
            int powerOfMinSetSum = 0;
            maxCubesAllowed = RetrieveMaxCubesAllowed();
            currLine = sr.ReadLine();

            // keep reading lines if they exist
            while (currLine != null)
            {
                int gameNum = -1;
                bool gameValid = true;

                // Parse current game number
                currGame = currLine.Substring(0, currLine.IndexOf(CHAR_COLON));
                currGame = currGame.Substring(currLine.IndexOf(CHAR_SPACE));
                gameNum = Int32.Parse(currGame);
                Console.WriteLine("Game: " +  gameNum);

                // Parse game subsets
                currSubset = currLine.Substring(currLine.IndexOf(CHAR_COLON) + SPACER);
                cubeSubsetList = new List<int[]>();

                while (currSubset.Length > 0)
                {
                    int currSubsetIndex = currSubset.IndexOf(CHAR_SEMICOLON);

                    // if no subset index found, then only one cube pull is left
                    if (currSubsetIndex == -1)
                    {
                        int[] newListItem = ParseCubeHand(currSubset);
                        cubeSubsetList.Add(newListItem);

                        currSubset = currSubset.Remove(0, currSubset.Length);
                    }
                    else
                    {
                        // parse out first cube pull and seperate/iterate the rest of the pulls
                        int[] newListItem = ParseCubeHand(currSubset.Substring(0, currSubsetIndex));
                        cubeSubsetList.Add(newListItem);

                        currSubset = currSubset.Remove(0, currSubsetIndex + SPACER);
                    }
                    
                }

                Console.WriteLine("[RED, GREEN, BLUE]");

                foreach(int[] subset in cubeSubsetList)
                {
                    Console.WriteLine("\t[" + subset[RED_CUBES] + ", " + subset[GREEN_CUBES] + ", " + subset[BLUE_CUBES] + "]");
                }

                gameValid = IsGameValid(cubeSubsetList, maxCubesAllowed);

                // if valid game, add current game number to solution sum
                if (gameValid)
                {
                    solutionSum += gameNum;
                }

                // whether or not game is valid, calculate the summation of the min set powers
                powerOfMinSetSum += CalculatePowerOfMinSet(cubeSubsetList);


                // Read next line in file
                currLine = sr.ReadLine();
            }

            AppendSolution("The sum of the IDs of valid games is: " + solutionSum);
            AppendSolution("The sum of the power of these sets is: " + powerOfMinSetSum);
        }

        private int CalculatePowerOfMinSet(List<int[]> cubeSet)
        {
            int[] maxCubeCount = new int[NUM_OF_CUBE_COLORS];
            maxCubeCount[RED_CUBES] = 0;
            maxCubeCount[GREEN_CUBES] = 0;
            maxCubeCount[BLUE_CUBES] = 0;

            foreach (int[] subset in cubeSet)
            {
                if (subset[RED_CUBES] > maxCubeCount[RED_CUBES])
                {
                    maxCubeCount[RED_CUBES] = subset[RED_CUBES];
                }

                if (subset[GREEN_CUBES] > maxCubeCount[GREEN_CUBES])
                {
                    maxCubeCount[GREEN_CUBES] = subset[GREEN_CUBES];
                }

                if (subset[BLUE_CUBES] > maxCubeCount[BLUE_CUBES])
                {
                    maxCubeCount[BLUE_CUBES] = subset[BLUE_CUBES];
                }
            }

            return maxCubeCount[RED_CUBES] * maxCubeCount[GREEN_CUBES] * maxCubeCount[BLUE_CUBES];
        }

        private int ValidateIntegerInput(String prompt)
        {
            String input;
            bool parsedInputValid = false;
            int value = -1;

            Console.Write(prompt);

            while(!parsedInputValid)
            {
                input = Console.ReadLine();
                parsedInputValid = Int32.TryParse(input, out value);

                if (!parsedInputValid)
                {
                    Console.Write("Invalid input: " + input);
                    Console.Write(prompt);
                }
            }

            return value;
        }

        private int[] RetrieveMaxCubesAllowed()
        {
            int[] maxCubesAllowed = new int[NUM_OF_CUBE_COLORS];

            maxCubesAllowed[RED_CUBES] = ValidateIntegerInput("What is the max # of RED cubes allowed? Enter here: ");
            maxCubesAllowed[GREEN_CUBES] = ValidateIntegerInput("What is the max # of GREEN cubes allowed? Enter here: ");
            maxCubesAllowed[BLUE_CUBES] = ValidateIntegerInput("What is the max # of BLUE cubes allowed? Enter here: ");

            return maxCubesAllowed;
        }

        private bool IsGameValid(List<int[]> cubeSubsetList, int[] maxCubesAllowed)
        {
            bool isGameValid = true;

            foreach (int[] subset in cubeSubsetList)
            {
                if (subset[RED_CUBES] > maxCubesAllowed[RED_CUBES])
                {
                    isGameValid = false;
                    break;
                }

                if (subset[BLUE_CUBES] > maxCubesAllowed[BLUE_CUBES])
                {
                    isGameValid = false;
                    break;
                }

                if (subset[GREEN_CUBES] > maxCubesAllowed[GREEN_CUBES])
                {
                    isGameValid = false;
                    break;
                } 
            }

            return isGameValid;
        }

        private int DetermineColor(String colorStrRep)
        {
            if (colorStrRep.ToLower().Contains("red")) {
                return RED_CUBES;
            }
            else if (colorStrRep.ToLower().Contains("green"))
            {
                return GREEN_CUBES;
            } 
            else if (colorStrRep.ToLower().Contains("blue"))
            {
                return BLUE_CUBES;
            }
            else
            {
                return -1;
            }
        }

        private int[] ParseCubeHand(String cubeStrRep)
        {
            String currLine = new String(cubeStrRep);

            int[] cubeHand = new int[NUM_OF_CUBE_COLORS];
            int currColor;
            int currCount;

            while (currLine.Length > 0)
            {
                int currColorIndex = currLine.IndexOf(CHAR_COMMA);

                // if no index found, this is the only color in the hand
                if (currColorIndex  == -1)
                {               
                    currColor = DetermineColor(currLine);

                    if (currColor == -1 )
                    {
                        Console.WriteLine("Invalid cube color retrieved.");
                        System.Environment.Exit(-1);
                    }

                    currCount = Int32.Parse(currLine.Substring(0, currLine.IndexOf(CHAR_SPACE)));

                    if (currColor == RED_CUBES)
                    {
                        cubeHand[RED_CUBES] = currCount;
                    }
                    else if (currColor == GREEN_CUBES)
                    {
                        cubeHand[GREEN_CUBES] = currCount;
                    } 
                    else if (currColor == BLUE_CUBES)
                    {
                        cubeHand[BLUE_CUBES] = currCount;
                    }

                    currLine = currLine.Remove(0, currLine.Length);
                }
                else
                {
                    
                    currColor = DetermineColor(currLine.Substring(0, currColorIndex));

                    if (currColor == -1)
                    {
                        Console.WriteLine("Invalid cube color retrieved.");
                        System.Environment.Exit(-1);
                    }

                    currCount = Int32.Parse(currLine.Substring(0, currLine.IndexOf(CHAR_SPACE)));

                    if (currColor == RED_CUBES)
                    {
                        cubeHand[RED_CUBES] = currCount;
                    }
                    else if (currColor == GREEN_CUBES)
                    {
                        cubeHand[GREEN_CUBES] = currCount;
                    }
                    else if (currColor == BLUE_CUBES)
                    {
                        cubeHand[BLUE_CUBES] = currCount;
                    }

                    // parse out first color, iterate the rest
                    currLine = currLine.Remove(0, currColorIndex + SPACER);

                }
            }

            return cubeHand;
        }
    }
}
