using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

class Program
{
    static async Task Main(string[] args)
    {
        string tenantId = "<your tenant id>";
        string clientId = "<your client id>";
        string clientSecret = "<your client secret>";
        string workspaceId = "<your workspace id>";
        string query = "your query goes here";
        
        /*
        n this code, you need to replace <your tenant id>, <your client id>, <your client secret>, and <your workspace id> 
        with your Azure AD tenant ID, client ID, client secret, and workspace ID, respectively. You also need to 
        replace "your query goes here" with the actual query you want to run. */
        
        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        var client = new LogsQueryClient(credential);

        Azure.Response<LogsQueryResult> response = await client.QueryWorkspaceAsync(
        workspaceId,
        query,
        new QueryTimeRange(TimeSpan.FromDays(15)));

        LogsTable table = response.Value.Table;

        foreach (var row in table.Rows)
        {
            Console.WriteLine(row["OperationName"] + " | " + row["ResourceGroup"] + " | " + row["CallerIPAddress"] + " | " + (row["requestUri_s"].ToString().Split("/")[4]));
        }
    }
}
