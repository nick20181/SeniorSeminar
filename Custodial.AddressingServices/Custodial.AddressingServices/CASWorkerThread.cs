using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Custodial.AddressingServices
{
    public class CASWorkerThread
    {
        private bool running = true;
        public IDatabase database { get; set; }
        public IServiceSettings settings = new ServiceSettings();

        public CASWorkerThread()
        {
            settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                running = false;
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase<Microservice>(settings.databaseSettings, new BsonMicroserviceConverter());
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
                running = false;
            }
            else
            {
                running = false;
            }

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File($"Logs\\LogCASWorker{DateTime.Now.ToString("yyyyMMdd_HHmm")}.txt")
            .CreateLogger();
            Log.Information($"Worker Thread Created");
        }

        public async Task MainWorkerThreadAsync(long timeout)
        {
            while (running)
            {
                Log.Information("Checking services");
                foreach (var ms in await database.ReadAsync())
                {
                    Log.Information($"Checking {ms.ToJson()}");
                    long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    Log.Information($"{now}:{ms.timeCreated}");
                    Log.Information($"{((Microservice)ms).serviceName} is at {now - ms.timeCreated} out of {timeout}");
                    if (now - ms.timeCreated > timeout)
                    {
                        Log.Information("Timed out");
                        if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                        {
                        }
                        else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                        {
                            MongoDatabase<Microservice> db = (MongoDatabase<Microservice>)database;
                            await db.DeleteAsync(ms.iD);
                        }
                        else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                        {
                        }
                    }
                    Log.Information("\n");
                }
                Thread.Sleep((int)timeout);
            }
        }
    }
}
