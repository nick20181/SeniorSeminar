﻿using Custodial.BoilerPlate.Service_Settings.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings
{
    public class ServiceSettings : IServiceSettings
    {
        [JsonIgnore]
        private IResourceLoader resourceLoader = new ResourceLoader();
        public INetworkSettings networkSettings { get; set; } = new NetworkSettings();

        public IDatabaseSettings databaseSettings { get; set; } = new DatabaseSettings();

        [JsonIgnore]
        public string certSSL = $"cert";

        [JsonIgnore]
        public string passwordSSL = $"pass";

        public Task InitServiceSettingsAsync(Assembly assembly = null, string resourceFile = "ServiceSettings.json")
        {
            if (assembly.Equals(null))
            {
                assembly = this.GetType().Assembly;
            }
            string embeddedString = resourceLoader.GetEmbeddedResourceString(assembly, resourceFile);
            var temp = JsonConvert.DeserializeObject<ServiceSettings>(embeddedString);
            networkSettings = temp.networkSettings;
            databaseSettings = temp.databaseSettings;
            networkSettings.InitNetworkSettingsAsync();
            return Task.CompletedTask;
        }
    }
}