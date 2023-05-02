using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

class Program
{
    static async Task Main(string[] args)
    {
        string tenantId = "";
        string clientId = "";
        string clientSecret = "";
        string workspaceId = "";
        string query = "AzureDiagnostics | where ResourceProvider =~ 'MICROSOFT.KEYVAULT'|where OperationName =='SecretGet' | where identity_claim_scp_s == 'user_impersonation' | where httpStatusCode_d == 200 ";

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
