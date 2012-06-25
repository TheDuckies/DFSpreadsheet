using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DFSpreadsheet
{
    internal class Program
    {
        private const string BASEPATH = @"C:\Projects\FitNesseRoot\";
        private const string OptimizePath = @"C:\Projects\Optimize\FitNesseList\";
        private static Dictionary<string, List<string>> sheets;

        private static void Main(string[] args)
        {
            //Feed in root for Fitnesse
            sheets = new Dictionary<string, List<string>>();

            Directory.SetCurrentDirectory(OptimizePath);

            recurse(new DirectoryInfo(BASEPATH));

            foreach (string key in sheets.Keys)
            {
                List<string> writingToFile;

                sheets.TryGetValue(key, out writingToFile);

                var suiteSheet = new StreamWriter(File.Open(key + ".txt", FileMode.OpenOrCreate));

                foreach (string line in writingToFile)
                {
                    suiteSheet.WriteLine(line);
                }

                Thread.Sleep(100);
                suiteSheet.Close();
            }

        }

        private static void recurse(DirectoryInfo newroot)
        {
            if (newroot.Name == ".svn")
                return;

            DirectoryInfo[] subdirectories = newroot.GetDirectories();

            if (newroot.Parent.Name == "FitNesseRoot")
            {
                string sheet = newroot.Name;
                Console.WriteLine(sheet);
                //check if sheet exists
                // if sheet does not exist, create
                // else, add to sheet
            }

            foreach (DirectoryInfo subdirectory in subdirectories)
            {
                recurse(subdirectory);
            }

            
            if (subdirectories.Count() == 1)
            {
                FileInfo[] testCases = newroot.GetFiles("content.txt");
                foreach (FileInfo testCase in testCases)
                {
                    //send off to ConversionOptimizer using

                    if (testCase.DirectoryName.ToUpper().Contains("MACRO") || testCase.DirectoryName.ToUpper().Contains("SETUP") ||testCase.DirectoryName.ToUpper().Contains("TEARDOWN"))
                        return;

                    string[] name = testCase.DirectoryName.Split('\\');

                    int max = name.GetLength(0)- 1;

                    if (name[max].ToUpper().Contains("SETUP") || name[max].ToUpper().Contains("TEARDOWN") || name[max].ToUpper().Contains("SUITE"))
                        return;

                    AppendToSheet(testCase.FullName.Replace(BASEPATH, "").Replace('\\', '.').Replace(".content.txt", ""));
                }
             }
            
        }

        public static void AppendToSheet(string testCase)
        {
            string sheetName = testCase.Split('.')[0];
            
            List<string> currSheet;
            
            if(!sheets.TryGetValue(sheetName, out currSheet))
            {
                currSheet = new List<string>();
                sheets.Add(sheetName, currSheet);

            }

            currSheet.Add(testCase + '\t' + "Not Started");
        }
    }
}