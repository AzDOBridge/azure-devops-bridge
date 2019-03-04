using System;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.Common;

namespace AzDoBridge.Clients
{
    public class AzureDevOpsClient
    {
        protected readonly string Username;
        protected readonly string PersonalAccessToken;
        Lazy<TfsTeamProjectCollection> _tfsTeamProjectCollection;
        public AzureDevOpsClient(Uri hostname, string username, string personalAccessToken)
        {
            Username = username;
            PersonalAccessToken = personalAccessToken;
            _tfsTeamProjectCollection = new Lazy<TfsTeamProjectCollection>(
                () => {

                    return TfsTeamProjectCollectionFactory.GetTeamProjectCollection(hostname);}
                );
        }
        public TfsTeamProjectCollection MyTfsTeamProjectCollection
        {
            get
            {
                return _tfsTeamProjectCollection.Value;
            }
        }
        public async Task<WorkItemStore> FetchWorkItemStoreAsync()
        {
            return await Task.Run(() =>
            {                
                MyTfsTeamProjectCollection.ClientCredentials = new VssCredentials(new VssBasicCredential(Username, PersonalAccessToken));
                MyTfsTeamProjectCollection.Authenticate();
                return MyTfsTeamProjectCollection.GetService<WorkItemStore>();

            }).ConfigureAwait(false);
        }
        public WorkItemStore FetchWorkItemStore()
        {
            MyTfsTeamProjectCollection.ClientCredentials = new VssCredentials(new VssBasicCredential(Username, PersonalAccessToken));
            MyTfsTeamProjectCollection.Authenticate();
            return MyTfsTeamProjectCollection.GetService<WorkItemStore>();
       
        }
    }
}
