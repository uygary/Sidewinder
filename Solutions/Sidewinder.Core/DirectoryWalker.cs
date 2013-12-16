using System;
using System.IO;
using System.Linq;

namespace Sidewinder.Core
{
    public class DirectoryWalker<T>
    {
        private string myStartDirectory;
        private T myContext;
        private Func<DirectoryInfo, T, bool> myDirectoryHandler;
        private Action<FileInfo, T> myFileHandler;

        private DirectoryWalker()
        {
            myDirectoryHandler = DefaultDirectoryHandler;
        }

        public static DirectoryWalker<T> StartAt(string path)
        {
            var walker = New();
            walker.myStartDirectory = path;
            return walker;
        }

        public static DirectoryWalker<T> New()
        {
            return new DirectoryWalker<T>();
        }

        private static bool DefaultDirectoryHandler(DirectoryInfo arg, T context)
        {
            return true;
        }

        public DirectoryWalker<T> OnDirectory(Func<DirectoryInfo, T, bool> action)
        {
            myDirectoryHandler = action;
            return this;
        }

        public DirectoryWalker<T> OnFile(Action<FileInfo, T> action)
        {
            myFileHandler = action;
            return this;
        }

        public virtual void Walk()
        {
            if (string.IsNullOrWhiteSpace(myStartDirectory))
                throw new InvalidOperationException("Start directory not set, unable to commence walk!");
            Walk(new DirectoryInfo(myStartDirectory));
        }

        public virtual void Walk(string directory, T context)
        {
            myStartDirectory = directory;
            myContext = context;
            Walk();
        }

        protected virtual void Walk(DirectoryInfo directory)
        {
            var stop = false;

            if (myDirectoryHandler != null)
                stop = !myDirectoryHandler(directory, myContext);

            if (stop)
                return;

            var files = directory.EnumerateFiles();
            if (myFileHandler != null)
            {
                files.ToList().ForEach(file => myFileHandler(file, myContext));
            }

            var dirs = directory.EnumerateDirectories();
            dirs.ToList().ForEach(Walk);
        }
    }
}