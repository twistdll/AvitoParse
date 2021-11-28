using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;


namespace avitoParse
{
    static class InfoSerializer
    {
        private static string _defaultRegionFilePath = "region.txt";
        private static string _defaultListFilePath = "list.txt";

        public static string ReadName()
        {
            return File.Exists(_defaultRegionFilePath) ? File.ReadAllText(_defaultRegionFilePath) : string.Empty;
        }

        public static void WriteName()
        {
            File.Create(_defaultRegionFilePath).Close();
            File.WriteAllText(_defaultRegionFilePath, "");
        }

        public static void DeleteName()
        {
            if (File.Exists(_defaultRegionFilePath))
                File.Delete(_defaultRegionFilePath);
        }

        public static void WriteList(List<string> links)
        {
            File.Create(_defaultListFilePath).Close();

            foreach (string link in links)
            {
                File.AppendAllText(_defaultListFilePath, link + "\n");
            }
        }
    }
}
