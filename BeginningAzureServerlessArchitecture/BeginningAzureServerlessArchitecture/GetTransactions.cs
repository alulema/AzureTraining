using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Alulema.Function.Models;

namespace Alulema.Function
{
    public static class GetTransactions
    {
        [FunctionName("GetTransactions")]
        public static List<Transaction> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = null)] HttpRequest req,
            [CosmosDB(
                ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            // Using the Cosmos DB SDK directly. Setting MaxItemCount to -1 means return all results
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            
            // This is using the LINQ API. It's possible to do .Where(x => x...) on the end of here.
            IQueryable<Transaction> transactionQuery = client.CreateDocumentQuery<Transaction>(
                UriFactory.CreateDocumentCollectionUri(
                    "AzureServerlessArchitectureCourse", "Transactions"),
                queryOptions);

            // Setup the response object.
            var transactions = new List<Transaction>();
            
            // Now execute
            foreach (Transaction transaction in transactionQuery)
            {
                transactions.Add(transaction);
            }
            
            // And return result
            return transactions;
        }
    }
}