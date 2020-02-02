using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core
{
    public class CASHttpConnection
    {
        public IServiceSettings settings;
        private string id = "ID";
        private Microservice service;
        private bool serviceExists = false;
        private HttpClient client = new HttpClient();
        private string url = "";
        private string json = "";
        private bool isDeleted = true;
    public CASHttpConnection(IServiceSettings settings, Assembly assembly, ILogger log, string serviceName, string shortName, string discription)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File($"Logs\\LogCASWorker{DateTime.Now.ToString("yyyyMMdd_HHmm")}.txt")
            .CreateLogger();
            this.settings = settings;
            service = new Microservice()
            {
                settings = settings,
                discription = discription,
                serviceName = serviceName,
                shortName = shortName,
                timeCreated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                isDeleted = false,
                iD = id
            };
            url = $"http://{settings.casSettings.address}:{settings.casSettings.port}/Addressing";
            json = JsonConvert.SerializeObject(service);
        }

        public async Task Start()
        {
            Log.Information("Starting CASWorker");
            Thread thread = new Thread(async () => await this.PostAsync());
            while (true)
            {
                await GetAsync();
                if (serviceExists)
                {
                    await UpdateAsync();
                } else
                {
                    await PostAsync();
                }
            }
        }

        public async Task PostAsync()
        {
            Log.Logger.Information($"Posting Service");
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync($"{url}", postData);
            string responseContent = await postResponse.Content.ReadAsStringAsync(); Log.Logger.Information($"Received content: {responseContent}");
            Log.Logger.Information($"Received content: {responseContent}");
            if (String.IsNullOrEmpty(responseContent) || !responseContent.Contains("{"))
            {
                Log.Logger.Information($"Post response is empty");
            }
            else
            {
                Microservice postService = JsonConvert.DeserializeObject<Microservice>(responseContent);
                if (service.serviceName.Equals(postService.serviceName) && postService.isDeleted == false)
                {
                    service.iD = postService.iD;
                    Log.Logger.Information($"Service added to Addressing.");
                    serviceExists = true;
                }
                else
                {
                    Log.Logger.Information($"Service Does not exist in addressing. Failed to post Service.");
                    serviceExists = false;
                }
            }
        }

        public async Task UpdateAsync()
        {
            Log.Logger.Information($"Createing Updated Service Now:");
            Microservice updateService = new Microservice()
            {
                settings = service.settings,
                discription = service.discription,
                serviceName = service.serviceName,
                shortName = service.shortName,
                timeCreated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                isDeleted = false,
                iD = service.iD
            };
            Log.Logger.Information($"Updateing service from addressing: {json} to: {JsonConvert.SerializeObject(updateService)}");
            var putData = new StringContent(JsonConvert.SerializeObject(updateService), Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync($"{url}/{service.iD}", putData);
            string responseContent = await updateResponse.Content.ReadAsStringAsync();
            Log.Logger.Information($"Received content: {responseContent}");
            if (String.IsNullOrEmpty(responseContent) || !responseContent.Contains("{"))
            {
                Log.Logger.Information($"Put response is empty");
            }
            else
            {
                Microservice putService = JsonConvert.DeserializeObject<Microservice>(responseContent);
                if (service.serviceName.Equals(putService.serviceName) && service.iD.Equals(putService.iD) && putService.isDeleted == false && service.timeCreated.Equals(updateService.timeCreated))
                {
                    Log.Logger.Information($"Service Exists in Addressing.");
                    isDeleted = false;
                }
                else
                {
                    Log.Logger.Information($"Service Does not exist in addressing. Failed to update Service.");
                    isDeleted = true;
                }
            }
        }

        public async Task GetAsync()
        {
            Log.Logger.Information($"Getting service from Addressing: {service.serviceName}");
            var getReponse = await client.GetAsync($"{url}/serviceName/{service.serviceName}");
            string responseContent = await getReponse.Content.ReadAsStringAsync();
            Log.Logger.Information($"Received content: {responseContent}");
            if (String.IsNullOrEmpty(responseContent) || !responseContent.Contains("{"))
            {
                Log.Logger.Information($"Service Does not exist in addressing.");
                serviceExists = false;
            } else
            {
                Microservice getService = JsonConvert.DeserializeObject<Microservice>(responseContent);
                if (service.serviceName.Equals(getService.serviceName))
                {
                    service.iD = getService.iD;
                    Log.Logger.Information($"Service Exists in Addressing.");
                    serviceExists = true;
                    isDeleted = getService.isDeleted;
                } else
                {
                    Log.Logger.Information($"Service Does not exist in addressing. Failed to Identify Service received.");
                    serviceExists = false;
                    isDeleted = getService.isDeleted;
                }
            }
        }
    }
}
