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
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace azFunctions
{
    public static class acrFunctions
    {
        private static string queueConStr = "DefaultEndpointsProtocol=https;AccountName=k8teststor;AccountKey=UBo5aA6+AZa5+1A79W8pZ0IhtVJkDkbfMJJ/gKPtl6T5k6fkUUrDx9X/72poW+1Ffz4r1bIQBj7e+ASte/Dkfg==;EndpointSuffix=core.windows.net";

        [FunctionName("pushHook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // change the logging mechanism to use the ILogger interface
            // add try catch block to catch any exceptions
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            if (data != null && "ping".Equals(data.action.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new StatusCodeResult(204);
            }
            
            if (data != null && data.request != null && data.target != null)
            {               

                ACRImagePushInfo imgPsInf = new ACRImagePushInfo
                {                    
                    plID = data.id,
                    plTimestamp = data.timestamp,
                    plAction = data.action,
                    pltgMediaType = data.target.mediaType,
                    pltgSize = data.target.size,
                    pltgDigest = data.target.digest,
                    pltgLength = data.target.length,
                    pltgRepository = data.target.repository,
                    pltgTag = data.target.tag,
                    reqID = data.request.id,
                    reqHost = data.request.host,
                    reqMethod = data.request.method,
                    reqUserAgent = data.request.userAgent
                };

                // submit the new object to the queue
                
                QueueClient queue = new QueueClient(queueConStr, "imagequeue");
                string message = System.Text.Json.JsonSerializer.Serialize(imgPsInf);

                await InsertMessageAsync(queue, message);

                return new OkResult();
            }
            return new BadRequestObjectResult("Invalid payload received");




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




            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;




            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);
        }

        static async Task InsertMessageAsync(QueueClient qc, string newMessage)
        {
            //if (null != await theQueue.CreateIfNotExistsAsync())
            //{
            //    Console.WriteLine("The queue was created.");
            //}

            await qc.SendMessageAsync(newMessage);
        }


        [FunctionName("purgeRepository")]
        public static void purgeRepository(
            [QueueTrigger("imagequeue")] string queueMessage, ILogger log)
        {
            //ACRImagePushInfo aCRImagePushInfo = System.Text.Json.JsonSerializer.Deserialize<ACRImagePushInfo>(queueMessage);
            
            
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
