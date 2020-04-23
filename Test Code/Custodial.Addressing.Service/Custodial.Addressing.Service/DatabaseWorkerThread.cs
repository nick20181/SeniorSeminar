using Custodial.Addressing.Service.Databases;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service
{
    public class DatabaseWorkerThread
    {
        private bool running = true;
        public IDatabase database { get; set; }
        public IServiceSettings settings = new ServiceSettings();

        public DatabaseWorkerThread()
        {
            settings.InitServiceSettingsAsync();
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                running = false;
            } else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase(settings.databaseSettings);
            } else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
                running = false;
            } else
            {
                running = false;
            }
        }

        public async Task MainWorkerThreadAsync(long timeout)
        {
            while (running)
            {
                foreach (var ms in await database.ReadAsync())
                {
                    Console.WriteLine($"Checking {ms.ToJson()}");
                    long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    Console.WriteLine($"{now}:{ms.timeCreated}");
                    Console.WriteLine($"{((Microservice)ms).serviceName} is at {now - ms.timeCreated} out of {timeout}");
                    if (now-ms.timeCreated > timeout)
                    {
                        Console.WriteLine("Timed out");
                        if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                        {
                        }
                        else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                        {
                            MongoDatabase db = (MongoDatabase)database;
                            await db.RemoveAsync(ms);
                        }
                        else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                        {
                        }
                    }
                    Console.WriteLine("\n");
                    Thread.Sleep((int)timeout);
                }
            }
        }

    }
}
