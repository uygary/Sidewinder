
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Interfaces
{
    public interface IUpdateAgent
    {
        IUpdateAgent Initialise(UpdateConfig config);
        bool Update();
    }
}