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
        private ConflictResolutionTypes _resolutionActionType;
        private string _installationFolder;
        private string _updateFolder;

        private void The_DirectoryContainsThePackageFiles(string folder)
        {
            _updateFolder = folder;
        }

        private void TheInstallLocation_IsUsed(string folder)
        {
            _installationFolder = folder;
        }

        private void TheInstallationLocationIsCleaned()
        {
            Path.Get(_installationFolder).Delete(true);
        }

        private void TheFilesAreCopied()
        {
            var command = new DistributeFiles
                              {
                                  ConflictResolution = _resolutionActionType,
                                  InstallFolder = _installationFolder,
                                  TargetFrameworkVersion = new Version(4, 0),
                                  DownloadFolder = _updateFolder,
                                  Updates = new List<UpdatedPackage>
                                                {
                                                    new UpdatedPackage
                                                        {
                                                            Target = new TargetPackage
                                                                    {
                                                                        Name = "TestPkg"
                                                                    }
                                                        }
                                                }
                              };

            var context = new DistributorContext
                              {
                                  Config = new DistributorConfig
                                               {
                                                   Command = command
                                               }
                              };

            _step.EntryConditions(context);
            _step.Execute(context);
            _step.ExitConditions(context);
        }

        private void TheInstallationFolderContainsTheContentFiles()
        {
            // content
            Path.Get(_installationFolder).Exists.Should().BeTrue();            
            Path.Get(_installationFolder, "Content_Sub1").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Content_Sub1", "content_sub1.txt").Exists.Should().BeTrue();
            Path.Get(_installationFolder, @"Content_Sub1\Content_Sub1.1").Exists.Should().BeTrue();
            Path.Get(_installationFolder, @"Content_Sub1\Content_Sub1.1", "content_sub1.1.txt").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Content_Sub2").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Content_Sub2", "content_sub2.txt").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Content").Exists.Should().BeFalse();
        }

        private void TheInstallationToolsFolderIsCorrect()
        {
            Path.Get(_installationFolder, "Tools_Sub1").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Tools_Sub1", "tools_sub1.txt").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Tools_Sub2").Exists.Should().BeTrue();
            Path.Get(_installationFolder, "Tools_Sub2", "tools_sub2.txt").Exists.Should().BeTrue();
        }

        private void TheInstallationBinariesAreCorrect()
        {
            Path.Get(_installationFolder, "net40.txt").Exists.Should().BeTrue();

            Path.Get(_installationFolder, "lib").Exists.Should().BeFalse();
            Path.Get(_installationFolder, "net35").Exists.Should().BeFalse();
            Path.Get(_installationFolder, "net40").Exists.Should().BeFalse();
        }

        private void TheOverwriteResolutionActionIsUsed()
        {
            _resolutionActionType = ConflictResolutionTypes.Overwrite;
        }

        private void TheManualResolutionActionIsUsed()
        {
            _resolutionActionType = ConflictResolutionTypes.Manual;
        }

        private void TheConflictingContentFilesAreCopiedToTheInstallationLocation()
        {
            var conflictRoot = Path.Get(SmartLocation.GetLocation(@"testdata\update\conflict\content"));
            Path.Get(conflictRoot.FullPath).AllFiles().Copy(source =>
                                                     {
                                                         var rel = source.MakeRelativeTo(conflictRoot);
                                                         var dest = Path.Get(_installationFolder,
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
            Path.Get(_installationFolder).Files(f => f.FileName.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                .ForEach(path => path.Read().Should().Be(content));
        }
    }
}