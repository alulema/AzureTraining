using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Alulema.Function.Models;

namespace Alulema.Function
{
    public static class PostTransactions
    {
        [FunctionName("PostTransactions")]
        public static HttpResponseMessage Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "transactions")] HttpRequestMessage req,
            [CosmosDB(
                "AzureServerlessArchitectureCourse",
                "Transactions",
                ConnectionStringSetting = "CosmosDBConnectionString")] out Transaction transaction,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            transaction = JsonConvert.DeserializeObject<Transaction>(message);

            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of {transaction.Amount}");
        }
    }
}
