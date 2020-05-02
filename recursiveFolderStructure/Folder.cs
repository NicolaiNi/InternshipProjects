using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderStructure {
    public class Folder {
        public static string RootPath { get; set; }
        private static int _depth = 0;
        public int TotalDepth { get { return _depth; } } 
        public int CurrentDepth { get; set; }
        public DirectoryInfo DirInfo { get; set; }
        public string Name { get; set; }
        public DirectoryInfo[] SubfoldersInfo { get; set; }
        public FileInfo[] Files { get; set; }
        public List<Folder> Subfolders = new List<Folder>();

        public Folder(string rootPath, DirectoryInfo dirInfo) {
            RootPath = rootPath;
            this.DirInfo = dirInfo;
            this.CurrentDepth = _depth;
        }

        public Folder(DirectoryInfo dirInfo) {
            this.DirInfo = dirInfo;
        }

        public void Init() {
            SetName();
            GetSubfolders();
            GetFiles();
            SetDepth();
        }

        private void SetName() {
            this.Name = DirInfo.Name; 
        }
        private void GetSubfolders() {
            this.SubfoldersInfo = DirInfo.GetDirectories();
        }
        private void GetFiles() {
            this.Files = DirInfo.GetFiles();
        }
        private void SetDepth() {
            var lengthRoot = RootPath.ToString().Split('\\').Length;
            var lengthCurrent = DirInfo.FullName.ToString().Split('\\').Length;
            var relDepth = lengthCurrent - lengthRoot;
            if(DirInfo.FullName.ToString() != RootPath) {
                this.CurrentDepth = relDepth;
            }
            if(relDepth > _depth) {
                _depth = relDepth;
            }
        }
    }
}
