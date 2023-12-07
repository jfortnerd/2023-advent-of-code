using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jfortnerd.Aod2k23.Dailies
{
    abstract class Day
    {
        private const int BufferSize = 1024;

        protected String input = "";
        protected String output = "";

        public String Sample
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public String Solution
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        public abstract void RunSolution();

        public void AppendSolution(String output)
        {
            if ((Solution.Equals("")) || (Solution == null)) 
            {
                Solution = output;
            }
            else
            {
                Solution = Solution + "\n" + output;
            }
        }

        private String GetInputFileName(int dayNum)
        {
            if (dayNum >= 10)
            {
                return "Day" + dayNum + "In.txt";
            }
            else
            {
                return "Day0" + dayNum + "In.txt";
            }

        }
        private String GetOutputFileName(int dayNum)
        {
            if (dayNum >= 10)
            {
                return "Day" + dayNum + "Out.txt";
            }
            else
            {
                return "Day0" + dayNum + "Out.txt";
            }

        }

        public String ReadInputSampleFile(int dayNum)
        {
            String fileName = GetInputFileName(dayNum);
            String currentLine = null;
            String filePathName = GetFullPath(fileName, "input");
            StreamReader sr = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                // Pass file path and name to StreamReader constructor
                sr = new StreamReader(filePathName);

                // Read the first line of text
                currentLine = sr.ReadLine();
                
                // Continue to read until end of file 
                while (currentLine != null)
                {
                    // build string
                    sb.AppendLine(currentLine);
                    currentLine = sr.ReadLine();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Pass file path and name to StreamReader constructor

                Thread.Sleep(3000);
                System.Environment.Exit(1);
            }
            finally
            {
                sr.Close();           
            }

            return sb.ToString();
        }

        private String GetFullPath(String filename, String subDir)
        {
            String currDir = "D:\\source\\2023-advent-of-code\\csharp\\" + subDir + "\\";

            return currDir + filename;
        }


        public void WriteOutputSolutionFile(int dayNum)
        {
            String outputDir = "D:\\source\\2023-advent-of-code\\csharp\\output\\";
            String fileName = GetOutputFileName(dayNum);
            String fullPath = outputDir + fileName;

            Console.WriteLine(Solution);

            File.WriteAllBytes(fullPath, Encoding.UTF8.GetBytes(Solution));
        }

    }
}
