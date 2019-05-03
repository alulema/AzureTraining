using Alulema.Function.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace Alulema.Function
{
    public static class PostTransactions
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey =
            Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

        private static TelemetryClient telemetry = new TelemetryClient
        {
            InstrumentationKey = key
        };

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

            try
            {
                transaction = JsonConvert.DeserializeObject<Transaction>(message);
            }
            catch (Exception e)
            {
                log.LogError($"Invalid payload received, input received was: {message}");
                telemetry.TrackEvent("Bad request received");
                telemetry.TrackException(e);
                transaction = null;
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "The request did not match the required schema");
            }

            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of {transaction.Amount}");
        }
    }
}
