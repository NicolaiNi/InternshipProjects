using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderStructure {
    class Program {
        
        public static void ListDirectory(List<Folder> folders, string path, Dictionary<string, List<FileInfo>> dic ) {

            var stack = new Stack<Folder>();
            var rootDirectory = new DirectoryInfo(path);
            var folder = new Folder(rootDirectory.FullName, rootDirectory);
            folder.Init();
            folders.Add(folder);
            stack.Push(folder);
            string subfolder1 = "\\" + folder.SubfoldersInfo[0].Name; 
            string subfolder2 = "\\" + folder.SubfoldersInfo[1].Name; 
            //// files in the root folder are not added to dictionary 

            while (stack.Count > 0) {
                Folder currentFolder = stack.Pop();
                foreach (var directory in currentFolder.SubfoldersInfo) {
                    var childFolder = new Folder(directory);   
                    childFolder.Init();
                    folders.Add(childFolder);
                    currentFolder.Subfolders.Add(childFolder);
                    stack.Push(childFolder);
                    
                    foreach (FileInfo fileInfo in childFolder.Files) {
                        string identifier = "";
                        if (fileInfo.FullName.Contains(subfolder1)) {
                            var splitPath = fileInfo.FullName.Split('\\');
                            //get parent of the folder
                            identifier = splitPath[splitPath.Length - 2] + "\\" + fileInfo.Name;
                        }
                        if (fileInfo.FullName.Contains(subfolder2)) {
                            var splitPath = fileInfo.FullName.Split('\\');
                            //get parent of the folder
                            identifier = splitPath[splitPath.Length - 2] + "\\" + fileInfo.Name;
                        }
                        if (dic.TryGetValue(identifier, out List<FileInfo> listFileInfo)) {
                            listFileInfo.Add(fileInfo);
                        } else {
                            dic.Add(identifier, new List<FileInfo> { fileInfo });
                        }
                    }
                }
            }
        }

        public void WriteFolderStructure(List<Folder> folders) {
            foreach (var folder in folders) {
                var tab = new String('\t', folder.CurrentDepth);
                Console.WriteLine(tab + folder.Name);
                foreach (var file in folder.Files) {
                    Console.WriteLine(tab + "\t~ " + file.Name);
                }
                var subfolders = folder.Subfolders.ToList();
                WriteFolderStructure(subfolders);
            }
        }

        static void Main(string[] args) {
            // root path should included two folders to compare
            string rootPath = @"path\to\rootPath\with\two\folders\to\compare"; 

            List<Folder> folders = new List<Folder>();
            Dictionary<string, List<FileInfo>> pathToFilesDictionary = new Dictionary<string, List<FileInfo>>();

            ListDirectory(folders, rootPath, pathToFilesDictionary);
            
            //some examples which could be done with this folderStructure implementation
            //find special file or file extensions with the same parent folder name in the folders structure, 
            //independant of depth in folder structure
            //examples given as ".txt"; file types/names could be added or deleted
            var keysOfInterest = pathToFilesDictionary.Where(kvp => kvp.Value.Count() == 2).Where(kvp => kvp.Key.Contains(".txt")).Select(i => i.Key);
            Console.WriteLine("The following files have the file extension .txt and are found in both folder structures");
            foreach (var key in keysOfInterest) {      
                var value = pathToFilesDictionary[key];
                Console.WriteLine(value[0].FullName);
                Console.WriteLine(value[1].FullName);
            }
            Console.WriteLine();

            //get number of total files counting from root directory
            var totalFiles = folders.SelectMany(f => f.Files).Count();
            Console.WriteLine("total files:\t" + totalFiles);

            Console.WriteLine();

            //get folder structure printed to console
            Program program = new Program();
            List<Folder> onlyRootFolder = new List<Folder>();
            onlyRootFolder.Add(folders.First());
            program.WriteFolderStructure(onlyRootFolder);

            // let console stay open
            Console.ReadKey();
        }
    }
}
