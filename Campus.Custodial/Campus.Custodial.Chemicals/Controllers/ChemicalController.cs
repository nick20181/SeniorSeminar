using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using static Campus.Custodial.Chemicals.Chemical;

//+
namespace Campus.Custodial.Chemicals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemicalController : ControllerBase
    {
        private string mongoConnectionip = "";
        private IDatabase datebase;
        private IChemicalFactory ChemFactory;
        
        public ChemicalController()
        {
            //Console.WriteLine($"MongoDB connection address: ");
            //mongoConnectionip = Console.ReadLine();
            datebase = new MongoDatabase($"mongodb://127.0.0.1:27017/db");
            //datebase = new MongoDatabase();
            var x = (MongoDatabase) datebase;
            Console.WriteLine($"Mongo connected at {x.GetMongoConnection()}");
            ChemFactory = new ChemicalFactory(datebase);
        }


        [Route("Get")]
        public async Task<string> GetAllAsync()
        {
            List<IChemical> temp = await ChemFactory.ReadAllChemicalsAsync();
            List<IChemical> toReturn = new List<IChemical>();
            foreach (var x in temp)
            {
                if (!x.GetDeletedStatus())
                {
                    toReturn.Add(x);
                }
            }
            return JsonConvert.SerializeObject(toReturn);
        }

        [Route("Get/{name}")]
        public async Task<List<string>> GetAsync(string name)
        {
            List<string> toReturn = new List<string>();
            if (string.IsNullOrEmpty(name))
            {
                toReturn.Add(new Chemical().NullChemical().ToJson());
            }
            IChemical chemicalToGet = await ChemFactory.ReadChemicalAsync(new Chemical()
            {
                chemicalName = name
            });
                if (chemicalToGet.ToJson().Equals(new Chemical().NullChemical()))
                {
                    toReturn.Add(new Chemical().NullChemical().ToJson());
                }
                else
                {
                    toReturn.Add(chemicalToGet.ToJson());
                }
            return toReturn;
        }

        [Route("Post/{name}")]
        public async Task<string> PostAsync(string name)
        {
            List<string> hazardStatements = new List<string>()
            {
                $"Place Holder"
            };
            List<string> precautionStatements = new List<string>()
            {
                $"Place Holder"
            };
            List<signalWords> sigWords = new List<signalWords>();
            String productIdentifier = $"Place Holder";
            Manufacturer manufacturer = new Manufacturer()
            {
                name = $"place Holder",
                address = $"place Holder",
                phoneNumber = $"place Holder"
            };
            string result = ((await ChemFactory.CreateChemicalAsync(new Chemical()
            {
                chemicalName = name,
                hazardStatements = hazardStatements,
                precautionStatements = precautionStatements,
                sigWords = sigWords,
                productIdentifier = productIdentifier,
                manufacturer = manufacturer
            })).ToJson());
            if (result.Contains(new Chemical().NullChemical().ToJson()))
            {
                return $"Failed to Post";
            }
            return result;

        }

        [Route("Put/{nameOriginal}/{nameUpdated}")]
        public async Task<string> PutAsync(string nameOriginal, string nameUpdated)
        {
            if (string.IsNullOrEmpty(nameOriginal) && string.IsNullOrEmpty(nameUpdated))
            {
                return $"Failed to update {nameOriginal} to {nameUpdated}";
            }
            else
            {
                IChemical result = await ChemFactory.ReadChemicalAsync(new Chemical() 
                {
                    chemicalName = nameUpdated 
                });
                return result.ToJson();
            }
        }

        [Route("Delete/{name}")]
        public async Task<string> DeleteAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return $"Failed to delete {name}";
            }
            else
            {
                IChemical results = await ChemFactory.ReadChemicalAsync(new Chemical()
                {
                    chemicalName = name
                });
                return results.ToJson();
            }
        }

        public async Task cleanDataBaseAsync()
        {
            IDatabase toClean = ChemFactory.getDB();
            foreach (var y in await toClean.ReadAllChemicalAsync())
            {
                await toClean.RemoveChemicalAsync(y);
            }
        }

        public static async Task Main(string[] args)
        {
            string internalIP = $"http://localhost:5000";
            IPAddress[] localIPs = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            //IPAddress[] localIPs = new IPAddress[0];
            foreach (var addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    internalIP = $"{internalIP};http://{addr}:5000";
                }
            }

            //CreateHostBuilder(args).Build().Run();
            Console.WriteLine($"Starting on addresses{internalIP}");
            var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseUrls(internalIP)
                    .Build();

            host.Run();
            BsonClassMap.RegisterClassMap<Manufacturer>();
            BsonClassMap.RegisterClassMap<Chemical>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
