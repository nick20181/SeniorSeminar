﻿using Custodial.Addressing.Service.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Service_Settings
{
    public interface IServiceSettings
    {
        INetworkSettings networkSettings { get; }
        IDatabaseSettings databaseSettings { get; }
        Task InitServiceSettingsAsync(Assembly assembly = null, string resourceFile = "serviceSettings.json");
    }
}
