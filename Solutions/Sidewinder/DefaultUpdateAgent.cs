using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class DefaultUpdateAgent : IUpdateAgent
    {
        public const string SidewinderCommandFile = "_sidewinder.xml";

        protected UpdateConfig myConfig;
        protected IBackupAgent myBackupAgent;        

        public DefaultUpdateAgent()
        {
            myBackupAgent = new DefaultBackupAgent();
        }

        public DefaultUpdateAgent(IBackupAgent backupAgent)
        {
            myBackupAgent = backupAgent;
        }

        public IUpdateAgent Initialise(UpdateConfig config)
        {
            myConfig = config;
            return this;
        }

        public bool Update()
        {
            SidewinderCommands commands;

            if (GetCommands(out commands))
            {
                DistributeFilesAfterDownload(commands);
            }
            else
            {
                // no command file - just check for update
                var current = Assembly.GetEntryAssembly().GetName().Version;

                Console.WriteLine("Checking for updates to v{0}...", current);
                var repo = PackageRepositoryFactory.Default.CreateRepository(myConfig.NuGetFeedUrl);
                var package = repo.FindPackage(myConfig.TargetPackage);

                if (package.Version <= current)
                {
                    Console.WriteLine("\tNo update available...running the latest version!");
                    return false;
                }

                Console.WriteLine("\tUpdate v{0} is available...starting update process", package.Version);
                GetPackageContent(package);
                Backup();                
                WriteUpdateCommandFile();                    
                LaunchApplicationForUpdate();
                return true;
            }

            return false;
        }

        private void DistributeFilesAfterDownload(SidewinderCommands commands)
        {
            if (commands.DistributeFiles == null)
                return;

            var targetFolder = Path.GetDirectoryName(commands.DistributeFiles.TargetProcess);
        }

        protected bool GetCommands(out SidewinderCommands commands)
        {
            commands = null;
            var commandFile = Fluent.IO.Path.Current.Combine(SidewinderCommandFile).FullPath;

            if (!File.Exists(commandFile))
                return false;
            commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
            return true;
        }

        protected void LaunchApplicationForUpdate()
        {
            var processFilename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            var appUpdateFilename = Fluent.IO.Path.Get(myConfig.DownloadFolder).Combine(processFilename).FullPath;
            Console.WriteLine("\tLaunching updated application @{0}...", appUpdateFilename);

            if (!File.Exists(appUpdateFilename))
            {
                Console.WriteLine("\t\tApp update does not exist...terminating update process :(");
                return;
            }
            
            var app = Process.Start(new ProcessStartInfo
                              {
                                  FileName = appUpdateFilename
                              });
        }

        protected void WriteUpdateCommandFile()
        {
            Console.WriteLine("\tWriting command file...");
            var commandFile = Fluent.IO.Path.Get(myConfig.DownloadFolder).Combine(SidewinderCommandFile).FullPath;
            var command = new SidewinderCommands
                              {
                                  DistributeFiles = new DistributeFiles
                                                        {
                                                            TargetProcess = Process.GetCurrentProcess().MainModule.FileName
                                                        }
                              };
            SerialisationHelper<SidewinderCommands>.DataContractSerialize(commandFile, command);
        }


        protected virtual void Backup()
        {
            if (!myConfig.Backup)
                return;    

            Console.WriteLine("\tBacking up existing application @{0} to {1}", myConfig.InstallFolder,
                myConfig.BackupFolder);
            myBackupAgent.Backup(new BackupConfig
                                     {
                                         BackupTo = myConfig.BackupFolder,
                                         DirectoryToBackup = myConfig.InstallFolder,
                                         FoldersToIgnore = new List<string>
                                                               {
                                                                   myConfig.DownloadFolder
                                                               }
                                     });
        }

        private void GetPackageContent(IPackage package)
        {
            Console.WriteLine("\t\tDownloading package content to: {0}...", myConfig.DownloadFolder);
            Fluent.IO.Path.CreateDirectory(myConfig.DownloadFolder);
            var files = package.GetFiles();

            files.ToList().ForEach(file =>
                              {
                                  Console.WriteLine("\t\tDownloading file: {0}...", file.Path);
                                  DownloadFile(file);
                              });
        }

        protected void DownloadFile(IPackageFile file)
        {
            using (var stream = file.GetStream())
            {
                var filename = Path.GetFileName(file.Path);
                var folder = Fluent.IO.Path.Get(myConfig.DownloadFolder).Combine(file.Path).Parent().FullPath;
                Directory.CreateDirectory(folder);

                using (var destination = File.Create(Path.Combine(folder, filename)))
                    stream.CopyTo(destination);
            }
        }
    }
}