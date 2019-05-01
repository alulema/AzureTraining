using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Alulema.Function.Models;

namespace Alulema.Function
{
    public static class PostUsers
    {
        [FunctionName("PostUsers")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(message);

            return req.CreateResponse(HttpStatusCode.OK, $"You created a user called {user.FirstName} {user.LastName}");
        }
    }
}
