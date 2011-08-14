using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class Initialise : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            Console.WriteLine("Cleaning download folder {0}", context.Config.DownloadFolder);
            Path.Get(context.Config.DownloadFolder).Delete(true);
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}