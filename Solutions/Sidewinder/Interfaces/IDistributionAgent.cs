
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Interfaces
{
    public interface IDistributionAgent
    {
        IDistributionAgent Initialise(DistributorConfig config);
        bool Execute();
    }
}