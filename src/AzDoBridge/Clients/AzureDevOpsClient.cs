using System;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.Common;

namespace AzDoBridge.Clients
{
    public class AzureDevOpsClient
    {
        public AzureDevOpsClient(Uri hostname, string username, string personalAccessToken)
        {
            TfsTeamProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(hostname);
            TfsTeamProjectCollection.ClientCredentials = new VssCredentials(new VssBasicCredential(username, personalAccessToken));
        }

        public TfsTeamProjectCollection TfsTeamProjectCollection { get; set; }

        public async Task<WorkItemStore> FetchWorkItemStoreAsync()
        {
            return await Task.Run(() =>
            {
                TfsTeamProjectCollection.Authenticate();

                return TfsTeamProjectCollection.GetService<WorkItemStore>();
            }).ConfigureAwait(false);

        }
    }
}
