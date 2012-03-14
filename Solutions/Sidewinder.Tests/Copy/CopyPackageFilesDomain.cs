using System;
using System.Collections.Generic;
using Fluent.IO;
using FluentAssertions;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Tests.Copy
{
    public partial class CopyPackageFilesSpecs
    {
        private ConflictResolutionTypes myResolutionActionType;
        private string myInstallationFolder;
        private string myUpdateFolder;

        private void The_DirectoryContainsThePackageFiles(string folder)
        {
            myUpdateFolder = folder;
        }

        private void TheInstallLocation_IsUsed(string folder)
        {
            myInstallationFolder = folder;
        }

        private void TheInstallationLocationIsCleaned()
        {
            Path.Get(myInstallationFolder).Delete(true);
        }

        private void TheFilesAreCopied()
        {
            var context = new DistributorContext
                              {
                                  Config = new DistributorConfig
                                               {
                                                   Command = new DistributeFiles
                                                                 {
                                                                     ConflictResolution = myResolutionActionType,
                                                                     InstallFolder = myInstallationFolder,
                                                                     TargetFrameworkVersion = new Version(4,0),
                                                                     DownloadFolder = myUpdateFolder,
                                                                     Updates = new List<UpdatedPackage>
                                                                                   {
                                                                                       new UpdatedPackage
                                                                                           {
                                                                                               Target =
                                                                                                   new TargetPackage
                                                                                                       {
                                                                                                           Name =
                                                                                                               "TestPkg"
                                                                                                       }
                                                                                           }
                                                                                   }
                                                                 }
                                               }
                              };

            myStep.EntryConditions(context);
            myStep.Execute(context);
            myStep.ExitConditions(context);
        }

        private void TheInstallationFolderContainsTheContentFiles()
        {
            // content
            Path.Get(myInstallationFolder).Exists.Should().BeTrue();            
            Path.Get(myInstallationFolder, "Content_Sub1").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub1", "content_sub1.txt").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, @"Content_Sub1\Content_Sub1.1").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, @"Content_Sub1\Content_Sub1.1", "content_sub1.1.txt").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub2").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub2", "content_sub2.txt").Exists.Should().BeTrue();

            Path.Get(myInstallationFolder, "Content").Exists.Should().BeFalse();
        }

        private void TheInstallationToolsFolderIsCorrect()
        {
            Path.Get(myInstallationFolder, "Tools_Sub1").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub1", "tools_sub1.txt").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub2").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub2", "tools_sub2.txt").Exists.Should().BeTrue();
        }

        private void TheInstallationBinariesAreCorrect()
        {
            Path.Get(myInstallationFolder, "net40.txt").Exists.Should().BeTrue();

            Path.Get(myInstallationFolder, "lib").Exists.Should().BeFalse();
            Path.Get(myInstallationFolder, "net35").Exists.Should().BeFalse();
            Path.Get(myInstallationFolder, "net40").Exists.Should().BeFalse();
        }

        private void TheOverwriteResolutionActionIsUsed()
        {
            myResolutionActionType = ConflictResolutionTypes.Overwrite;
        }

        private void TheManualResolutionActionIsUsed()
        {
            myResolutionActionType = ConflictResolutionTypes.Manual;
        }

        private void TheConflictingContentFilesAreCopiedToTheInstallationLocation()
        {
            var conflictRoot = Path.Get(SmartLocation.GetLocation(@"testdata\update\conflict\content"));
            Path.Get(conflictRoot.FullPath).AllFiles().Copy(source =>
                                                     {
                                                         var rel = source.MakeRelativeTo(conflictRoot);
                                                         var dest = Path.Get(myInstallationFolder,
                                                                             rel.DirectoryName,
                                                                             rel.FileName);
                                                         return dest;
                                                     });
        }

        private void AllTheContentFilesHaveBeenUpdated()
        {
            AllFilesThatStartWith_HaveContent_("content", "updated content");
        }

        private void AllTheContentFilesHaveNotBeenUpdated()
        {
            AllFilesThatStartWith_HaveContent_("content", "original content");
        }

        private void AllFilesThatStartWith_HaveContent_(string start, string content)
        {
            Path.Get(myInstallationFolder).Files(f => f.FileName.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                .ForEach(path => path.Read().Should().Be(content));
        }
    }
}