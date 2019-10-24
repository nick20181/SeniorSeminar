using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemicalController : ControllerBase
    {
        static private IDatabase datebase = new MongoDatabase();
        private IChemicalFactory ChemFactory = new ChemicalFactory(datebase);

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
            List<IChemical> chemicalToGet = await ChemFactory.ReadChemicalAsync(name);
            foreach (var chemical in chemicalToGet)
            {
                if (chemicalToGet == null)
                {
                    toReturn.Add(new Chemical().NullChemical().ToJson());
                }
                else
                {
                    toReturn.Add(chemical.ToJson());
                }
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
            string result = (await ChemFactory.CreateChemicalAsync(name, manufacturer, productIdentifier, sigWords, hazardStatements, precautionStatements)).ToJson();
            if (result.Contains($"null"))
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
            } else
            {
                string toReturn = "";
                List<IChemical> results = await ChemFactory.ReadChemicalAsync(nameOriginal);
                foreach (var chemical in results)
                {
                    toReturn = $"{toReturn}" + chemical.UpdateChemicalAsync(new Chemical()
                    {
                        chemicalName = nameUpdated,
                        DB = ChemFactory.getDB()
                    }).ToJson();
                }
                return toReturn;
            }
        }

        [Route("Delete/{name}")]
        public async Task<string> DeleteAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return $"Failed to delete {name}";
            } else
            {
                string toReturn = "";
                List<IChemical> results = await ChemFactory.ReadChemicalAsync(name);
                foreach (var chemical in results)
                {
                    toReturn = $"{toReturn}" + chemical.ToJson();
                }
                return toReturn;
            }
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

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
