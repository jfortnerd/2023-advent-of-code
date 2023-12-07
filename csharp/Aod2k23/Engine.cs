using Aod2k23.Dailies;
using Jfortnerd.Aod2k23.Dailies;
using System;
using System.IO;

namespace Jfortnerd.Aod2k23
{
    class Engine
    {
        private enum ErrorType
        {
            Arguments,
            ParseString,
            NotImplemented
        }

        public static void Main(string[] args)
        {
            // check for proper number of arguments
            if (args.Length == 0)
            {
                DisplayError("No arguments found.", ErrorType.Arguments);
                return;
            }
            else if (args.Length > 2)
            {
                DisplayError("Too many arguments found - cannot be more than 2.", ErrorType.Arguments);
                return;
            }

            // check if help argument used, and for proper usage
            if (args[0].Equals("--help"))
            {
                if (args.Length > 1)
                {
                    DisplayError("Too many arguments found - only use 1 argument for --help.", ErrorType.Arguments);
                }
                else
                {
                    DisplayHelp();
                }
            }
            // else check if day argument used, and for proper usage
            else if (args[0].Equals("day"))
            {
                if (args.Length == 1)
                {
                    DisplayError("Too few arguments found - only use 1 argument for day.", ErrorType.Arguments);
                }
                else
                {
                    int currentDay = ParseDayInt(args[1]);

                    if (currentDay != -1) {
                        RunDailySolution(currentDay);
                    }
                }
            } 
            // else check if filegen argument used, and for proper usage
            else if (args[0].Equals("filegen"))
            {
                if (args.Length == 1)
                {
                    DisplayError("Too few arguments found - only use 1 argument for day.", ErrorType.Arguments);
                }
                else
                {
                    int currentDay = ParseDayInt(args[1]);

                    if (currentDay != -1)
                    {
                        GenerateDailyFiles(currentDay);
                    }
                }
            }
            else
            {
                DisplayError("Argument does not exist: " + args[0], ErrorType.Arguments);
            }

            Thread.Sleep(3000);
            System.Environment.Exit(0);
        }

        private static int ParseDayInt(string numberString)
        {
            if (Int32.TryParse(numberString, out int result))
            {
                if ((result > 31) || (result < 1))
                {
                    DisplayError("Second argument is not a valid day.", ErrorType.ParseString);
                    return -1;
                }
                return result;
            }
            else
            {
                DisplayError("Second argument could not be parsed as a number.", ErrorType.ParseString);
                return -1;
            }
        }

        private static void GenerateDailyFiles(int day)
        {
            String inputDir = "D:\\source\\2023-advent-of-code\\csharp\\input\\";
            String outputDir = "D:\\source\\2023-advent-of-code\\csharp\\output\\";
            String dayString = "";
            String fileName = "";
            

            if (day >= 10)
            {
                dayString = day.ToString();
            } 
            else
            {
                dayString = "0" + day.ToString();
            }

            fileName = inputDir + "Day" + dayString + "In.txt";

            if (!File.Exists(fileName)) {
                createNewFile(fileName);
                Console.WriteLine("File generated: " + fileName);
            }
            else
            {
                Console.WriteLine("File NOT generated, already exists: " + fileName);
            }

            fileName = outputDir + "Day" + dayString + "Out.txt";

            if (!File.Exists(fileName))
            {
                createNewFile(fileName);
                Console.WriteLine("File generated: " + fileName);
            }
            else
            {
                Console.WriteLine("File NOT generated, already exists: " + fileName);
            }
        }

        private static void createNewFile(String fileName)
        {
            File.Create(fileName).Dispose();
        }

        private static void DisplayError(string message, ErrorType typeOfError)
        {
            Console.WriteLine("Error: " + message);

            if (typeOfError == ErrorType.Arguments)
            {
                Console.WriteLine("To display proper usage of arguments, use flag: --help");
            }

            Thread.Sleep(3000);
            System.Environment.Exit(1);
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Here's some help!");
        }

        private static void RunDailySolution(int currentDay)
        {
            Day currentProblem = null;

            switch (currentDay)
            {
                case 1:
                    currentProblem = new Day01();
                    break;
                case 2:
                    currentProblem = new Day02();
                    break;
                default:
                    DisplayError("Day " + currentDay + " not implemented.", ErrorType.NotImplemented);
                    break;
            }

            if (currentProblem == null)
            {
                DisplayError("Day " + currentDay + " not implemented.", ErrorType.NotImplemented);
            }

            currentProblem.Sample = currentProblem.ReadInputSampleFile(currentDay);

            currentProblem.RunSolution();

            currentProblem.WriteOutputSolutionFile(currentDay);
        }
    }
}