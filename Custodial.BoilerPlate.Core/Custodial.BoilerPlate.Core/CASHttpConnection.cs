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
                timeCreated = DateTime.UtcNow.Ticks,
                isDeleted = false,
                iD = id
            };
            url = $"http://{settings.casSettings.address}:{settings.casSettings.port}/Addressing";
        }

        public async Task Start()
        {
            Log.Information("Starting CASWorker");
            Thread thread = new Thread(async () => await this.PostAsync());
            while (true)
            {
                await PostAsync();
                Log.Information("CASWorker Timer started");
                Thread.Sleep(15000);
                Log.Information("CASWorker Timer Ended");
            }
        }

        public async Task PostAsync()
        {
            string json = JsonConvert.SerializeObject(service);
            StringContent data;
            if (!serviceExists)
            {
                data = new StringContent(json, Encoding.UTF8, "application/json");
                var getResponse = client.GetAsync($"url/servicename/{service.serviceName}");
            }
            Log.Information(settings.casSettings.address + ":" + settings.casSettings.port);
            Log.Information($"http://{settings.casSettings.address}:{settings.casSettings.port}");
            Log.Information("Made Request");
            data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            Log.Information($"Request Sent");
            string result = (await response.Content.ReadAsStringAsync());
            id = JsonConvert.DeserializeObject<Microservice>(result).iD;
            Log.Information($"{result}");
        }
    }
}
