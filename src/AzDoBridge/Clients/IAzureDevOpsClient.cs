using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Clients
{
    public interface IAzureDevOpsClient
    {
        TfsTeamProjectCollection MyTfsTeamProjectCollection { get; }

        WorkItemStore FetchWorkItemStore();
        Task<WorkItemStore> FetchWorkItemStoreAsync();
    }
}