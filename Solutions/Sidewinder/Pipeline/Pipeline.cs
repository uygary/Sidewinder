
using System.Collections.Generic;
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
            foreach (var step in mySteps)
            {
                step.EntryConditions(context);
                if (!step.Execute(context))
                    return false;
                step.ExitConditions(context);
            }

            return true;
        }
    }
}