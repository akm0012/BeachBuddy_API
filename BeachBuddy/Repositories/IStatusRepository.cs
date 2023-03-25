using System.Threading.Tasks;
using BeachBuddy.Models;

namespace BeachBuddy.Repositories
{
    /**
     * Todo: Not sure where this really belongs, so making it a Repo for now.
     *
     * This is used to check the status of the Server, Database, and it's Services. 
     */
    public interface IStatusRepository
    {
        Task<SystemStatus> GetSystemStatus();
    }
}