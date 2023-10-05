using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Containers.ContainerRegistry;
using Azure.ResourceManager.ContainerRegistry.Models;

namespace azFunctions
{
    public static class func1
    {
        [FunctionName("func1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            //var acrConnInfo = await GetAcrConnInfoAsync();

            
            //ContainerRegistryClientOptions options = new ContainerRegistryClientOptions()
            //{
            //    Retry =
            //    {
            //             Delay= TimeSpan.FromSeconds(2),
            //             MaxDelay = TimeSpan.FromSeconds(8),
            //             MaxRetries = 3,
            //             Mode = RetryMode.Exponential
            //            }  
            //};
            
            //var client = new ContainerRegistryClient(new Uri("https://k8stestacreastus2.azurecr.io"), new DefaultAzureCredential(), options);
            //var repository = "<repository-name>";

            //var repo = await client.GetRepositoryNamesAsync();
            //var tags = await client.Tags.ListImageNamesAsync(repository);
            //foreach (var tag in tags)
            //{
            //    Console.WriteLine(tag);
            //}




            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;




            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        //public static async Task<acrConInfo> GetAcrConnInfoAsync()
        //{
        //    SecretClientOptions options = new SecretClientOptions()
        //    {
        //        Retry = {
        //                 Delay= TimeSpan.FromSeconds(2),
        //                 MaxDelay = TimeSpan.FromSeconds(8),
        //                 MaxRetries = 3,
        //                 Mode = RetryMode.Exponential
        //                }  
        //    };

        //    var client = new SecretClient(new Uri("https://kvk8stest.vault.azure.net/"), new DefaultAzureCredential(), options);

        //    KeyVaultSecret secAcrEP = await client.GetSecretAsync("acr-endpoint");
        //    KeyVaultSecret secAcrToken = await client.GetSecretAsync("acr-token");            

        //    return new acrConInfo()
        //    {
        //        AcrEP = secAcrEP.Value,
        //        AcrToken = secAcrToken.Value
        //    };
        //}
    }
}
