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
        public CASHttpConnection(IServiceSettings settings, Assembly assembly, ILogger log)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File($"Logs\\LogCASWorker{DateTime.Now.ToString("yyyyMMdd_HHmm")}.txt")
            .CreateLogger();
            this.settings = settings;
        }

        public async Task workerThreadAsync(string serviceName, string shortName, string discription)
        {
            Log.Information("Starting CASWorker");
            while (true)
            {
                await PostAsync(serviceName, shortName, discription);
                Log.Information("CASWorker Timer started");
                Thread.Sleep(15000);
                Log.Information("CASWorker Timer Ended");
            }
        }

        public async Task PostAsync(string serviceName, string shortName, string discription)
        {
            Log.Information(settings.casSettings.address + ":" + settings.casSettings.port);
            Log.Information($"http://{settings.casSettings.address}:{settings.casSettings.port}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp($"http://{settings.casSettings.address}:{settings.casSettings.port}");
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new Microservice()
                {
                    settings = settings,
                    discription = discription,
                    serviceName = serviceName,
                    shortName = shortName,
                    timeCreated = DateTime.UtcNow.Ticks,
                    isDeleted = false
                });
                streamWriter.Write(json);
                streamWriter.Close();
            }
                string result;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            Log.Information($"{result}");
        }
    }
}
