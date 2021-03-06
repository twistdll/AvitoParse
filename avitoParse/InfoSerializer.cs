using System.Collections.Generic;
using System.IO;

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

        public static void WriteName(string name)
        {
            File.Create(_defaultRegionFilePath).Close();
            File.WriteAllText(_defaultRegionFilePath, name.Replace(name[0], char.ToUpper(name[0])));
        }

        public static void DeleteName()
        {
            if (File.Exists(_defaultRegionFilePath))
                File.Delete(_defaultRegionFilePath);
        }

        public static void CreateFile()
        {
            File.Create(_defaultListFilePath).Close();
        }

        public static void WritePage(List<string> list, uint pageNumber)
        {
            if (File.Exists(_defaultListFilePath))
            {
                File.AppendAllText(_defaultListFilePath, "Страница: " + pageNumber + "\n");

                foreach (var element in list)
                {
                    File.AppendAllText(_defaultListFilePath, element + "\n");
                }

                File.AppendAllText(_defaultListFilePath, "\n");
            }
        }
    }
}
