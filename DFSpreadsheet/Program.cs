using System;
using System.IO;
using System.Linq;

namespace DFSpreadsheet
{
    internal class Program
    {
        private const string BASEPATH = @"C:\Projects\FitNesseRoot\";
        private const string OptimizePath = @"C:\Projects\Optimize\FitNesseList\";
        private static DirectoryInfo root;

        private static void Main(string[] args)
        {
            //Feed in root for Fitnesse

            Directory.SetCurrentDirectory(OptimizePath);
            root = new DirectoryInfo(BASEPATH);


            recurse(root);

            DirectoryInfo[] directoryList = root.GetDirectories();
            if (directoryList[0].Parent == root)
            {
                /*
                 * if parent == root
                 * check if this.tostring exists as sheet
                 * if not, create sheet.
                 * 
                 * else 
                 */
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
                    WriteToSheet(testCase.FullName.Replace(BASEPATH, "").Replace('\\', '.').Replace(".content.txt", ""));
                }
            }
        }

        public static void WriteToSheet(string testCase)
        {
            string sheetName = testCase.Split('.')[0];

            var suiteSheet = new StreamWriter(File.Open(sheetName + ".txt", FileMode.Append));

            suiteSheet.WriteLine(testCase + '\t' + "Not Started");
            suiteSheet.Close();
        }
    }
}