using MetricsManager.DAL.Models;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IAgentsRepository : IRepository<AgentModel>
    {
        IList<AgentModel> GetAllActive();
    }
}
