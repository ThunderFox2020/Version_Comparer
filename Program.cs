using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Version_Comparer
{
    public class Program
    {
        public static void Main()
        {
            Input(out string oldVersionPath, out string newVersionPath, out string[] extensions);

            DirectoryInfo oldVersionDirectory = new DirectoryInfo(oldVersionPath);
            DirectoryInfo newVersionDirectory = new DirectoryInfo(newVersionPath);

            var oldVersionDirectoriesQuery = from directory in oldVersionDirectory.GetDirectories("*", SearchOption.AllDirectories)
                                             select directory.FullName.Replace(oldVersionPath, "root");
            var newVersionDirectoriesQuery = from directory in newVersionDirectory.GetDirectories("*", SearchOption.AllDirectories)
                                             select directory.FullName.Replace(newVersionPath, "root");

            var oldVersionFilesQuery = from file in oldVersionDirectory.GetFiles("*", SearchOption.AllDirectories)
                                       where extensions.Contains(file.Extension) || (extensions.Length == 1 && extensions[0] == "")
                                       select file.FullName.Replace(oldVersionPath, "root");
            var newVersionFilesQuery = from file in newVersionDirectory.GetFiles("*", SearchOption.AllDirectories)
                                       where extensions.Contains(file.Extension) || (extensions.Length == 1 && extensions[0] == "")
                                       select file.FullName.Replace(newVersionPath, "root");

            List<string> deletedDirectories = (oldVersionDirectoriesQuery.Except(newVersionDirectoriesQuery).OrderBy(x => x)).ToList();
            List<string> createdDirectories = (newVersionDirectoriesQuery.Except(oldVersionDirectoriesQuery).OrderBy(x => x)).ToList();

            List<string> deletedFiles = (oldVersionFilesQuery.Except(newVersionFilesQuery).OrderBy(x => Path.GetDirectoryName(x)).ThenBy(x => Path.GetExtension(x)).ThenBy(x => Path.GetFileNameWithoutExtension(x))).ToList();
            List<string> createdFiles = (newVersionFilesQuery.Except(oldVersionFilesQuery).OrderBy(x => Path.GetDirectoryName(x)).ThenBy(x => Path.GetExtension(x)).ThenBy(x => Path.GetFileNameWithoutExtension(x))).ToList();

            Output(deletedDirectories, createdDirectories, deletedFiles, createdFiles);
        }
        
        private static void Input(out string oldVersionPath, out string newVersionPath, out string[] extensions)
        {
            Console.WriteLine("==================================================");

            Console.WriteLine("Путь к старой версии: ");
            oldVersionPath = Console.ReadLine()!;

            Console.WriteLine();

            Console.WriteLine("Путь к новой версии: ");
            newVersionPath = Console.ReadLine()!;

            Console.WriteLine();

            Console.WriteLine("Расширения (.ext1 .ext2 .extN ...): ");
            extensions = Console.ReadLine()!.Split(' ');
        }
        private static void Output(List<string> deletedDirectories, List<string> createdDirectories, List<string> deletedFiles, List<string> createdFiles)
        {
            Console.WriteLine("==================================================");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("==================================================");

            Console.WriteLine("Директории, удаленные в новой версии:");
            foreach (var directory in deletedDirectories) Console.WriteLine("- " + directory);

            Console.WriteLine();

            Console.WriteLine("Директории, созданные в новой версии:");
            foreach (var directory in createdDirectories) Console.WriteLine("- " + directory);

            Console.WriteLine("==================================================");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("==================================================");

            Console.WriteLine("Файлы, удаленные в новой версии:");
            foreach (var file in deletedFiles) Console.WriteLine("- " + file);

            Console.WriteLine();

            Console.WriteLine("Файлы, созданные в новой версии:");
            foreach (var file in createdFiles) Console.WriteLine("- " + file);

            Console.WriteLine("==================================================");

            Console.ReadLine();
        }
    }
}