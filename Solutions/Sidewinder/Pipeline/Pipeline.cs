
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sidewinder.Interfaces;

namespace Sidewinder.Pipeline
{
    public class Pipeline<T> where T: class
    {
        private List<IPipelineStep<T>> mySteps;

        public static Pipeline<T> Run(IPipelineStep<T> step)
        {
            return new Pipeline<T>
                       {
                           mySteps = new List<IPipelineStep<T>> {step}
                       };
        }

        public Pipeline<T> Then(IPipelineStep<T> step)
        {
            mySteps.Add(step);
            return this;
        }

        public bool Execute(T context)
        {
            var timer = new Stopwatch();

            timer.Start();
            foreach (var step in mySteps)
            {
                Console.WriteLine("Executing step: {0}", step.GetType().Name);
                step.EntryConditions(context);
                if (!step.Execute(context))
                {
                    Console.WriteLine("**WARNING** Pipeline aborting!");
                    return false;
                }
                step.ExitConditions(context);
            }

            timer.Stop();
            Console.WriteLine("Pipeline<{0}> completed in {1}s", typeof(T).Name, timer.Elapsed.TotalSeconds);

            return true;
        }
    }
}