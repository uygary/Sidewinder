using System;
using System.IO;
using System.Linq;

namespace Sidewinder
{
    public class DirectoryWalker
    {
        private string myStartDirectory;
        private Func<DirectoryInfo, bool> myDirectoryHandler;
        private Action<FileInfo> myFileHandler;

        public static DirectoryWalker StartAt(string path)
        {
            return new DirectoryWalker
                       {
                           myStartDirectory = path
                       };
        }

        public DirectoryWalker OnDirectory(Func<DirectoryInfo, bool> action)
        {
            myDirectoryHandler = action;
            return this;
        }

        public DirectoryWalker OnFile(Action<FileInfo> action)
        {
            myFileHandler = action;
            return this;
        }

        public virtual void Walk()
        {
            Walk(new DirectoryInfo(myStartDirectory));

        }

        protected virtual void Walk(DirectoryInfo directory)
        {
            var stop = false;

            if (myDirectoryHandler != null)
                stop = !myDirectoryHandler(directory);

            if (stop)
                return;

            var files = directory.EnumerateFiles();
            if (myFileHandler != null)
            {
                files.ToList().ForEach(file => myFileHandler(file));
            }

            var dirs = directory.EnumerateDirectories();
            dirs.ToList().ForEach(Walk);
        }
    }
}