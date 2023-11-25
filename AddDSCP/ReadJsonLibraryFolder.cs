using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AddDSCP
{
    internal class ReadJsonLibraryFolder
    {
        public class Read
        {
            public static HashSet<string> gamePaths = new HashSet<string>();
            public static void ReadLibraryFolder(string fileName)
            {
                try
                {
                    string[] lines = File.ReadAllLines(fileName);

                    string[] paths = lines
                        .Where(line => line.Contains("path"))
                        .Select(line => line.Replace("\"", "").Replace(@"\\", @"\").Replace("path", "").TrimStart())
                        .ToArray();

                    foreach (string line in paths)
                    {
                        GetFolderNames(Path.Combine(line, "steamapps", "common"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            private static void GetFolderNames(string path)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        string[] folders = Directory.GetDirectories(path);

                        foreach (var item in folders)
                        {

                            string folderName = Path.GetFileName(item);

                            foreach (var pair in ReadJsonAppInfo.Read.gameInstalldir)
                            {
                                //if installdir from ReadJsonAppInfo is equal to folderName
                                if (pair[0] == folderName)
                                {
                                    gamePaths.Add(path + @"\" + folderName + @"\" + pair[1]);
                                }
                            }

                        }

                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}

