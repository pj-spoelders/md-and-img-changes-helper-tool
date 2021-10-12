using System;
using System.Collections.Generic;
using System.IO;

namespace text_and_file_changer
{
    class Program
    {
        static void Main(string[] args)
        {
           
            ChangeFilenamesAndReferences();
        }

        private static void ChangeFilenamesAndReferences()
        {
            string directory = GetDirectory();

            bool doContinue = true;
            //todo: improve/fix this shoddy do while and quit mechanism
            do
            {

                ListImages(directory);
                doContinue = PromptForNamechange(directory);

            } while (doContinue);
        }

        private static string GetDirectory()
        {
            Console.WriteLine("Enter directory path (or leave empty for current directory):");
            var path = Console.ReadLine();
            string directory;
            if (string.IsNullOrEmpty(path))
            {
                directory = Environment.CurrentDirectory;
            }
            else
            {
                directory = Directory.CreateDirectory(path).FullName;
            }

            return directory;
        }

        private static bool PromptForNamechange(string directory)
        {
            string inputFilename;
            Console.WriteLine("enter filename path (enter q to quit):");
            inputFilename = Console.ReadLine();
            if (CallingItQuits(inputFilename))
            {
                return false;
            }
            //TODO: improve the input, make it check if it exists, maybe check the list if they don't use an absolute path

            var imgFile = inputFilename;
            var imgFileName = Path.GetFileName(imgFile);
            var imgFilePath = Path.GetDirectoryName(imgFile) ;
            
            var imgExtension = Path.GetExtension(imgFile);

            Console.WriteLine("enter new filename (enter q to quit):");
            var inputNewFilename = Console.ReadLine();
            if (CallingItQuits(inputNewFilename)) {
                return false;
            }
            var newFileName =  inputNewFilename + imgExtension;
            var newFile = imgFilePath + Path.DirectorySeparatorChar + newFileName;
            ChangeFileNameAndReference(imgFile, imgFileName, newFile,newFileName,directory);

            return true;
        }

        private static bool CallingItQuits(string input)
        {
            if (input == "q" || input == "Q")
            {
                return true;
            } else
            {
                return false;
            }
        }

        private static void ChangeFileNameAndReference(string imgFile, string imgFileName, string newFile,string newFileName,string directory)
        {
            File.Move(imgFile, newFile);

            AlterMdFiles(imgFileName, newFileName, directory);
        }

        private static void AlterMdFiles(string imgFileName, string newFileName, string directory)
        {
            var currDirectory = directory;
            string[] mdFilePaths = Directory.GetFiles(currDirectory, "*.md", SearchOption.AllDirectories);

            foreach (var filename in mdFilePaths)
            {
                var currText = System.IO.File.ReadAllText(filename);
                var alteredText = currText.Replace(imgFileName, newFileName);
                using (StreamWriter writer = System.IO.File.CreateText(filename))
                {
                    writer.Write(alteredText);
                }
            }
        }

        private static void ListImages(string directory)
        {
            Console.WriteLine("Welcome");
            var currDirectory = directory;

            Console.WriteLine($"Looking at directory: { currDirectory }");
            //https://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
            string[] allFiles = Directory.GetFiles(currDirectory, "*.*", SearchOption.AllDirectories);

            List<string> imageFilePaths = GetImageFilePaths(allFiles);

            string[] mdFilePaths = Directory.GetFiles(currDirectory, "*.md", SearchOption.AllDirectories);
            List<string> mdfileContents = ReadMdFileContents(mdFilePaths);

            List<string> foundList = new List<string>();
            List<string> notFoundList = new List<string>();

            CheckIfImagesGetReferenced(imageFilePaths, mdfileContents, foundList, notFoundList);

            PrintFoundAndNotFound(foundList, notFoundList);

        }

        private static List<string> ReadMdFileContents(string[] mdFilePaths)
        {
            List<string> mdfileContents = new List<string>();
            foreach (var filename in mdFilePaths)
            {
                mdfileContents.Add(System.IO.File.ReadAllText(filename));
            }

            return mdfileContents;
        }

        private static void PrintFoundAndNotFound(List<string> foundList, List<string> notFoundList)
        {
            Console.WriteLine("not found:");
            foreach (var notFoundImage in notFoundList)
            {
                Console.WriteLine(notFoundImage);
            }
            Console.WriteLine("found:");
            foreach (var foundImage in foundList)
            {
                Console.WriteLine(foundImage);
            }
        }

        private static void CheckIfImagesGetReferenced(List<string> imageFilePaths, List<string> mdfileContents, List<string> foundList, List<string> notFoundList)
        {
            foreach (var imageFileNamePath in imageFilePaths)
            {
                bool found = false;
                foreach (var content in mdfileContents)
                {
                    string filename = Path.GetFileName(imageFileNamePath);
                    if (content.Contains(filename))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    notFoundList.Add(imageFileNamePath);
                }
                else
                {
                    foundList.Add(imageFileNamePath);
                }
            }
        }

        private static List<string> GetImageFilePaths(string[] allFiles)
        {
            List<string> imageFileNames = new List<string>();

            foreach (string file in allFiles)
            {
                if (file.EndsWith(".png", true, null) || (file.EndsWith(".jpg", true, null)))
                {
                    imageFileNames.Add(file);
                }
            }

            return imageFileNames;
        }
    }
}
